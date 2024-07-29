package com.matchai.board.service;

import java.util.HashMap;
import java.util.List;

public interface BaseballService {

    List<HashMap<String, Object>> getGameList(int offset, int pageSize);

    List<HashMap<String, Object>> getAIList(int offset, int pageSize);

    int getCountForAI();

    int geCountForAct();

}
