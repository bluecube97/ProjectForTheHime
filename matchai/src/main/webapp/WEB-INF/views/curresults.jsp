<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>MATCHAI</title>
<link rel="stylesheet" type="text/css" href="/resource/css/test.css">
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
<script src="https://cdn.jsdelivr.net/npm/chart.js@3.7.0/dist/chart.min.js" defer></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
<script src="/resource/js/curresults.js" defer></script>
</head>
<body>
	<header>
		<div class="logo">
			<button type="button" class="btn-logo" id="btn-logo">
			    <img src="/resource/images/mainlogo/MATCHAIBoard.png" alt="로고" class="logo-img">
			</button>
		</div>
		<nav>
			<div class="main_menu">
				<ul>
					<li><button type="button" class="menu-btn" id="game-menu">경기</button></li>
					<li><button type="button" class="menu-btn" id="data-menu">데이터센터</button></li>
					<li><button type="button" class="menu-btn" id="aipick-menu">AI'sPick</button></li>
					<li><button type="button" class="menu-btn" id="board-menu">게시판</button></li>
					<c:choose>
						<c:when test="${empty sessionScope.userInfo}">
							<button type="button" class="menu-btn" id="minigame-menu" onclick="loginPlz()">미니게임</button>
						</c:when>
						<c:otherwise>
							<button type="button" class="menu-btn" id="minigame-menu" onclick="startUnity()">미니게임</button>
						</c:otherwise>
					</c:choose>				</ul>
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
	    <label for="league">리그 선택:</label>
	    <button id="mlb-button" value="mlb" onclick="selectLeague('mlb')">MLB</button>
	    <button id="kbo-button" value="kbo" onclick="selectLeague('kbo')">KBO</button>
	
	    <label for="year">년도 선택:</label>
	    <select id="year" name="year" onchange="updateCalendar()">
	    </select>
	
	    <div class="month-selector">
	        <button onclick="selectMonth(1)">1월</button>
	        <button onclick="selectMonth(2)">2월</button>
	        <button onclick="selectMonth(3)">3월</button>
	        <button onclick="selectMonth(4)">4월</button>
	        <button onclick="selectMonth(5)">5월</button>
	        <button onclick="selectMonth(6)">6월</button>
	        <button onclick="selectMonth(7)">7월</button>
	        <button onclick="selectMonth(8)">8월</button>
	        <button onclick="selectMonth(9)">9월</button>
	        <button onclick="selectMonth(10)">10월</button>
	        <button onclick="selectMonth(11)">11월</button>
	        <button onclick="selectMonth(12)">12월</button>
	    </div>
	
	    <div class="box_type_board">
	        <div class="item_box cw100">
	            <div class="inner">
	                <div class="sh_box">
	                    <div class="box_head" id="month-header">7월</div>
	                    <div class="box_cont">
	                        <div class="calendar_area">
	                            <table id="calendar" border="1">
	                                <thead>
	                                    <tr>
	                                        <th>월</th>
	                                        <th>화</th>
	                                        <th>수</th>
	                                        <th>목</th>
	                                        <th>금</th>
	                                        <th>토</th>
	                                        <th>일</th>
	                                    </tr>
	                                </thead>
	                                <tbody id="calendar-body">
	                                </tbody>
	                            </table>
	                        </div>
	                    </div>
	                </div>
	            </div>
	        </div>
	    </div>
	</main>
</body>
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
</html>
