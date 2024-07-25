package com.unity.game.service;

import java.util.List;
import java.util.Map;

public interface BattleService {
    List<Map<String, Object>> GetMobList(List<Integer> list);
}
