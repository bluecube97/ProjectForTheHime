package com.matchai.board.controller;

import com.matchai.board.service.BaseballService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.servlet.ModelAndView;

import javax.servlet.http.HttpServletRequest;
import java.util.HashMap;
import java.util.List;

@RequestMapping("/board")
@Controller
public class BaseballController {
    @Autowired
    BaseballService gamesvc;

    @GetMapping("/aibaseball")
    public ModelAndView getAIGameList(ModelAndView mv,@RequestParam(defaultValue = "1") int page, HttpServletRequest req) {
        int pageSize = 10; // Number of items per page
        int offset = (page - 1) * pageSize;

        List<HashMap<String, Object>> ailist = gamesvc.getAIList(offset, pageSize);
        System.out.println(ailist);

        int totalRecords = gamesvc.getCountForAI();

        mv.addObject("ailist", ailist);
        mv.addObject("totalRecords", totalRecords);
        mv.addObject("pageSize", pageSize);
        mv.addObject("currentPage", page);
        mv.setViewName("aibaseball");
        return mv;
    }

    @GetMapping("/actbaseball")
    public ModelAndView getActualGameList(ModelAndView mv, @RequestParam(defaultValue = "1") int page, HttpServletRequest req) {
        int pageSize = 10; // Number of items per page
        int offset = (page - 1) * pageSize;

        List<HashMap<String, Object>> gamelist = gamesvc.getGameList(offset, pageSize);

        int totalRecords = gamesvc.geCountForAct();

        mv.addObject("gamelist", gamelist);
        mv.addObject("totalRecords", totalRecords);
        mv.addObject("pageSize", pageSize);
        mv.addObject("currentPage", page);
        mv.setViewName("actbaseball");
        return mv;
    }
}
