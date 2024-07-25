package com.unity.game.controller;

import com.unity.game.service.BattleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.Map;

@RequestMapping("/battle")
@RestController
public class BattleController {

    @Autowired
    private BattleService battleService;

    @GetMapping("/moblist")
    public List<Map<String, Object>> GetMobList(@RequestBody List<Integer> list){
        return battleService.GetMobList(list);
    }
}
