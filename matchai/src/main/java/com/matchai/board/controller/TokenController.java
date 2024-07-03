package com.matchai.board.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import javax.servlet.http.HttpSession;

@RequestMapping("/api")
@RestController
public class TokenController {

    @Autowired
    private HttpSession session;

    // Get 요청을 받으면 토큰 반환
    @GetMapping("/token")
    public String token() {
        System.out.println("zzzzz");
        System.out.println("token(tokencon): " + session.getAttribute("token").toString());
        return session.getAttribute("token").toString();
    }
}
