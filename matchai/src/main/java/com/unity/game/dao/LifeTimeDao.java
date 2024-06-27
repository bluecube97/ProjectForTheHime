package com.unity.game.dao;

import java.util.List;
import java.util.Map;

public interface LifeTimeDao {
    List<Integer> getTodoNo(int year, int month);

    List<Map<String, Object>> getTodoList(List<Integer> list);
}
