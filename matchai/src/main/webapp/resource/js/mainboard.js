// DOMContentLoaded 이벤트가 발생할 때 실행되는 함수
document.addEventListener('DOMContentLoaded', function() {

	// 로고 버튼 클릭 시
	document.getElementById('btn-logo').addEventListener('click', function() {
		window.location.href = '/board/main';
	});

	// 로그인, 로그아웃, 회원가입 버튼 요소를 가져옴
	const loginButton = document.getElementById('btn-login');
	const logoutButton = document.getElementById('btn-logout');
	const signupButton = document.getElementById('btn-signup');

	// 로그인 버튼이 존재하면 클릭 이벤트 리스너 추가
	if (loginButton) {
		// 로그인 버튼 클릭 시, 로그인 주소로 이동
		loginButton.addEventListener('click', function() {
			window.location.href = '/user/login';
		});
	}

	// 로그아웃 버튼이 존재하면 클릭 이벤트 리스너 추가
	if (logoutButton) {
		// 로그아웃 버튼 클릭 시, 로그아웃 주소로 이동
		logoutButton.addEventListener('click', function() {
			window.location.href = '/user/logout';
		});
	}

	// 회원가입 버튼이 존재하면 클릭 이벤트 리스너 추가
	if (signupButton) {
		// 회원가입 버튼 클릭 시, 회원가입 주소로 이동
		signupButton.addEventListener('click', function() {
			window.location.href = '/user/signup';
		});
	}
});
