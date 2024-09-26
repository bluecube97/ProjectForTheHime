package com.matchai.board.controller;

import java.io.IOException;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ClassPathResource;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.client.RestTemplate;
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
	@GetMapping("/aidetail")
	public ModelAndView gameAnalysis(HttpServletRequest req, ModelAndView mv, HttpSession session,
			@RequestParam(name = "matchcode") String matchcode,
			@RequestParam(name = "team1") String team1,
			@RequestParam(name = "team2") String team2) {

		System.out.println("MatchCode: " + matchcode);
		System.out.println(team1);
		System.out.println(team2);
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
		int brdno = boardsvc.getBoardNumber(matchcode);

		// 게시글 댓글 받아옴
		List<HashMap<String, Object>> comment = boardsvc.getCommentList(brdno);


		mv.addObject("team1", team1);
		mv.addObject("team2", team2);
		mv.addObject("comment", comment);
		mv.addObject("aiData", aiData);
		mv.setViewName("aidetail");

		return mv;
	}
	@GetMapping("/actdetail")
	public ModelAndView actGameLog(HttpServletRequest req, ModelAndView mv, HttpSession session,
									 @RequestParam(name = "matchcode") String matchcode,
									 @RequestParam(name = "team1") String team1,
									 @RequestParam(name = "team2") String team2) {

		System.out.println("MatchCode: " + matchcode);
		System.out.println(team1);
		System.out.println(team2);
		// 게임 데이터 조회
		HashMap<String, Object> actData = boardsvc.actData(matchcode);

		// matchcode가 다르면
		if (actData == null) {
			LocalDate currentDate = LocalDate.now();
			// 날짜 형식을 지정하여 출력
			DateTimeFormatter dateFormatter = DateTimeFormatter.ofPattern("yyyyMMdd");
			String formattedDate = currentDate.format(dateFormatter);
			matchcode = formattedDate + team1 + team2;
			System.out.println("변경된 matchcode : " + matchcode);
			actData = boardsvc.actData(matchcode);
		}

		// boardDB에 값이 있는지 여부 조회
		int count = boardsvc.searchBoard(matchcode);

		// 없으면 board에 ai예측 데이터 넣기
		if (count <= 0) {
			String title = actData.get("team1name").toString() + " VS " + actData.get("team2name").toString();
			actData.put("title", title);
			actData.put("brdcode", "10");
			actData.put("adduser", "admin");
			actData.put("matchcode", matchcode);

			boardsvc.insertactData(actData);
		}

		// 댓글을 받아 오기 위한 brdno 조회
		int brdno = boardsvc.getBoardNumber(matchcode);

		// 게시글 댓글 받아옴
		List<HashMap<String, Object>> comment = boardsvc.getCommentList(brdno);

		// KBO 경기 목록 들고오기


		mv.addObject("team1", team1);
		mv.addObject("team2", team2);
		mv.addObject("comment", comment);
		mv.addObject("actData", actData);
		mv.setViewName("actdetail");

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
		int brdno;
		try {
			brdno = boardsvc.getBoardNumber(matchcode);
		} catch (Exception e) {
			response.put("status", "error");
			response.put("message", "게시글 번호 조회 중 오류가 발생했습니다.");
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response);
		}

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

	@GetMapping("/datacenter")
	public String gotoDataCenter(){
		return "redirect:/datacenter/main";

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

	@GetMapping("/refreshScheduleData")
	@CrossOrigin(origins = "http://13.125.238.85:8080")
	public ResponseEntity<String> refreshScheduleData() {
	    RestTemplate restTemplate = new RestTemplate();

	    // HTTP 헤더 설정 (필요한 경우)
	    HttpHeaders headers = new HttpHeaders();
	    headers.add("Content-Type", "application/json");

	    // 필요한 데이터가 있을 경우
	    String requestBody = "{}"; // 필요한 경우 JSON 요청 본문을 작성

	    // HttpEntity 생성 (요청 헤더 및 본문 포함)
	    HttpEntity<String> entity = new HttpEntity<>(requestBody, headers);

	    // 외부 API로 POST 요청 보내기
	    String apiUrl = "http://13.125.238.85:8080/api/v1/schedule/process";
	    ResponseEntity<String> response = restTemplate.exchange(apiUrl, HttpMethod.POST, entity, String.class);

	    // 응답 처리
	    if (response.getStatusCode() == HttpStatus.OK) {
	        return ResponseEntity.ok("스케줄 데이터가 성공적으로 갱신되었습니다.");
	    } else {
	        return ResponseEntity.status(response.getStatusCode()).body("스케줄 데이터 갱신에 실패했습니다.");
	    }
	}

	@GetMapping("/refreshResultsData")
	@CrossOrigin(origins = "http://13.125.238.85:8080")
	public ResponseEntity<String> refreshResultsData() {
	    RestTemplate restTemplate = new RestTemplate();

	    // HTTP 헤더 설정 (필요한 경우)
	    HttpHeaders headers = new HttpHeaders();
	    headers.add("Content-Type", "application/json");

	    // 필요한 데이터가 있을 경우
	    String requestBody = "{}"; // 필요한 경우 JSON 요청 본문을 작성

	    // HttpEntity 생성 (요청 헤더 및 본문 포함)
	    HttpEntity<String> entity = new HttpEntity<>(requestBody, headers);

	    // 외부 API로 POST 요청 보내기
	    String apiUrl = "http://13.125.238.85:8080/api/v1/results/process";
	    ResponseEntity<String> response = restTemplate.exchange(apiUrl, HttpMethod.POST, entity, String.class);

	    // 응답 처리
	    if (response.getStatusCode() == HttpStatus.OK) {
	        return ResponseEntity.ok("결과 데이터가 성공적으로 갱신되었습니다.");
	    } else {
	        return ResponseEntity.status(response.getStatusCode()).body("결과 데이터 갱신에 실패했습니다.");
	    }
	}
}
