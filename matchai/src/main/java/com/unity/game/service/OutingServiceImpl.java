package com.unity.game.service;

import com.unity.game.dao.OutingDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class OutingServiceImpl implements OutingService {

    @Autowired
    private OutingDao outingDao;

    @Override
    public List<Map<String, Object>> getClothingList() {
        return outingDao.getClothingList();
    }

    @Override
    public List<Map<String, Object>> getClothingBuyList() {
        return outingDao.getClothingBuyList();
    }

    @Override
    public List<Map<String, Object>> getSmithyList() {
        return outingDao.getSmithyList();
    }

    @Override
    public List<Map<String, Object>> getSmithyBuyList() {
        return outingDao.getSmithyBuyList();
    }

    @Override
    public List<Map<String, Object>> gethospitalBuyList() {
        return outingDao.gethospitalBuyList();
    }

    @Override
    public void setafterHeal(HashMap<String, Object> map) {
        outingDao.setafterHeal(map);

    }

    @Override
    public List<Map<String, Object>> getVarietyStoreList() {
        return outingDao.getvarietyStoreList();
    }

    @Override
    public List<Map<String, Object>> getrestaurantList() {
        return outingDao.getrestaurantList();
    }

    @Override
    public List<Map<String, Object>> getquestList() {
        return outingDao.getquestList();
    }

    @Override
    public void updatequestFlag(HashMap<String, Object> map) {
        outingDao.updatequestFlag(map);
    }


}
