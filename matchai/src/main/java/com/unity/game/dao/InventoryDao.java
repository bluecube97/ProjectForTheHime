package com.unity.game.dao;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public interface InventoryDao {

    void GetSellThing( HashMap<String, Object> map);

    Map<String, Object> GetUserInfoFromDB(String param);

    List<Map<String, Object>> GetInventoryList(String param);

    void Setpayment(HashMap<String, Object> map);

    void SetpurchaseUpdate(HashMap<String, Object> map);

    void SetpurchaseInsert(HashMap<String, Object> map);

    void SetCreatePayment(HashMap<String, Object> map);

    void SetCreateUpdate(HashMap<String, Object> map);

    void SetCreateInsert(HashMap<String, Object> map);

}
