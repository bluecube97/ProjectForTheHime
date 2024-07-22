package com.matchai.board.service;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.matchai.board.dao.BoardDao;

import javax.servlet.http.HttpSession;

@Service
public class BoardServiceImpl implements BoardService {
	@Autowired
	private BoardDao boardDao;
	
	// kbo 당일 경기 수
	@Override
	public int kboCnt() {
		return boardDao.kboCnt();
	}
	
	// mlb 당일 경기 수
	@Override
	public int mlbCnt() {
		return boardDao.mlbCnt();
	}
	
	// kbo 경기 목록
	@Override
	public List<HashMap<String, Object>> kboMatchList() {
		return boardDao.kboMatchList();
	}
	
	// mlb 경기 목록
	@Override
	public List<HashMap<String, Object>> mlbMatchList() {
		return boardDao.mlbMatchList();
	}

	@Override
	public HashMap<String, Object> fetchGameData(String teamCode1, String teamCode2) {
		HashMap<String, Object> params = new HashMap<>();
		params.put("teamCode1", teamCode1);
		params.put("teamCode2", teamCode2);
		System.out.println("서비스임플"+params);
		return boardDao.getGameData(params);
	}

	@Override
	public HashMap<String, Object> aiData(String matchcode) {

		return boardDao.aiData(matchcode);
	}

	@Override
	public int searchBoard(String matchcode) {

		return boardDao.searchBoard(matchcode);
	}

	@Override
	public void insertAiData(HashMap<String, Object> aiData) {

		boardDao.insertAiData(aiData);
	}

	@Override
	public void insertComment(HashMap<String,Object>map ) {
			boardDao.insertComment(map);


	}

	@Override
	public int getBoardNumber(String matchcode) {

		return boardDao.getBoardNumber(matchcode);
	}

	@Override
	public List<HashMap<String, Object>> getCommentList(String brdno) {
		return boardDao.getCommentList(brdno);
	}


}
