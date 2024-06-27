package com.unity.game.dao;

import com.unity.game.mapper.ClothingMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

@Repository
public class ClothingDaoImpl implements ClothingDao{

    @Autowired
    private ClothingMapper clothingMapper;

    @Override
    public List<Map<String, Object>> getClothingList() {
        return clothingMapper.getClothingList();
    }

    @Override
    public List<Map<String, Object>> getClothingBuyList() {
        return clothingMapper.getClothingBuyList();
    }
}
