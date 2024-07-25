package com.matchai.board.controller;

import com.matchai.board.service.DataCenterService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.servlet.ModelAndView;

import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

@RequestMapping("/datacenter/*")
@Controller
public class DataCenterController {

    @Autowired
    private DataCenterService datacentersvc;

    @GetMapping("/main")
    public void mainDataCenter(@RequestParam(required = false, name = "sortBy") String sortBy,
                               @RequestParam(required = false, name = "sortType") String sortType,
                               ModelAndView mv) {
        // 뷰 이름을 정의합니다.
        mv.setViewName("datacenter");
        // 데이터베이스에서 구단 정보를 가져옵니다.
        List<Map<String, Object>> kboBatList = datacentersvc.getKboBatList();
        List<Map<String, Object>> kboPitList = datacentersvc.getKboPitList();
        List<Map<String, Object>> mlbBatList = datacentersvc.getMlbBatList();
        List<Map<String, Object>> mlbPitList = datacentersvc.getMlbPitList();
        // 정렬기준 목록을 리스트에서 가져옵니다.
        Set<String> sortKeySet = new HashSet<>(kboBatList.get(0).keySet());
        sortKeySet.addAll(kboPitList.get(0).keySet());
        // 정렬을 수행한 뒤 뷰에 데이터를 전달합니다.
        sortData(kboBatList, kboPitList, mlbBatList, mlbPitList, sortBy, sortType, sortKeySet, mv);
    }

    @PostMapping("/sort")
    public ModelAndView sortData(List<Map<String, Object>> kboBatList,
                                 List<Map<String, Object>> kboPitList,
                                 List<Map<String, Object>> mlbBatList,
                                 List<Map<String, Object>> mlbPitList,
                                 @RequestParam(required = false, name = "sortBy") String sortBy,
                                 @RequestParam(required = false, name = "sortType") String sortType,
                                 Set<String> sortKeySet,
                                 ModelAndView mv) {
        // 데이터를 정렬합니다.
        kboBatList = datacentersvc.sortList(kboBatList, sortBy, sortType);
        kboPitList = datacentersvc.sortList(kboPitList, sortBy, sortType);
        mlbBatList = datacentersvc.sortList(mlbBatList, sortBy, sortType);
        mlbPitList = datacentersvc.sortList(mlbPitList, sortBy, sortType);
        // 뷰에 데이터를 전달합니다.
        mv.addObject("kboBatList", kboBatList);
        mv.addObject("kboPitList", kboPitList);
        mv.addObject("mlbBatList", mlbBatList);
        mv.addObject("mlbPitList", mlbPitList);
        mv.addObject("sortList", sortKeySet);

        return mv;
    }
}
