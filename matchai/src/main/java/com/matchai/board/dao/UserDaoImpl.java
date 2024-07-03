package com.matchai.board.dao;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import com.matchai.board.mapper.UserMapper;

@Repository
public class UserDaoImpl implements UserDao {

	@Autowired
	private UserMapper userMapper;

	// 로그인 시 사용자 정보 가져오기
	@Override
	public HashMap<String, Object> LoginCheck(HashMap<String, Object> param) {
		return userMapper.LoginCheck(param);
	}

	// 아이디 존재 유무 확인
	@Override
	public int UserCheck(HashMap<String, Object> param) {
		return userMapper.UserCheck(param);
	}

	// 아이디 중복 확인
	@Override
	public int emailCheck(String param) {
		return userMapper.emailCheck(param);
	}

	// 회원가입 정보 저장
	@Override
	public int signUp(HashMap<String, Object> param) {
		return userMapper.signUp(param);
	}

	// 팀정보 가져오기
	@Override
	public List<Map<String, Object>> getTeams(String param) {
		return userMapper.getTeams(param);
	}

	// 사용자별 선호하는 팀 저장
	@Override
	public int saveUserTeam(Map<String, Object> param) {
		return userMapper.saveUserTeam(param);
	}
	
	// 구글 아이디 존재 유무 확인
	@Override
	public int GoogleUserCheck(HashMap<String, Object> param) {
		return userMapper.GoogleUserCheck(param);
	}
	
	// 구글 사용자 정보 저장
	@Override
	public void saveGoogleUser(Map<String, Object> param) {
		userMapper.saveGoogleUser(param);
	}
}
