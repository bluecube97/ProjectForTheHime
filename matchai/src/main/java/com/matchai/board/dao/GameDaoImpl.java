package com.matchai.board.dao;

import com.matchai.board.mapper.GameMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.HashMap;
import java.util.List;

@Repository
public class GameDaoImpl implements GameDao {

    @Autowired
    private GameMapper gamemapper;

    @Override
    public List<HashMap<String, Object>> getGameList() {
        return gamemapper.getGameList();
    }
}
