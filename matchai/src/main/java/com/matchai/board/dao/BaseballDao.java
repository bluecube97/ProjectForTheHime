package com.matchai.board.dao;

import java.util.HashMap;
import java.util.List;

public interface BaseballDao {


    List<HashMap<String, Object>> getGameList(int offset, int pageSize);

    List<HashMap<String, Object>> getAIList(int offset, int pageSize);

    int geCountForAct();

    int getCountForAI();
}
