<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css" integrity="sha512-z3gLpd7yknf1YoNbCzqRKc4qyor8gaKU1qmn+CShxbuBusANI9QpRohGBreCFkKxLhei6S9CQXFEbbKuqLg0DA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
<script src="/resource/js/login.js?version=1.0" defer></script>
<title>로그인</title>
<link rel="stylesheet" type="text/css" href="/resource/css/login.css">
</head>
<body>
	<div class="container">
		<div class="logo">
			<button type="button" class="btn-logo" id="btn-logo">
				<img src="/resource/images/MATCHAI.png" alt="로고" class="logo-img">
			</button>
		</div>
		<form id="loginForm">
			<div class="form-group">
				<label for="smail">ID</label> <input type="text" class="form-control" id="smail" placeholder="아이디를 입력하세요.">
			</div>
			<div class="form-group">
				<label for="spass">Password</label> <input type="password" class="form-control" id="spass" placeholder="비밀번호를 입력하세요.">
			</div>
			<button type="button" class="btn btn-primary" onclick="idcheck()">로그인</button>
			<button type="button" class="btn btn-primary"  id="btn-signup">회원가입</button>
			<a href="/oauth2/authorization/google" class="btn btn-danger">구글로 로그인</a>
		</form>
	</div>
	<div class="modal fade" id="idCheckModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
		<div class="modal-dialog" role="document">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="exampleModalLabel">로그인 결과</h5>
					<button type="button" class="close" data-dismiss="modal" onclick="closeModal()" aria-label="Close">
						<span aria-hidden="true">&times;</span>
					</button>
				</div>
				<div class="modal-body" id="modalBody"></div>
				<div class="modal-footer">
					<button type="button" class="btn btn-secondary" onclick="closeModal()" data-dismiss="modal">Close</button>
				</div>
			</div>
		</div>
	</div>
</body>
</html>
