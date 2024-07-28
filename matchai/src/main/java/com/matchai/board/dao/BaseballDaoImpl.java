package com.matchai.board.dao;

import com.matchai.board.mapper.BaseballMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.HashMap;
import java.util.List;

@Repository
public class BaseballDaoImpl implements BaseballDao {

    @Autowired
    private BaseballMapper gamemapper;

    @Override
    public List<HashMap<String, Object>> getGameList(int offset, int pageSize) {
        return gamemapper.getGameList(offset,pageSize);
    }@Override
    public  List<HashMap<String, Object>> getAIList(int offset, int pageSize) {
        return gamemapper.getAIList(offset,pageSize);
    }

    @Override
    public int geCountForAct() {
        return gamemapper.geCountForAct();
    }

    @Override
    public int getCountForAI() {
        return gamemapper.getCountForAI();
    }


}
