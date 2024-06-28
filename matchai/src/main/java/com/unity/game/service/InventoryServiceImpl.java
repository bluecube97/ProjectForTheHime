package com.unity.game.service;

import com.unity.game.dao.InventoryDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class InventoryServiceImpl implements InventoryService {

    @Autowired
    private InventoryDao inventoryDao;

    @Override
    public void GetSellThing(HashMap<String, Object> map) {
        inventoryDao.GetSellThing(map);
    }

    @Override
    public Map<String, Object> GetUserInfoFromDB(String param) {
        return inventoryDao.GetUserInfoFromDB(param);
    }

    @Override
    public List<Map<String, Object>> GetInventoryList() {
        return inventoryDao.GetInventoryList();
    }

    @Override
    public void SetpurchaseInsert(HashMap<String, Object> map) {
        inventoryDao.SetpurchaseInsert(map);
    }

    @Override
    public void SetpurchaseUpdate(HashMap<String, Object> map) {
        inventoryDao.SetpurchaseUpdate(map);
    }

    @Override
    public void Setpayment(HashMap<String, Object> map) {
        inventoryDao.Setpayment(map);
    }

    @Override
    public void SetCreatePayment(HashMap<String, Object> map) {
        inventoryDao.SetCreatePayment(map);

    }

    @Override
    public void SetCreateUpdate(HashMap<String, Object> map) {
        inventoryDao.SetCreateUpdate(map);

    }

    @Override
    public void SetCreateInsert(HashMap<String, Object> map) {
        inventoryDao.SetCreateInsert(map);

    }
}
