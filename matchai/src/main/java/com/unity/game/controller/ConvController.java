package com.unity.game.controller;

import com.unity.game.service.ConvService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RequestMapping("/api/conv")
@RestController
public class ConvController {

    @Autowired
    private ConvService convService;

    @PostMapping("/get")
    public String getConv(@RequestBody String userConv) {
        return convService.getConv(userConv);
    }
}
