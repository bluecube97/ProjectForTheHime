package com.matchai.board.mapper;

import java.util.HashMap;
import java.util.List;

public interface BaseballMapper {

    List<HashMap<String, Object>> getGameList(int offset, int pageSize);

    List<HashMap<String, Object>> getAIList(int offset, int pageSize);


    int geCountForAct();

    int getCountForAI();
}
