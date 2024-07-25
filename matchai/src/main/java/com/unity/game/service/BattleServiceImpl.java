package com.unity.game.service;

import com.unity.game.dao.BattleDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class BattleServiceImpl implements BattleService {

    @Autowired
    private BattleDao battleDao;

    @Override
    public List<Map<String, Object>> GetMobList(List<Integer> list) {
        return battleDao.GetMobList(list);
    }
}
