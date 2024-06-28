package com.unity.game.dao;

import com.unity.game.mapper.InventoryMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class InventoryDaoImpl implements InventoryDao {

    @Autowired
    private InventoryMapper inventoryMapper;

    @Override
    public void GetSellThing(HashMap<String, Object> map) {
        inventoryMapper.GetSellThing(map);
    }

    @Override
    public Map<String, Object> GetUserInfoFromDB(String param) {
        return inventoryMapper.GetUserInfoFromDB(param);
    }

    @Override
    public List<Map<String, Object>> GetInventoryList() {
        return inventoryMapper.GetInventoryList();
    }

    @Override
    public void Setpayment(HashMap<String, Object> map) {
        inventoryMapper.Setpayment(map);
    }

    @Override
    public void SetpurchaseUpdate(HashMap<String, Object> map) {
        inventoryMapper.SetpurchaseUpdate(map);
    }

    @Override
    public void SetpurchaseInsert(HashMap<String, Object> map) {
        inventoryMapper.SetpurchaseInsert(map);
    }

    @Override
    public void SetCreatePayment(HashMap<String, Object> map) {
        inventoryMapper.SetCreatePayment(map);

    }

    @Override
    public void SetCreateUpdate(HashMap<String, Object> map) {
        inventoryMapper.SetCreateUpdate(map);

    }

    @Override
    public void SetCreateInsert(HashMap<String, Object> map) {
        inventoryMapper.SetCreateInsert(map);

    }
}
