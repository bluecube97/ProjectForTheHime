<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>메인화면</title>
<link rel="stylesheet" type="text/css" href="/resource/css/mainboard.css">
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
<script src="/resource/js/mainboard.js" defer></script>
</head>
<body>
	<header>
		<div class="logo">
			<button type="button" class="btn-logo" id="btn-logo">
				<img src="/resource/images/MATCHAI(Board).png" alt="로고" class="logo-img">
			</button>
		</div>
		<nav>
			<div class="main_menu">
				<ul>
					<li><button type="button" class="menu-btn" id="game-menu">경기</button></li>
					<li><button type="button" class="menu-btn" id="data-menu">데이터센터</button></li>
					<li><button type="button" class="menu-btn" id="aipick-menu">AI'sPick</button></li>
					<li><button type="button" class="menu-btn" id="board-menu">게시판</button></li>
					<li><button type="button" class="menu-btn" id="minigame-menu">미니게임</button></li>
				</ul>
			</div>
		</nav>
		<div class="auth-buttons">
			<!-- 우측 상단에 로그인, 회원가입 버튼, 로그인 시에만 로그아웃이 보임 -->
			<c:if test="${!empty sessionScope.userInfo}">
				<div class="ment">
					*${sessionScope.userInfo.usernickname}님*
					<!-- 로그인 했을 때 사용자의 닉네임이 뜸 -->
				</div>
				<button type="button" id="btn-logout">로그아웃</button>
			</c:if>
			<c:if test="${empty sessionScope.userInfo}">
				<button type="button" id="btn-login">로그인</button>
				<button type="button" id="btn-signup">회원가입</button>
			</c:if>
		</div>
	</header>
	<div class="content">
		<aside class="sidebar">
			<!-- 좌측 사이드 바에 최근 조회한 경기 목록이 뜨도록 -->
			<div class="recent-views">
				<h3>최근 조회 경기</h3>
				<ul>
					<li>경기 1</li>
					<li>경기 2</li>
					<li>경기 3</li>
					<li>경기 4</li>
					<li>경기 5</li>
				</ul>
			</div>
		</aside>
		<main>
			<div class="today-games">
				<!-- 오늘 진행하는 경기 표시 -->
				<h2>오늘의 경기</h2>
				<div class="leagues">
					<!-- 각 리그를 클릭 or 자동으로 움직이도록 -->
					<span>KBO</span> | <span>MLB(AL)</span> | <span>MLB(NL)</span>
				</div>
				<div class="games-container">
					<div class="games-row">
						<!-- 각 팀 클릭 시, 네이버 야구 전력 페이지 https://m.sports.naver.com/game/20240625HTLT02024/preview 여기로 -->
						<div class="game">
							<div class="team team-a">A팀</div>
							<div class="versus">VS</div>
							<div class="team team-b">B팀</div>
							<div class="summary">경기 요약 분석</div>
							<!-- 우리 페이지에서 경기 요약 분석한 결과 modal 방식으로 띄우기 -->
						</div>
						<div class="game">
							<div class="team team-a">A팀</div>
							<div class="versus">VS</div>
							<div class="team team-b">B팀</div>
							<div class="summary">경기 요약 분석</div>
						</div>
						<div class="game">
							<div class="team team-a">A팀</div>
							<div class="versus">VS</div>
							<div class="team team-b">B팀</div>
							<div class="summary">경기 요약 분석</div>
						</div>
					</div>
					<div class="games-row">
						<div class="game">
							<div class="team team-a">A팀</div>
							<div class="versus">VS</div>
							<div class="team team-b">B팀</div>
							<div class="summary">경기 요약 분석</div>
						</div>
						<div class="game">
							<div class="team team-a">A팀</div>
							<div class="versus">VS</div>
							<div class="team team-b">B팀</div>
							<div class="summary">경기 요약 분석</div>
						</div>
					</div>
				</div>
			</div>
		</main>
	</div>
	<section class="details-section">
		<!-- 각 게시판의 가장 최신 글 3개 미리 보기 -->
		<div class="details-box">
			<h3>경기 일정</h3>
			<ul>
				<li>세부 항목 1</li>
				<!-- 글을 누르면 해당 게시글로 이동 -->
				<li>세부 항목 2</li>
				<li>세부 항목 3</li>
			</ul>
			<div class="plus-icon">+</div>
			<!-- + 버튼을 누르면 게시판으로 이동 -->
		</div>
		<div class="details-box">
			<h3>경기 결과</h3>
			<ul>
				<li>세부 항목 1</li>
				<li>세부 항목 2</li>
				<li>세부 항목 3</li>
			</ul>
			<div class="plus-icon">+</div>
		</div>
		<div class="details-box">
			<h3>AI's Picks</h3>
			<ul>
				<li>세부 항목 1</li>
				<li>세부 항목 2</li>
				<li>세부 항목 3</li>
			</ul>
			<div class="plus-icon">+</div>
		</div>
		<div class="details-box">
			<h3>자유게시판</h3>
			<ul>
				<li>세부 항목 1</li>
				<li>세부 항목 2</li>
				<li>세부 항목 3</li>
			</ul>
			<div class="plus-icon">+</div>
		</div>
	</section>
	<footer class="footer">
		<!-- footer -->
		<div class="footer-items">
			<div>
				<div>
					<a href="https://www.naver.com">네이버</a>
				</div>
				<div>
					<a href="https://www.naver.com">네이버</a>
				</div>
				<div>
					<a href="https://www.naver.com">네이버</a>
				</div>
				<div>
					<a href="https://www.naver.com">네이버</a>
				</div>
			</div>
			<div>
				<div>
					<a href="https://www.youtube.com">youtube</a>
				</div>
				<div>
					<a href="https://www.youtube.com">youtube</a>
				</div>
				<div>
					<a href="https://www.youtube.com">youtube</a>
				</div>
				<div>
					<a href="https://www.youtube.com">youtube</a>
				</div>
			</div>
			<div>
				<div>
					<a href="https://www.google.com">google.com</a>
				</div>
				<div>
					<a href="https://www.google.com">google.com</a>
				</div>
				<div>
					<a href="https://www.google.com">google.com</a>
				</div>
				<div>
					<a href="https://www.google.com">google.com</a>
				</div>
			</div>
		</div>
	</footer>
</body>
</html>
