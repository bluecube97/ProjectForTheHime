package com.unity.game.controller;

import com.unity.game.service.InventoryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/inven")
public class InventoryController {

    @Autowired
    private InventoryService inventoryService;

    //유저의 현금 들고옴
    @GetMapping("/cash")
    public Map<String, Object> GetUserInfoFromDB(@RequestParam String pid) {
        return inventoryService.GetUserInfoFromDB(pid);
    }

    @GetMapping("/list")
    public List<Map<String, Object>> GetInventoryList(@RequestParam String pid) {
        List<Map<String, Object>> inventoryList = inventoryService.GetInventoryList(pid);
        for (Map<String, Object> map : inventoryList) {
        }
        return inventoryList;
    }

    //구매 시 update 구문
    @PostMapping("/purchase/update")
    public void SetpurchaseUpdate(@RequestParam String bitem, @RequestParam String itemid, @RequestParam String pid) {
        System.out.println("업데이트 값 : " + bitem + ", ItemID: " + itemid);
        HashMap<String, Object> map = new HashMap<>();
        map.put("bitem", bitem);
        map.put("itemid", itemid);
        map.put("pid",pid);
        inventoryService.SetpurchaseUpdate(map);
    }

    //구매 시 insert 구문
    @PostMapping("/purchase/insert")
    public void SetpurchaseInsert(
            @RequestParam String pid,
            @RequestParam String itemid,
            @RequestParam String cnt,
            @RequestParam String usbl,
            @RequestParam String slot) {

        HashMap<String, Object>map = new HashMap<>();
        map.put("pid", pid);
        map.put("itemid", itemid);
        map.put("cnt", cnt);
        map.put("usbl", usbl);
        map.put("slot", slot);

        inventoryService.SetpurchaseInsert(map);
    }

    //구매 시 결제
    @PostMapping("/purchase/payment")
    public void Setpayment(@RequestParam String pid, @RequestParam String payment) {
        HashMap<String, Object>map = new HashMap<>();
        map.put("pid", pid);
        map.put("payment", payment);
        inventoryService.Setpayment(map);
    }

    //제작 시 update
    @PostMapping("/create/update")
    public void SetCreateUpdate(@RequestParam String pid, @RequestParam String itemid, @RequestParam String itemcnt) {
        System.out.println("제작 시 아이디 "+pid );
        System.out.println("제작하는 아이템 아이디 "+itemid );
        System.out.println("제작 후 아이템 갯수 "+itemcnt );

        HashMap<String, Object>map = new HashMap<>();
        map.put("pid", pid);
        map.put("itemid", itemid);
        map.put("itemcnt", itemcnt);
        inventoryService.SetCreateUpdate(map);

    }
    //제작 시 insert
    @PostMapping("/create/insert")
    public void SetCreateInsert(@RequestParam String pid, @RequestParam String itemid, @RequestParam String itemcnt) {
        HashMap<String, Object>map = new HashMap<>();
        map.put("pid", pid);
        map.put("itemid", itemid);
        map.put("itemcnt", itemcnt);
        inventoryService.SetCreateInsert(map);

    }

    //제작 시 결재
    @PostMapping("/create/payment")
    public void SetCreatePayment(@RequestParam String pid, @RequestParam String itemid, @RequestParam String itemcnt) {
        System.out.println("계산 시 아이디 "+pid );
        System.out.println("소모되는 아이템 아이디 "+itemid );
        System.out.println("소모 후 갯수"+itemcnt );

        HashMap<String, Object>map = new HashMap<>();
        map.put("pid", pid);
        map.put("itemid", itemid);
        map.put("itemcnt", itemcnt);
        inventoryService.SetCreatePayment(map);
    }

    @PostMapping("/sell")
    public void GetSellThing(@RequestParam String itemcnt, @RequestParam String itemid,@RequestParam String pid) {
        HashMap<String, Object>map = new HashMap<>();
        map.put("pid", pid);
        map.put("itemid", itemid);
        map.put("itemcnt", itemcnt);

        inventoryService.GetSellThing(map);
    }
}
