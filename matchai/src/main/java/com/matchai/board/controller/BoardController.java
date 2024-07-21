	package com.matchai.board.controller;

	import java.io.IOException;
	import java.util.HashMap;
	import java.util.List;

	import javax.servlet.http.HttpServletRequest;

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

		@GetMapping("/gamedetail")
		public ModelAndView gameAnalysis(HttpServletRequest req, ModelAndView mv, @RequestParam(name = "matchcode") String matchcode) {
			// team1과 team2 파라미터 값 확인
			System.out.println("MatchCode: " + matchcode);
			// 게임 데이터 조회
			HashMap<String, Object> aiData = boardsvc.aiData(matchcode);
			//boardDB에 값이 있는지 여부 조회
			int count = boardsvc.searchBoard(matchcode);
			//없으면 board에 ai예측 데이터 넣기
			if (count <= 0) {
				String title = aiData.get("team1name").toString() +" VS "+ aiData.get("team2name").toString();
				aiData.put("title",title);
				aiData.put("brdcode","10");
				aiData.put("adduser","admin");
				aiData.put("matchcode",matchcode);

				boardsvc.insertAiData(aiData);
			}

			mv.addObject("aiData", aiData);
			mv.setViewName("gamedetail");

			return mv;
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

			//System.out.println(team1Code + team2Code);
			HashMap<String, Object> gameData = boardsvc.fetchGameData(team1Code, team2Code);

			// JSON 응답을 반환
			if (gameData == null || gameData.isEmpty()) {
				throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Game data not found");
			}

			return gameData;
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

		@GetMapping("/freeboard")
		public ModelAndView gotoFreeBoard(ModelAndView mv) {
			mv.setViewName("freeboard");
			return mv;
		}
	}
