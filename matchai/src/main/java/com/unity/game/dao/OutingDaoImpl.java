package com.unity.game.dao;

import com.unity.game.mapper.OutingMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Repository
public class OutingDaoImpl implements OutingDao {

    @Autowired
    private OutingMapper outingMapper;

    @Override
    public List<Map<String, Object>> getClothingList() {
        return outingMapper.getClothingList();
    }

    @Override
    public List<Map<String, Object>> getClothingBuyList() {
        return outingMapper.getClothingBuyList();
    }

    @Override
    public List<Map<String, Object>> getSmithyList() {
        return outingMapper.getSmithyList();
    }

    @Override
    public List<Map<String, Object>> getSmithyBuyList() {
        return outingMapper.getSmithyBuyList();
    }

    @Override
    public List<Map<String, Object>> gethospitalBuyList() {
        return outingMapper.gethospitalBuyList();
    }

    @Override
    public void setafterHeal(HashMap<String, Object> map) {
        outingMapper.setafterHeal(map);
    }

    @Override
    public List<Map<String, Object>> getvarietyStoreList() {
        return outingMapper.getvarietyStoreList();
    }

    @Override
    public List<Map<String, Object>> getrestaurantList() {
        return outingMapper.getrestaurantList();
    }

    @Override
    public List<Map<String, Object>> getquestList() {
        return outingMapper.getquestList();
    }

    @Override
    public void updatequestFlag(HashMap<String, Object> map) {
        outingMapper.updatequestFlag(map);
    }


}
