package com.matchai.board.mapper;

import org.apache.ibatis.annotations.Mapper;

import java.util.HashMap;
import java.util.List;

@Mapper
public interface BoardMapper {
    // kbo 당일 경기 수
    int kboCnt();

    // mlb 당일 경기 수
    int mlbCnt();

    // kbo 경기목록
    List<HashMap<String, Object>> kboMatchList();

    // mlb 경기목록
    List<HashMap<String, Object>> mlbMatchList();

    HashMap<String, Object> getGameData(HashMap<String, Object> params);


    // 예전 경기 기록
    List<HashMap<String, Object>> getCurResults(HashMap<String, Object> params);

    int searchBoard(String matchcode);

    int getBoardNumber(String matchcode);

    List<HashMap<String, Object>> getCommentList(int brdno);

    void insertComment(HashMap<String, Object> map);


    HashMap<String, Object> aiData(String matchcode);

    void insertAiData(HashMap<String, Object> aiData);


    HashMap<String, Object> actData(String matchcode);

    void insertactData(HashMap<String, Object> actData);
}
