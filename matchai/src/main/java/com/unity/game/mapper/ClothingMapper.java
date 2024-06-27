package com.unity.game.mapper;

import java.util.List;
import java.util.Map;

public interface ClothingMapper {
    List<Map<String, Object>> getClothingList();

    List<Map<String, Object>> getClothingBuyList();
}
