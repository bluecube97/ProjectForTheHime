// 성공 시 리다이렉트할 URL을 저장할 변수
let redirectUrl = null;

// 로고 버튼 클릭 시
document.getElementById('btn-logo').addEventListener('click', function() {
	window.location.href = '/board/main';
});

// 회원가입 버튼 클릭 시 회원가입 페이지로 이동
document.getElementById('btn-signup').addEventListener('click', function() {
	window.location.href = '/user/signup';
});

// 로그인 정보 확인 함수
function idcheck() {
	let smail = document.getElementById('smail').value; // 입력된 이메일 값 가져오기
	let spass = document.getElementById('spass').value; // 입력된 비밀번호 값 가져오기
	const modalBody = document.querySelector('#modalBody'); // 모달의 입력할 본문 값 가져오기
console.log(smail)
	console.log(smail)

	// 입력된 이메일과 비밀번호를 JSON 문자열로 변환하여 콘솔에 출력
	console.log(JSON.stringify({ smail: smail, spass: spass }));

	// 로그인 요청을 서버로 전송
	fetch('/user/login', {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
		},
		body: JSON.stringify({ smail: smail, spass: spass }) // 이메일과 비밀번호를 요청 본문에 포함
	})
		.then(response => response.json()) // 서버 응답을 JSON으로 변환
		.then(data => {
			modalBody.textContent = data.ment; // 서버 응답의 멘트를 모달 본문에 표시
			$('#idCheckModal').modal('show'); // 모달을 표시

			// 리다이렉트 URL이 있으면 저장
			if (data.redirect) {
				redirectUrl = data.redirect;  // 성공 시 리다이렉트 URL 저장
			}
		});
}

// 모달 닫기 함수
function closeModal() {
	$('#idCheckModal').modal('hide'); // 클릭시 모달 닫기
	if (redirectUrl) { // redirectUrl 있으면
		window.location.href = redirectUrl; // 저장된 리다이렉트 URL로 이동
	}
}

fetch('/board/main', {
	method: 'GET',
	// ... 기타 옵션 ...
})
	.then(response => {
		// 헤더에서 토큰 읽기
		const token = response.headers.get('Authorization');

		// 콘솔에 토큰 출력
		console.log('Token:', token);
	})
	.catch(error => console.error('Error:', error));