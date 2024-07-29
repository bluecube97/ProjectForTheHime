package com.matchai.board.service;

import com.matchai.board.dao.BaseballDao;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.List;

@Service
public class BaseballServiceImpl implements BaseballService {
    @Autowired
    private BaseballDao gamedao;

    @Override
    public List<HashMap<String, Object>> getGameList(int offset, int pageSize) {
        return gamedao.getGameList(offset, pageSize);
    }

    public List<HashMap<String, Object>> getAIList(int offset, int pageSize) {
        return gamedao.getAIList(offset, pageSize);
    }

    @Override
    public int getCountForAI() {
        return gamedao.getCountForAI();
    }

    @Override
    public int geCountForAct() {
        return gamedao.geCountForAct();
    }



}
