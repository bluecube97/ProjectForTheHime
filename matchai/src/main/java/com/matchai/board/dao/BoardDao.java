package com.matchai.board.dao;

import java.util.HashMap;
import java.util.List;

public interface BoardDao {
	// kbo 당일 경기 수
	int kboCnt();

	// mlb 당일 경기 수
	int mlbCnt();

	// kbo 경기목록
	List<HashMap<String, Object>> kboMatchList();

	// mlb 경기목록
	List<HashMap<String, Object>> mlbMatchList();

	// 
	HashMap<String, Object> getGameData(HashMap<String, Object> params);

	HashMap<String, Object> aiData(String matchcode);

	//예전 경기 기록 Fetch통신으로
	List<HashMap<String, Object>> getCurResults(HashMap<String, Object> params);
  
	int searchBoard(String matchcode);

	void insertAiData(HashMap<String, Object> aiData);

	void insertComment(HashMap<String, Object> map);

	int getBoardNumber(String matchcode);

	List<HashMap<String, Object>> getCommentList(String brdno);

}
