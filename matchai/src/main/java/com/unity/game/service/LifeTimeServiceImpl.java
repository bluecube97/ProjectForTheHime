package com.unity.game.service;

import com.unity.game.dao.LifeTimeDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class LifeTimeServiceImpl implements LifeTimeService{

    @Autowired
    private LifeTimeDao lifeTimeDao;

    @Override
    public List<Integer> getTodoNo(int year, int month) {
        return lifeTimeDao.getTodoNo(year, month);
    }

    @Override
    public List<Map<String, Object>> getTodoList(List<Integer> list) {
        return lifeTimeDao.getTodoList(list);
    }
}
