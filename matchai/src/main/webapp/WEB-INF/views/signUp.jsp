<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<%@ taglib prefix="fn" uri="http://java.sun.com/jsp/jstl/functions"%>
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
<!DOCTYPE html>
<c:set var="root" value="${pageContext.request.contextPath}" />
<html>
<head>
<meta charset="UTF-8">
<title>회원가입</title>
<link rel="stylesheet" type="text/css" href="${root}/resource/css/signup.css">
<script type="text/javascript" src="${root}/resource/js/signup.js?version=1" defer></script>
</head>
<body>
	<div class="realMain">
		<div class="mainForm">
			<form id="signForm" action="" method="post" class="signup-form">
				<div class="logo">
					<button type="button" class="btn-logo" id="btn-logo">
						<img src="/resource/images/MATCHAI.png" alt="로고" class="logo-img">
					</button>
				</div>
				<div class="form-param email-param">
					<label for="smail">이메일</label>
					<div class="input-group">
						<input type="email" id="smail" class="smail" name="smail" placeholder="name@example.com" />
						<button type="button" class="input-group-btn" onclick="mailCheck()">아이디 확인</button>
					</div>
				</div>
				<div class="form-param">
					<label for="spass">비밀번호</label>
					<input type="password" id="spass" class="spass" name="spass" placeholder="특수문자, 숫자 포함한 8자리 이상 비밀번호 사용 가능" onblur="passCheck()" />
				</div>
				<div class="form-param">
					<label for="srepass">비밀번호 확인</label>
					<input type="password" id="srepass" class="srepass" name="srepass" placeholder="비밀번호를 다시 입력하세요."  onblur="repassCheck()" />
				</div>
				<div class="form-param">
					<label for="sname">이름</label>
					<input type="text" id="sname" class="sname" name="sname" placeholder="이름"  onblur="nameCheck()" />
				</div>
				<div class="form-param">
					<label for="snick">닉네임</label>
					<input type="text" id="snick" class="snick" name="snick" placeholder="닉네임"  onblur="nickCheck()" />
				</div>
				<div class="form-param">
					<label for="sage">나이</label>
					<select id="sage" class="sage" name="sage">
						<option value="10">10대</option>
						<option value="20">20대</option>
						<option value="30">30대</option>
						<option value="40">40대</option>
						<option value="50">50대</option>
						<option value="60">60대</option>
					</select>
				</div>
				<div class="leaguemsg">
					<label for="listleagues">&lt;선호하는팀&gt;</label>
				</div>
				<div class="leagueForm">
					<div class="form-index">
						<label for="league">리그 목록</label>
						<select id="league" class="league" name="league" onchange="getTeams(this.value)">
							<option value="KB">KBO</option>
							<option value="AL">AL</option>
							<option value="NL">NL</option>
						</select>
					</div>
					<div class="dual-list-container">
						<div>
							<label for="listleagues">팀 목록</label>
							<select id="listleagues" name="listleagues" multiple size="20"></select>
						</div>
						<div class="dual-list-buttons">
							<button type="button" name="add" id="add" onClick="addteam()">추가</button>
							<button type="button" name="remove" id="remove"
								onClick="delteam()">삭제</button>
						</div>
						<div>
							<label for="chsleagues">선택한 팀 목록</label>
							<select id="chsleagues" name="chsleagues" multiple size="20"></select>
						</div>
					</div>
				</div>
				<div class="valbtn">
					<button type="button" onclick="signUp()">가입</button>
				</div>
				<div id="message"></div>
			</form>
		</div>
	</div>
</body>
</html>