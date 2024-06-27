package com.unity.game.service;

import java.util.List;
import java.util.Map;

public interface ClothingService {
    List<Map<String, Object>> getClothingList();

    List<Map<String, Object>> getClothingBuyList();
}
