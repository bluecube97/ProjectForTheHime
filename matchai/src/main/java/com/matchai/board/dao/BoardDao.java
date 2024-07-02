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

}
