package com.unity.game.controller;

import com.unity.game.service.ConvService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpSession;
import java.util.Map;

@RequestMapping("/api/conv")
@RestController
public class ConvController {

    @Autowired
    private ConvService convService;


    @PostMapping("/get")
    public String getConv(@RequestBody String userConv) {
        return convService.getConv(userConv);
    }

    @GetMapping("/save")
    public String Save(HttpSession session) {
        Map<String,Object> map = (Map<String, Object>) session.getAttribute("userInfo");
        String pid = map.get("useremail").toString();
        return convService.saveGame(pid);
    }
    @GetMapping("/load")
    public String Load(HttpSession session) {
        Map<String,Object> map = (Map<String, Object>) session.getAttribute("userInfo");
        String pid = map.get("useremail").toString();
        return convService.loadGame(pid);
    }
}
