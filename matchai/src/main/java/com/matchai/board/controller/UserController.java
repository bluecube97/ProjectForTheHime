package com.matchai.board.controller;

import java.security.Principal;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import com.matchai.board.config.JwtUtil;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.oauth2.client.authentication.OAuth2AuthenticationToken;
import org.springframework.security.oauth2.core.user.OAuth2User;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.servlet.ModelAndView;

import com.matchai.board.service.UserService;

@RequestMapping("/user/*")
@Controller
public class UserController {

	@Autowired
	UserService usersvc;
  
	// 구글 로그인 성공 시 호출되는 메서드(사용 안하는거 같음)
	@Autowired
	UserDetailsService userDetailsService;

	@Autowired
	JwtUtil jwtUtil;
  
	@GetMapping("/loginSuccess")
	public String loginSuccess(Principal principal, HttpSession session) {

		// Principal 객체가 OAuth2AuthenticationToken의 인스턴스인지 확인
		if (principal instanceof OAuth2AuthenticationToken) {

			// Principal 객체를 OAuth2AuthenticationToken으로 캐스팅
			OAuth2AuthenticationToken oAuth2Token = (OAuth2AuthenticationToken) principal;

			// OAuth2User 객체를 통해 구글 사용자 정보를 가져옴
			OAuth2User user = oAuth2Token.getPrincipal();

			// 사용자 정보 담을 map 생성
			Map<String, Object> userInfo = new HashMap<>();

			// 구글 사용자 정보에서 가져온 정보를 Map에 추가
			userInfo.put("name", user.getAttribute("name"));
			userInfo.put("email", user.getAttribute("email"));
			userInfo.put("nickname", user.getAttribute("name")); // 예시로 추가한 항목
			userInfo.put("age", user.getAttribute("age")); // 예시로 추가한 항목

			// 세션에 사용자 정보를 저장
			session.setAttribute("userInfo", userInfo);

			// 구글 사용자 정보를 데이터베이스에 저장
			usersvc.saveGoogleUser(userInfo);
		}
		// 메인 화면으로 리다이렉트
		return "redirect:/board/main";
	}

	// 구글 로그인 실패(사용 안하는거 같음)
	@GetMapping("/loginFailure")
	public ModelAndView loginFailure(ModelAndView mv) {

		mv.setViewName("login");

		mv.addObject("error", "로그인에 실패하였습니다.");

		return mv;
	}

	// 로그인 창 띄우기
	@GetMapping("/login")
	public ModelAndView userLogin(ModelAndView mv, HttpServletRequest req) {

		mv.setViewName("login");

		return mv;
	}

	// 로그인 창에서 사용자 정보 확인
	@PostMapping("/login")
	@ResponseBody
	public Map<String, Object> LoginCheck(HttpServletRequest req, @RequestBody HashMap<String, Object> param) {

		// 세션 객체 가져오기
		HttpSession session = req.getSession();

		// 사용자가 입력한 아이디, 비밀번호 가져옴
		String smail = (String) param.get("smail");
		String spass = (String) param.get("spass");

		System.out.println("유저 이메일 : " + smail);
		System.out.println("유저 비번 : " + spass);


		// 아이디와 비밀번호로 로그인 체크 메서드를 거쳐 사용자 정보를 map에 가져옴
		HashMap<String, Object> map = usersvc.LoginCheck(param);

		// 아이디 존재 여부 확인
		int cnt = usersvc.UserCheck(param);

		// 멘트 저장할 응답 Map 생성
		Map<String, Object> response = new HashMap<>();

		// 사용자가 로그인 창에 아이디 비밀번호를 모두 입력하지 않았을 경우,
		if (smail == null || smail.equals("") || spass == null || spass.equals("")) {
			response.put("ment", "ID, PASSWORD를 확인해주세요.");
			return response;
		}
		// 존재하는 아이디가 없을 경우,
		if (cnt == 0) {
			response.put("ment", "존재하지 않는 계정입니다.");
		} else {
			// 그 외 가져온 사용자 정보가 들어있는 map이 null일 경우,
			if (map == null) {
				response.put("ment", "아이디 또는 비밀번호가 일치하지 않습니다.");
			} else {
				// 존재하는 아이디가 있거나, 사용자 정보 map이 null이 아닐 경우,
				session.setAttribute("userInfo", map);
				System.out.println("세션 정보: " + session.getAttribute("userInfo"));

				session = jwtUtil.setToken(session, map);

				System.out.println("token(usercon): " + session.getAttribute("token"));
				response.put("ment", "로그인 성공");
				response.put("redirect", "/board/main"); // 로그인 성공 시 리다이렉트할 URL
			}
		}
		return response;
	}

	// 로그아웃
	@GetMapping("/logout")
	public String Logout(HttpServletRequest req) {

		// 세션이 없으면 null 반환
		HttpSession session = req.getSession(false);

		// 세션이 존재하면 세션을 무효화시켜 로그아웃 처리
		if (session != null) {
			session.invalidate();
		}

		// 보드 메인으로 리다이렉트
		return ("redirect:/board/main");
	}

	// 회원가입 창 띄우기
	@GetMapping("/signup")
	public ModelAndView signUp(HttpServletRequest req, ModelAndView mv) {

		// 세션이 없으면 null 반환
		HttpSession session = req.getSession(false);

		// 세션이 존재하고 userInfo의 값이 있으면 메인 화면으로 리다이렉트
		if (session != null && session.getAttribute("userInfo") != null) {
			mv.setViewName("redirect:/board/main");
			// 그 외에는 회원가입 창으로 이동
		} else {
			mv.setViewName("signUp");
		}
		return mv;
	}

	// 회원가입 정보 저장
	@PostMapping("/signup")
	@ResponseBody
	public Map<String, Object> signUp(@RequestBody HashMap<String, Object> param) {

		// 사용자가 입력한 회원가입 정보를 모두 가져옴
		String smail = (String) param.get("smail");
		String spass = (String) param.get("spass");
		String sname = (String) param.get("sname");
		String snick = (String) param.get("snick");
		String sage = (String) param.get("sage");
		String league = (String) param.get("league");
		// 사용자가 선택한 선호하는 팀 목록 받기
		@SuppressWarnings("unchecked")
		List<HashMap<String, Object>> teams = (List<HashMap<String, Object>>) param.get("teams");

		// 회원가입 정보를 담을 map 생성, 넣기
		HashMap<String, Object> map = new HashMap<>();
		map.put("smail", smail);
		map.put("spass", spass);
		map.put("sname", sname);
		map.put("snick", snick);
		map.put("sage", sage);
		map.put("league", league);

		// 회원가입 정보를 저장
		int result = usersvc.signUp(map);

		// 회원가입 성공 시 팀 정보 저장
		if (result > 0) {
			for (HashMap<String, Object> team : teams) {
				// 아이디 추가해서 해당 사용자의 선호하는 팀 저장
				team.put("smail", smail);
				// 선호하는 팀 저장
				usersvc.saveUserTeam(team);
			}
		}

		// 리턴할 멘트 map 생성
		Map<String, Object> r_map = new HashMap<>();
		if (result > 0) {
			r_map.put("connection", "O");
			r_map.put("redirect", "/login");
		} else {
			r_map.put("ment", "회원가입에 실패하였습니다.");
			r_map.put("connection", "X");
		}
		return r_map;
	}

	// 아이디 중복 체크
	@PostMapping("/emailcheck")
	@ResponseBody
	public Map<String, Object> emailCheck(@RequestBody HashMap<String, Object> param) {

		// 회원가입 창에 사용자가 입력한 메일을 가져옴
		String smail = (String) param.get("smail");

		// 입력한 메일에 대한 중복 여부 확인
		int smailCheck = usersvc.emailCheck(smail);

		// 응답할 멘트 map 생성
		Map<String, Object> response = new HashMap<>();

		// 중복된 메일이 있을 경우,
		if (smailCheck > 0) {
			response.put("ment", "이미 사용 중인 이메일입니다.");
			response.put("connection", "X");
			// 중복된 메일이 없을 경우,
		} else {
			response.put("connection", "O");
		}
		return response;
	}

	// 리그에 대한 팀 목록 가져오기
	@PostMapping("/getteams")
	@ResponseBody
	public Map<String, Object> getTeams(@RequestBody Map<String, Object> param) {

		// 회원가입 창에 사용자가 선택한 리그를 가져오기
		String league = (String) param.get("league");

		// 선택한 리그에 대한 팀 목록 가져오기
		List<Map<String, Object>> teams = usersvc.getTeams(league);

		// 리턴할 map 생성
		Map<String, Object> response = new HashMap<>();

		// map에다 선택한 리그에 대한 팀 목록 넣기
		response.put("teams", teams);

		return response;
	}
	
}
