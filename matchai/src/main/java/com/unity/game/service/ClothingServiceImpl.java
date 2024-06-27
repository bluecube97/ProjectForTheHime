package com.unity.game.service;

import com.unity.game.dao.ClothingDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class ClothingServiceImpl implements ClothingService {

    @Autowired
    private ClothingDao clothingDao;

    @Override
    public List<Map<String, Object>> getClothingList() {
        return clothingDao.getClothingList();
    }

    @Override
    public List<Map<String, Object>> getClothingBuyList() {
        return clothingDao.getClothingBuyList();
    }
}
