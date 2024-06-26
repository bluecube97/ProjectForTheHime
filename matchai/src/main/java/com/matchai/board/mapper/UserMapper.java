package com.matchai.board.mapper;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.apache.ibatis.annotations.Mapper;

@Mapper
public interface UserMapper {
	// 로그인 시 사용자 정보 가져오기
	HashMap<String, Object> LoginCheck(HashMap<String, Object> param);

	// 아이디 존재 유무 확인
	int UserCheck(HashMap<String, Object> param);

	// 아이디 중복 확인
	int emailCheck(String param);
	
	// 회원가입 정보 저장
	int signUp(HashMap<String, Object> param);

	// 팀정보 가져오기
	List<Map<String, Object>> getTeams(String param);

	// 사용자별 선호하는 팀 저장
	int saveUserTeam(Map<String, Object> param);

	// 구글 사용자 정보 저장
	void saveGoogleUser(Map<String, Object> param);
}
