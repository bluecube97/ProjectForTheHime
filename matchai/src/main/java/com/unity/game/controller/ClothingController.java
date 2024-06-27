package com.unity.game.controller;

import com.unity.game.service.ClothingService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.Map;

@RequestMapping("/api/clothing")
@RestController
public class ClothingController {

    @Autowired
    private ClothingService clothingService;

    @GetMapping("/list")
    public List<Map<String, Object>> GetClothingList(){
        List<Map<String, Object>> testList = clothingService.getClothingList();
        for (Map<String, Object> map : testList) {
            System.out.println(map);
        }
        // return clothingService.getClothingList();
        return testList;
    }

    @GetMapping("/buy")
    public List<Map<String, Object>> GetClothingBuyList(){
        return clothingService.getClothingBuyList();
    }

    public String GetUserInfoFromDB(){
        return "test";
    }
}
