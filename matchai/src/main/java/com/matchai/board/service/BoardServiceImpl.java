package com.matchai.board.service;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.matchai.board.dao.BoardDao;
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
		//System.out.println("서비스임플"+params);
		return boardDao.getGameData(params);
	}

	@Override
	public HashMap<String, Object> aiData(String matchcode) {
		return boardDao.aiData(matchcode);
	}

	@Override
	public List<HashMap<String, Object>> getCurResults(String selLeague, String selYear, String selMonth) {
		HashMap<String, Object> params = new HashMap<>();
		System.out.println("서비스 임플 :" + selLeague + selYear + selMonth);
		String leagueTbl = "tbl_kborslt_nt03";
		
		if(selLeague.equals("mlb")) {
		    leagueTbl = "tbl_mlbrslt_nt03";
		} else if(selLeague.equals("kbo")) {
		    leagueTbl = "tbl_kborslt_nt03";
		} else if(selLeague == null || selLeague.equals("")) {
			leagueTbl = "tbl_kborslt_nt03";
		}
		
		// 쿼리문에 들어갈 조건 검색을 위해, 년도와 월을 합침
		String selym = selYear + "-" + selMonth + "-%";
		
		params.put("selym", selym);
		params.put("leagueTbl", leagueTbl);
		
        System.out.println("Executing query with parameters:");
        System.out.println("leagueTbl: " + leagueTbl);
        System.out.println("selYm: " + selym);
		
		System.out.println("서비스 임플 해시맵 : " + params);
		
		List<HashMap<String, Object>> result = boardDao.getCurResults(params);
		System.out.println("서비스 임플 결과"+result);
		
		return result;
	}
}
