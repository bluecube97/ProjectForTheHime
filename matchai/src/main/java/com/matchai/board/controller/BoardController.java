package com.matchai.board.controller;

import java.io.IOException;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ClassPathResource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.web.servlet.ModelAndView;

import com.matchai.board.service.BoardService;

@RequestMapping("/board/*")
@Controller
public class BoardController {

	@Autowired
	BoardService boardsvc;

	// 메인화면 창 띄우기
	@GetMapping("/main")
	public ModelAndView mainBoard(ModelAndView mv, HttpServletRequest req) {
		// kbo 당일 경기 수
		int kboCnt = boardsvc.kboCnt();
		System.out.println("kbo경기 수:" + kboCnt);

		// mlb 당일 경기 수
		int mlbCnt = boardsvc.mlbCnt();
		System.out.println("mlb경기 수:" + mlbCnt);

		if (kboCnt >= 1) {
			// 경기 목록 들고오기
			List<HashMap<String, Object>> kboList = boardsvc.kboMatchList();
			System.out.println(kboList);
			mv.addObject("klist", kboList);
		} else {
			// 경기 없다면 멘트 추가
			mv.addObject("kment", "오늘 KBO 경기는 없습니다.");
		}
		if (mlbCnt >= 1) {
			// 경기 목록 들고오기
			List<HashMap<String, Object>> mlbList = boardsvc.mlbMatchList();
			System.out.println(mlbList);
			mv.addObject("mlist", mlbList);
		} else {
			// 경기 없다면 멘트 추가
			mv.addObject("mment", "다음날 MLB 경기는 없습니다.");
		}
		// jsp. 주소 setting
		mv.setViewName("mainboard");
		return mv;
	}

	// 경기예측 페이지
	@GetMapping("/gamedetail")
	public ModelAndView gameAnalysis(HttpServletRequest req, ModelAndView mv, HttpSession session,
			@RequestParam(name = "matchcode") String matchcode,
			@RequestParam(name = "team1") String team1,
			@RequestParam(name = "team2") String team2) {
		
		System.out.println("MatchCode: " + matchcode);
		// 게임 데이터 조회
		HashMap<String, Object> aiData = boardsvc.aiData(matchcode);

		// matchcode가 다르면
		if (aiData == null) {
			LocalDate currentDate = LocalDate.now();
			// 날짜 형식을 지정하여 출력
			DateTimeFormatter dateFormatter = DateTimeFormatter.ofPattern("yyyyMMdd");
			String formattedDate = currentDate.format(dateFormatter);
			matchcode = formattedDate + team1 + team2;
			System.out.println("변경된 matchcode : " + matchcode);
			aiData = boardsvc.aiData(matchcode);
		}

		// boardDB에 값이 있는지 여부 조회
		int count = boardsvc.searchBoard(matchcode);

		// 없으면 board에 ai예측 데이터 넣기
		if (count <= 0) {
			String title = aiData.get("team1name").toString() + " VS " + aiData.get("team2name").toString();
			aiData.put("title", title);
			aiData.put("brdcode", "10");
			aiData.put("adduser", "admin");
			aiData.put("matchcode", matchcode);

			boardsvc.insertAiData(aiData);
		}

		// 댓글을 받아 오기 위한 brdno 조회
		int brdno_ = boardsvc.getBoardNumber(matchcode);
		String brdno = Integer.toString(brdno_);

		// 게시글 댓글 받아옴
		List<HashMap<String, Object>> comment = boardsvc.getCommentList(brdno);
		
		// KBO 경기 목록 들고오기
		List<HashMap<String, Object>> kboList = boardsvc.kboMatchList();

		// MLB 경기 목록 들고오기
		List<HashMap<String, Object>> mlbList = boardsvc.mlbMatchList();
		
		mv.addObject("team1", team1);
		mv.addObject("team2", team2);
		mv.addObject("klist", kboList);
		mv.addObject("mlist", mlbList);
		mv.addObject("comment", comment);
		mv.addObject("aiData", aiData);
		mv.setViewName("gamedetail");

		return mv;
	}

	@PostMapping("/comment")
	@ResponseBody
	public ResponseEntity<Map<String, String>> insertComment(@RequestBody Map<String, String> request,
			HttpSession session) {
		Map<String, String> response = new HashMap<>();

		// 요청에서 댓글 내용 가져오기
		String memo = request.get("memo");
		// 댓글 내용이 없거나 빈 문자열인 경우
		if (memo == null || memo.trim().isEmpty()) {
			response.put("status", "error");
			response.put("message", "댓글은 비워 둘 수 없습니다.");
			return ResponseEntity.badRequest().body(response);
		}

		// 요청에서 matchcode 가져오기
		String matchcode = request.get("matchcode");
		// 세션에서 사용자 정보 가져오기
		HashMap<String, Object> userinfo = (HashMap<String, Object>) session.getAttribute("userInfo");
		String pid = userinfo != null ? (String) userinfo.get("useremail") : null;
		// 로그인되지 않은 사용자
		if (pid == null) {
			response.put("status", "error");
			response.put("message", "로그인 후 이용 해 주세요.");
			return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(response);
		}

		// brdno 조회
		int brdno_;
		try {
			brdno_ = boardsvc.getBoardNumber(matchcode);
		} catch (Exception e) {
			response.put("status", "error");
			response.put("message", "게시글 번호 조회 중 오류가 발생했습니다.");
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response);
		}
		String brdno = Integer.toString(brdno_);

		// 댓글 정보를 담을 맵 생성
		HashMap<String, Object> map = new HashMap<>();
		map.put("pid", pid);
		map.put("memo", memo);
		map.put("brdno", brdno);
		// 댓글을 DB에 저장
		try {
			boardsvc.insertComment(map);
		} catch (Exception e) {
			response.put("status", "error");
			response.put("message", "댓글 저장 중 오류가 발생했습니다.");
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response);
		}

		// 성공적인 응답 반환
		response.put("status", "success");
		response.put("message", "작성 완료 되었습니다.");
		return ResponseEntity.ok(response);
	}

	@GetMapping("/unity")
	public String unity() {
		return "game";
	}

	@PostMapping("/fetchGameData")
	@ResponseBody
	public HashMap<String, Object> fetchGameData(@RequestBody HashMap<String, String> request) {
		String team1Code = request.get("team1Code");
		String team2Code = request.get("team2Code");

		// System.out.println(team1Code + team2Code);
		HashMap<String, Object> gameData = boardsvc.fetchGameData(team1Code, team2Code);

		// JSON 응답을 반환
		if (gameData == null || gameData.isEmpty()) {
			throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Game data not found");
		}


		return gameData;
	}

	@GetMapping("/freeboard")
	public String gotoFreeBoard() {
		return "redirect:/freeboard/main";
	}

	@GetMapping("/resource/Build/UnityBuilder.framework.js.gz")
	@ResponseBody
	public ResponseEntity<ClassPathResource> getCompressedFile() throws IOException {
		ClassPathResource gzFile = new ClassPathResource("Build/UnityBuilder.framework.js.gz");
		HttpHeaders headers = new HttpHeaders();
		headers.add("Content-Encoding", "gzip");
		headers.add("Content-Type", "application/javascript");
		headers.add("Cache-Control", "max-age=31536000, public");

		return new ResponseEntity<>(gzFile, headers, HttpStatus.OK);
	}

	@GetMapping("/curresults")
	public String curResults() {
		return "curresults";
	}

	@PostMapping("/getCurResults")
	@ResponseBody
	public List<HashMap<String, Object>> getCurResults(@RequestBody HashMap<String, Object> requests) {
		String selLeague = (String) requests.get("selLeague");
		String selYear = (String) requests.get("selYear");
		String selMonth = (String) requests.get("selMonth");
		System.out.println(selLeague);
		System.out.println(selYear);
		System.out.println(selMonth);

		List<HashMap<String, Object>> curGamesResults = boardsvc.getCurResults(selLeague, selYear, selMonth);
		System.out.println("컨트롤러 결과1 :" + curGamesResults);
		HashMap<String, Object> map = new HashMap<String, Object>();
		return curGamesResults;
	}

}
