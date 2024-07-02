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

}
