<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>MATCHAI</title>
<link rel="stylesheet" type="text/css" href="/resource/css/mainboard.css">
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
<script src="https://cdn.jsdelivr.net/npm/chart.js@3.7.0/dist/chart.min.js" defer></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
<script src="/resource/js/mainboard.js" defer></script>
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
					<li><button type="button" class="menu-btn" id="board-menu" onclick="openFreeBoard()">자유 게시판</button></li>
					<c:choose>
						<c:when test="${empty sessionScope.userInfo}">
							<button type="button" class="menu-btn" id="minigame-menu" onclick="loginPlz()">미니게임</button>
						</c:when>
						<c:otherwise>
							<button type="button" class="menu-btn" id="minigame-menu" onclick="startUnity()">미니게임</button>
						</c:otherwise>
					</c:choose>
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
				<h2>오늘의 경기</h2>
				<div class="leagues">
					<span id="kbo-tab">KBO</span> | <span id="mlb-tab">MLB</span>
				</div>
				<div class="games-container" id="games-container">
					<div id="kbo-content">
						<c:choose>
							<c:when test="${not empty klist}">
								<div class="games-row">
									<c:forEach var="game" items="${klist}">
										<div class="game">
											<div class="teams">
												<div class="team team-a">
														${game.team1}<img src="/resource/images/teamlogo/KBO/${game.code1}.png" alt="" class="team-logo">
												</div>
												<div class="versus">VS</div>
												<div class="team team-b">
													<img src="/resource/images/teamlogo/KBO/${game.code2}.png" alt="" class="team-logo">${game.team2}
												</div>
											</div>
											<form action="/board/gamedetail" method="get">
												<input type="hidden" name="team1" value="${game.code1}">
												<input type="hidden" name="team2" value="${game.code2}">
												<input type="hidden" name="matchcode" value="${game.matchcode}">
												<button type="submit" class="summary">경기 분석</button>
											</form>
										</div>
									</c:forEach>
								</div>
							</c:when>
							<c:otherwise>
								<div class="no-games">${kment}</div>
							</c:otherwise>
						</c:choose>
					</div>
					<div id="mlb-content" style="display: none;">
						<div class="mlbment">MLB 경기는 다음 날 기준입니다.</div>
						<c:choose>
							<c:when test="${not empty mlist}">
								<div class="games-row">
									<c:forEach var="game" items="${mlist}">
										<div class="game">
											<div class="teams">
												<div class="team team-a">
														${game.team1}<img src="/resource/images/teamlogo/MLB/${game.code1}.png" alt="" class="team-logo">
												</div>
												<div class="versus">VS</div>
												<div class="team team-b">
													<img src="/resource/images/teamlogo/MLB/${game.code2}.png" alt="" class="team-logo">${game.team2}
												</div>
											</div>
											<form action="/board/gamedetail" method="get">
												<input type="hidden" name="team1" value="${game.code1}">
												<input type="hidden" name="team2" value="${game.code2}">
												<input type="hidden" name="matchcode" value="${game.matchcode}">
												<button type="submit" class="summary">경기 분석</button>
											</form>
										</div>
									</c:forEach>
								</div>
							</c:when>
							<c:otherwise>
								<div class="no-games">${mment}</div>
							</c:otherwise>
						</c:choose>
					</div>
				</div>
			</div>
		</main>
	</div>
	<section class="details-section">
		<!-- 각 게시판의 가장 최신 글 3개 미리 보기 -->
		<div class="details-box large">
			<h3>AI's Picks</h3>
			<ul>
				<!-- 글을 누르면 해당 게시글로 이동 -->
				<li>AI의 경기 결과 예측 내용, 칸이 넘어가면 ...</li>
				<li>AI가 예측한 결과 성공 여부에 대한 게시판, 칸이 넘어가면 ...</li>
				<li>날짜 별로 계산해서 매일 자동으로 작성, 칸이 넘어가면 ...</li>
				<li>KBO와 MLB 경기에 대한 승률 보여주기, 칸이 넘어가면 ...</li>
				<li>경기 예측에 대한 성공 유무 보여주기, 칸이 넘어가면 ...</li>
				<li>AI의 경기 결과 예측 내용, 칸이 넘어가면 ...</li>
				<li>AI의 경기 결과 예측 내용, 칸이 넘어가면 ...</li>
				<li>AI의 경기 결과 예측 내용, 칸이 넘어가면 ...</li>
				<li>AI의 경기 결과 예측 내용, 칸이 넘어가면 ...</li>
			</ul>
			<div class="plus-icon">+</div>
		</div>
		<div class="details-box small">
			<h3>공지 사항</h3>
			<ul>
				<!-- 글을 누르면 해당 게시글로 이동 -->
				<li>관리자가 게시한 공시사항 게시판, 칸이 넘어가면 ...</li>
				<li>공지 한 글이 최신순으로 정렬, 칸이 넘어가면 ...</li>
				<li>공시사항 게시판 내용, 칸이 넘어가면 ...</li>
			</ul>
			<div class="plus-icon">+</div>
			<!-- + 버튼을 누르면 게시판으로 이동 -->
		</div>
		<div class="details-box small">
			<h3>자유게시판</h3>
			<ul>
				<li>사용자들이 작성한 자유게시판 내용, 칸이 넘어가면 ...</li>
				<li>자유게시판에 들어가면 최근 공지 3~5개 항목이 최상단 고정, 칸이 넘어가면 ...</li>
				<li>자유게시판 내용, 칸이 넘어가면 ...</li>
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
	<!-- 모달 추가 -->
	<div class="modal fade" id="summaryModal" tabindex="-1"
		aria-labelledby="summaryModalLabel" aria-hidden="true">
		<div class="modal-dialog modal-lg">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="summaryModalLabel">경기 요약 분석</h5>
					<button type="button" class="btn-close" data-bs-dismiss="modal"
						aria-label="Close"></button>
				</div>
				<div class="modal-body">
					<div class="modal-body_row">
						<div class="col-md-6">
							<canvas id="summaryChart" width="300" height="350"></canvas>
						</div>
						<div class="col-md-6">
							<img src="/resource/images/modal/modaltop.jpg" alt="모달" class="modal_top">
							<div id="summaryText"></div>
							<div id="teamScores">
                            	<div id="team1Score"></div>
                            	<div id="team2Score"></div>
								<div id="exepect-warning">※저는 이때까지의 경기 결과로 분석을 해요! 제 추측은 빗나갈 수도 있어요!※</div>
                        	</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<!-- 모달 끝 -->
</body>
</html>
