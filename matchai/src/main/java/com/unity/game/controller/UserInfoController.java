package com.unity.game.controller;

import com.unity.game.service.UserInfoService;

import org.apache.ibatis.javassist.compiler.Parser;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpSession;
import java.util.HashMap;
import java.util.Map;

@RequestMapping("/api/user")
@RestController
public class UserInfoController {
    @Autowired
    private UserInfoService userInfoService;

    @GetMapping("/login")
    public Map<String,Object> GetUserEmail(HttpSession session){
        Map<String,Object> map = (Map<String, Object>) session.getAttribute("userInfo");
        System.out.println("유저정보 : " + session.getAttribute("userInfo"));
        if(map == null){
            System.out.println("값 안들어옴");
            return null;
        }else{
            String email = map.get("useremail").toString();
            System.out.println(email);
            HashMap<String,Object> dic = new HashMap<>();
            dic.put("useremail",email);
            return dic;

        }
    }
    @PostMapping("/search")
    public int SearchUserInfo(@RequestParam String email){
        return userInfoService.searchUserInfo(email);
    }
    @PostMapping("/insert")
    public void InsertUserInfo(@RequestParam String PID, @RequestParam String PNM, @RequestParam String PSEX){
        Map<String,Object> map = new HashMap<>();
        map.put("PID",PID);
        map.put("PNM",PNM);
        map.put("PSEX",PSEX);
        userInfoService.insertUserInfo(map);
    }

    @PostMapping("/info")
    public Map<String,Object> GetUserInfo(@RequestParam String PID){
        return userInfoService.getUserInfo(PID);

    }
    @PostMapping("/dstats")
    private void SetDstate(@RequestParam String pid, @RequestBody HashMap<String,Object> jsontext){
        System.out.println("딸 스탯에 넣을 유저아이디 값  " + pid);
        System.out.println("딸 스탯  " + jsontext);
        HashMap<String,Object> dstats = (HashMap<String, Object>) jsontext.get("daughter");
        int reple = (int) dstats.get("age");
        String age = String.valueOf(reple);
        dstats.replace("age",age);
        dstats.put("pid",pid);
       userInfoService.setDstate(dstats);
    }

}
