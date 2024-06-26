package com.matchai.board.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
public class HomeController {
	// 루트 주소 보드 메인으로 띄우기
	@RequestMapping("/")
	public String fowordBoard() {
		return "redirect:/board/main";
	}
}
