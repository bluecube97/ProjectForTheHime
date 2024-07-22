package com.matchai.board.controller;

import com.matchai.board.service.GameService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import javax.servlet.http.HttpServletRequest;
import java.util.HashMap;
import java.util.List;

@RequestMapping("/board/*")
@Controller
public class GameController {
    @Autowired
    GameService gamesvc;

    @GetMapping("/game")
    public ModelAndView getGameList(ModelAndView mv, HttpServletRequest req){
        List<HashMap<String,Object>>gamelist = gamesvc.getGameList();
        mv.addObject("gamelist", gamelist);
        mv.setViewName("game");
        return mv;
    }
}
