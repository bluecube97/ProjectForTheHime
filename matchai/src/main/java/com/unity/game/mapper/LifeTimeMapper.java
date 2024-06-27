package com.unity.game.mapper;

import java.util.List;
import java.util.Map;

public interface LifeTimeMapper {
    List<Integer> getTodoNo(int year, int month);

    Map<String, Object> getTodoList(int todoNo);
}
