package com.matchai.board.controller;

import javax.servlet.http.HttpServletRequest;

import org.springframework.core.io.ClassPathResource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.servlet.ModelAndView;

import java.io.IOException;

@RequestMapping("/board/*")
@Controller
public class BoardController {
	// 메인화면 창 띄우기
	@GetMapping("/main")
	public ModelAndView mainBoard(ModelAndView mv, HttpServletRequest req) {
		mv.setViewName("mainboard");
		return mv;
	}

	@GetMapping("/unity")
	public String unity(){
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
