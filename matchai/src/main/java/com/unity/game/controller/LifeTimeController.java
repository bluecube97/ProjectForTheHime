package com.unity.game.controller;

import com.unity.game.service.LifeTimeService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RequestMapping("/api/lifetime")
@RestController
public class LifeTimeController {

    @Autowired
    private LifeTimeService lifeTimeService;

    @GetMapping("/todono/{year}/{month}")
    public List<Integer> GetTodoNo(@PathVariable("year") int year, @PathVariable("month") int month){
        return lifeTimeService.getTodoNo(year, month);
    }

    @PostMapping("/todolist")
    public List<Map<String, Object>> GetTodoList(@RequestBody List<Integer> list){
        return lifeTimeService.getTodoList(list);
    }


}
