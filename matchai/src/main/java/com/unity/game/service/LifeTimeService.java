package com.unity.game.service;

import java.util.List;
import java.util.Map;

public interface LifeTimeService {
    List<Integer> getTodoNo(int year, int month);

    List<Map<String, Object>> getTodoList(List<Integer> list);
}
