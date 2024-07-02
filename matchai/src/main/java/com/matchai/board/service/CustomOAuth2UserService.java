package com.matchai.board.service;

import java.util.HashMap;
import java.util.Map;

import javax.servlet.http.HttpSession;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.oauth2.client.userinfo.DefaultOAuth2UserService;
import org.springframework.security.oauth2.client.userinfo.OAuth2UserRequest;
import org.springframework.security.oauth2.core.OAuth2AuthenticationException;
import org.springframework.security.oauth2.core.user.OAuth2User;
import org.springframework.stereotype.Service;

@Service
public class CustomOAuth2UserService extends DefaultOAuth2UserService {

	@Autowired
	private HttpSession session;

	@Autowired
	private UserService userService;

	// OAuth2 사용자 정보를 로드하고 처리하는 메서드
	@Override
	public OAuth2User loadUser(OAuth2UserRequest userRequest) throws OAuth2AuthenticationException {
		
		// 부모 클래스의 loadUser 메서드를 호출하여 사용자 정보를 가져옴
		OAuth2User oAuth2User = super.loadUser(userRequest);

		// 가져온 사용자 정보에서 속성들을 추출
		Map<String, Object> attributes = oAuth2User.getAttributes();

		// 사용자 정보에서 이메일, 이름 등을 가져옴
		String useremail = (String) attributes.get("email");
		String usernm = (String) attributes.get("name");
		// 닉네임, 나이는 구글 사용자 정보에 없기 때문에 기본 값으로 설정
		String usernickname = usernm; // 사용자 이름으로 기본 값 설정
		String userage = "20"; // 기본 값 설정

		// 사용자 정보를 저장할 Map 생성
		Map<String, Object> userInfo = new HashMap<>();
		userInfo.put("useremail", useremail);
		userInfo.put("usernm", usernm);
		userInfo.put("usernickname", usernickname);
		userInfo.put("userage", userage);

		// 사용자 정보를 디버깅 로그로 출력
		System.out.println(userInfo.get("useremail"));
		System.out.println(userInfo.get("usernm"));
		System.out.println(userInfo.get("usernickname"));
		System.out.println(userInfo.get("userage"));
		System.out.println("Google 로그인 사용자 정보: " + userInfo);

		// 사용자 정보가 DB에 존재하는지 확인
		HashMap<String, Object> param = new HashMap<>();
		param.put("smail", useremail);
		int userCount = userService.GoogleUserCheck(param);

		// 사용자 정보가 데이터베이스에 존재하지 않는 경우에만 저장
		if (userCount == 0) {
			userService.saveGoogleUser(userInfo);
		}

		// 세션에 사용자 정보 저장
		session.setAttribute("userInfo", userInfo);

		// OAuth2User 객체를 반환
		return oAuth2User;
	}
}
