package com.unity.game.service;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public interface OutingService {
    List<Map<String, Object>> getClothingList();

    List<Map<String, Object>> getClothingBuyList();


    List<Map<String, Object>> getSmithyList();

    List<Map<String, Object>> getSmithyBuyList();

    List<Map<String, Object>> gethospitalBuyList();

    void setafterHeal(HashMap<String, Object> map);

    List<Map<String, Object>> getVarietyStoreList();

    List<Map<String, Object>> getrestaurantList();

    List<Map<String, Object>> getquestList();

    void updatequestFlag(HashMap<String, Object> map);
}
