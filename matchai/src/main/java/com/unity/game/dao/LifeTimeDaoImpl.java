package com.unity.game.dao;

import com.unity.game.mapper.LifeTimeMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

@Repository
public class LifeTimeDaoImpl implements LifeTimeDao {

    @Autowired
    private LifeTimeMapper lifeTimeMapper;

    @Override
    public List<Integer> getTodoNo(int year, int month) {
        return lifeTimeMapper.getTodoNo(year,month);
    }

    @Override
    public List<Map<String, Object>> getTodoList(List<Integer> list) {
        List<Map<String, Object>> todoList = new ArrayList<>();

        for (int todoNo : list) {
            todoList.add(lifeTimeMapper.getTodoList(todoNo));
        }

        return todoList;
    }
}
