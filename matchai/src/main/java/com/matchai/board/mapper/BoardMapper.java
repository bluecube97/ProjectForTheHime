package com.matchai.board.mapper;

import java.util.HashMap;
import java.util.List;

import org.apache.ibatis.annotations.Mapper;

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

	HashMap<String, Object> aiData(String matchcode);


	// 예전 경기 기록
	List<HashMap<String, Object>> getCurResults(HashMap<String, Object> params);

	int searchBoard(String matchcode);

	int getBoardNumber(String matchcode);

	void insertAiData(HashMap<String, Object> aiData);

    void insertComment(HashMap<String, Object> map);

	List<HashMap<String, Object>> getCommentList(String brdno);

}
