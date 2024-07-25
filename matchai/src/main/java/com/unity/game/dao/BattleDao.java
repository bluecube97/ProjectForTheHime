package com.unity.game.dao;

import java.util.List;
import java.util.Map;

public interface BattleDao {
    List<Map<String, Object>> GetMobList(List<Integer> list);
}
