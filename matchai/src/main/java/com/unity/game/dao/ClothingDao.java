package com.unity.game.dao;

import java.util.List;
import java.util.Map;

public interface ClothingDao {
    List<Map<String, Object>> getClothingList();

    List<Map<String, Object>> getClothingBuyList();
}
