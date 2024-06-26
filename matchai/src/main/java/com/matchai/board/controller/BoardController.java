package com.matchai.board.controller;

import javax.servlet.http.HttpServletRequest;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

@RequestMapping("/board/*")
@Controller
public class BoardController {
	// 메인화면 창 띄우기
	@GetMapping("/main")
	public ModelAndView mainBoard(ModelAndView mv, HttpServletRequest req) {
		mv.setViewName("mainboard");
		return mv;
	}
}
