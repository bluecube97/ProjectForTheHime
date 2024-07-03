package com.matchai.board.controller;

import javax.servlet.http.HttpServletRequest;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ClassPathResource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.servlet.ModelAndView;

import com.matchai.board.service.BoardService;

import java.io.IOException;
import java.util.HashMap;
import java.util.List;

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
