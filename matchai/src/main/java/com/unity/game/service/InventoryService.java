package com.unity.game.service;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public interface InventoryService {

    List<Map<String, Object>> GetInventoryList(String param);

    Map<String, Object> GetUserInfoFromDB(String param);

    void SetpurchaseInsert(HashMap<String, Object> map);

    void SetpurchaseUpdate(HashMap<String, Object> map);

    void Setpayment(HashMap<String, Object> map);

    void SetCreatePayment(HashMap<String, Object> map);

    void SetCreateUpdate(HashMap<String, Object> map);

    void SetCreateInsert(HashMap<String, Object> map);

    void GetSellThing(HashMap<String, Object> map);
}
