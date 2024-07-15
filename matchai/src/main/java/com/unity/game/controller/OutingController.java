package com.unity.game.controller;

import com.unity.game.service.InventoryService;
import com.unity.game.service.OutingService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RequestMapping("/api/outing")
@RestController
public class OutingController {

    @Autowired
    private OutingService outingService;

    @Autowired
    private InventoryService inventoryService;

    //옷 제작 목록 들고옴
    @GetMapping("/clothing/list")
    public List<Map<String, Object>> GetClothingList() {
        List<Map<String, Object>> testList = outingService.getClothingList();

        return testList;
    }

    //옷 구매 목록 들고옴
    @GetMapping("/clothing/buy")
    public List<Map<String, Object>> GetClothingBuyList() {
        return outingService.getClothingBuyList();
    }

    //대장간 재련 목록
    @GetMapping("/smithy/list")
    public List<Map<String, Object>> GetSmithyList() {
        return outingService.getSmithyList();
    }

    //대장간 구매목록 
    @GetMapping("/smithy/buy")
    public List<Map<String, Object>> GetSmithyBuyList() {
        return outingService.getSmithyBuyList();
    }

    @GetMapping("/varstory/buy")
    public List<Map<String, Object>> GetVarietyStoreList() {
        return outingService.getVarietyStoreList();
    }

    @GetMapping("/hospital/buy")
    public List<Map<String, Object>> GetHospitalBuyList() {
        return outingService.gethospitalBuyList();
    }
    @GetMapping("/restaurant/list")
    public List<Map<String, Object>> GetRestaurantList() {
        return outingService.getrestaurantList();
    }
    @GetMapping("/quest/list")
    public List<Map<String, Object>> GetQuestList() {
        return outingService.getquestList();
    }
    @PostMapping("/quest/flag")
    public void updateQuestFlag(@RequestParam String pid,@RequestParam String sflag, @RequestParam String cflag, @RequestParam int questno) {
        HashMap<String, Object> map = new HashMap<>();
        map.put("sflag", sflag);
        map.put("cflag", cflag);
        map.put("questno", questno);
        outingService.updatequestFlag(map);
    }
    @PostMapping("/hospital/heal")
    public void SetAfterHeal(@RequestParam String pid, @RequestParam String payment, @RequestParam String maxhp) {
        HashMap<String,Object> map = new HashMap<>();
        map.put("pid", pid);
        map.put("payment", payment);
        map.put("maxhp", maxhp);
        outingService.setafterHeal(map);
        inventoryService.Setpayment(map);

    }

}
