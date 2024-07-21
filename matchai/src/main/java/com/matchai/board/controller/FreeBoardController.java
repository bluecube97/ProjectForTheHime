package com.matchai.board.controller;

import com.matchai.board.service.FreeBoardService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import java.util.List;
import java.util.Map;

@RequestMapping("/freeboard/*")
@Controller
public class FreeBoardController {

    @Autowired
    private FreeBoardService freeboardsvc;

    @GetMapping("/main")
    public ModelAndView mainFreeBoard(ModelAndView mv) {
        List<Map<String, Object>> freeBoardList = freeboardsvc.getFreeBoardList();

        for(Map<String, Object> board : freeBoardList) {
            System.out.println("board = " + board);
        }

        mv.addObject("freeboardlist", freeBoardList);

        return mv;
    }

}
