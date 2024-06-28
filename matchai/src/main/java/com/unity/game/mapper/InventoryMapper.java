package com.unity.game.mapper;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public interface InventoryMapper {

    void GetSellThing(HashMap<String, Object> map);

    Map<String, Object> GetUserInfoFromDB(String param);

    List<Map<String, Object>> GetInventoryList();

    void SetpurchaseInsert(HashMap<String, Object> map);

    void SetpurchaseUpdate(HashMap<String, Object> map);

    void Setpayment(HashMap<String, Object> map);

    void SetCreatePayment(HashMap<String, Object> map);

    void SetCreateUpdate(HashMap<String, Object> map);

    void SetCreateInsert(HashMap<String, Object> map);

}
