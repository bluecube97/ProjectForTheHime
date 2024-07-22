package com.matchai.board.dao;

import java.util.HashMap;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import com.matchai.board.mapper.BoardMapper;

@Repository
public class BoardDaoImpl implements BoardDao {
	
	@Autowired
	private BoardMapper boardMapper;
	
	// kbo 당일 경기 수
	@Override
	public int kboCnt() {
		return boardMapper.kboCnt();
	}
	
	// mlb 당일 경기 수
	@Override
	public int mlbCnt() {
		return boardMapper.mlbCnt();
	}
	
	// kbo 경기 목록
	@Override
	public List<HashMap<String, Object>> kboMatchList() {
		return boardMapper.kboMatchList();
	}
	
	// mlb 경기 목록
	@Override
	public List<HashMap<String, Object>> mlbMatchList() {
		return boardMapper.mlbMatchList();
	}

    @Override
    public HashMap<String, Object> getGameData(HashMap<String, Object> params) {
    	System.out.println("dao Params : " + params);
    	HashMap<String, Object> result = boardMapper.getGameData(params);
        System.out.println("select 결과: " + result);
        return result;
    }

	@Override
	public HashMap<String, Object> aiData(String matchcode) {

		return boardMapper.aiData(matchcode);
	}

	@Override
	public List<HashMap<String, Object>> getCurResults(HashMap<String, Object> params) {
		return boardMapper.getCurResults(params);
	}

	public int searchBoard(String matchcode) {

		return boardMapper.searchBoard(matchcode);
	}

	@Override
	public void insertAiData(HashMap<String, Object> aiData) {

		boardMapper.insertAiData(aiData);
	}

	@Override
	public void insertComment(HashMap<String, Object> map) {
		boardMapper.insertComment(map);

	}

	@Override
	public int getBoardNumber(String matchcode) {
		return boardMapper.getBoardNumber(matchcode);

	}

	@Override
	public List<HashMap<String, Object>> getCommentList(String brdno) {
		return boardMapper.getCommentList(brdno);
	}

}
