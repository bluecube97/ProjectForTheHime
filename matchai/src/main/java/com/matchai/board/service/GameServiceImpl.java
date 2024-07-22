package com.matchai.board.service;

import com.matchai.board.dao.GameDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.List;

@Service
public class GameServiceImpl implements GameService{
    @Autowired
    private GameDao gamedao;

    @Override
    public List<HashMap<String, Object>> getGameList() {
        return gamedao.getGameList();
    }
}
