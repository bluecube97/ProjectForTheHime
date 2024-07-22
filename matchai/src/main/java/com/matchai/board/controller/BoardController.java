package com.matchai.board.controller;

import java.io.IOException;
import java.util.HashMap;
import java.util.List;

import javax.servlet.http.HttpServletRequest;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ClassPathResource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
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

	
	@GetMapping("/unity")
	public String unity() {
		return "game";
	}

	// modal에서 페이지 이동으로 수정해서 틀만 많들어놔서 쓰지 않았음
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

	@GetMapping("/gameanalysis")
	public ModelAndView gameAnalysis(HttpServletRequest req, ModelAndView mv, @RequestParam(name = "team1") String team1Code,
            @RequestParam(name = "team2") String team2Code) {
		
	    // team1과 team2 파라미터 값 확인
	    System.out.println("Team1: " + team1Code);
	    System.out.println("Team2: " + team2Code);
	    
	    // 게임 데이터 조회
	    HashMap<String, Object> gameData = boardsvc.fetchGameData(team1Code, team2Code);
	    System.out.println("GameData: " + gameData);

	    // 게임 데이터가 없을 경우 404 에러 처리
	    if (gameData == null || gameData.isEmpty()) {
	        throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Game data not found");
	    }
	    
	    /*
	     * 아직 차트에 데이터 넣기는 실패..
	     * */
	    
		// KBO 경기 목록 들고오기
		List<HashMap<String, Object>> kboList = boardsvc.kboMatchList();

		// MLB 경기 목록 들고오기
		List<HashMap<String, Object>> mlbList = boardsvc.mlbMatchList();
		
	    // gameAnalysis.jsp에 데이터 전달
		mv.addObject("klist", kboList);
		mv.addObject("mlist", mlbList);
		mv.setViewName("gameanalysis");

		return mv;
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

}
