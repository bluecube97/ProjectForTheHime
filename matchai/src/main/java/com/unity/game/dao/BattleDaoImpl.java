package com.unity.game.dao;

import com.unity.game.mapper.BattleMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

@Repository
public class BattleDaoImpl implements BattleDao {

    @Autowired
    private BattleMapper battleMapper;

    @Override
    public List<Map<String, Object>> GetMobList(List<Integer> list) {
        List<Map<String, Object>> mobList = new ArrayList<>();

        for (int mobNo : list) {
            mobList.add(battleMapper.GetMobList(mobNo));
        }

        return mobList;
    }
}
