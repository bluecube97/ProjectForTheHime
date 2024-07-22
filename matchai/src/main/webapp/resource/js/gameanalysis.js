
/**
 * 
 */

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


	function startUnity() {
		window.location.href = '/board/unity';
	}

	function loginPlz() {
		alert("로그인 후 사용가능 합니다.")
	}


	// 예시 데이터 (실제 데이터로 변경)
	const teamAData = 80;
	const teamBData = 20;
	const gptment = '********가라 데이터 입니다용가리어카*********      ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ';

	// 차트 생성
	const ctx = document.getElementById('summaryChart').getContext('2d');
	const chart = new Chart(ctx, {
		type: 'bar',
		data: {
			labels: ['예측 승률'],
			datasets: [
				{
					label: '팀 A',
					data: [teamAData],
					backgroundColor: 'rgba(255, 99, 132, 0.2)',
					borderColor: 'rgba(255, 99, 132, 1)',
					borderWidth: 1,
					maxBarThickness: 70
				},
				{
					label: '팀 B',
					data: [teamBData],
					backgroundColor: 'rgba(54, 162, 235, 0.2)',
					borderColor: 'rgba(54, 162, 235, 1)',
					borderWidth: 1,
					maxBarThickness: 70
				}
			]
		},
		options: {
			responsive: false,
			scales: {
				y: {
					beginAtZero: true
				}
			}
		}
	});

	// GPTMENT 텍스트 추가
	const gptmentElement = document.getElementById('summaryText');
	gptmentElement.innerText = gptment;

});
document.addEventListener('DOMContentLoaded', function () {
	try {
		// 로고 버튼 클릭 시
		document.getElementById('btn-logo').addEventListener('click', function () {
			window.location.href = '/board/main';
		});

		// 로그인, 로그아웃, 회원가입 버튼 요소를 가져옴
		const loginButton = document.getElementById('btn-login');
		const logoutButton = document.getElementById('btn-logout');
		const signupButton = document.getElementById('btn-signup');

		// 로그인 버튼이 존재하면 클릭 이벤트 리스너 추가
		if (loginButton) {
			loginButton.addEventListener('click', function () {
				window.location.href = '/user/login';
			});
		}

		// 로그아웃 버튼이 존재하면 클릭 이벤트 리스너 추가
		if (logoutButton) {
			logoutButton.addEventListener('click', function () {
				window.location.href = '/user/logout';
			});
		}

		// 회원가입 버튼이 존재하면 클릭 이벤트 리스너 추가
		if (signupButton) {
			signupButton.addEventListener('click', function () {
				window.location.href = '/user/signup';
			});
		}

		// JSON 데이터를 HTML 요소에서 가져옴
		let team1name = document.getElementById('team1name').value; // 예시로 '%'
		let team2name = document.getElementById('team2name').value; // 예시로 '%'
		let team1_winrate = document.getElementById('team1_winrate').value; // 예시로 '%'
		let team2_winrate = document.getElementById('team2_winrate').value; // 예시로 '%'
		let game_analysis = document.getElementById('game_analysis').value; // 예시로 텍스트

		// 데이터 처리
		const teamAData = parseFloat(team1_winrate.replace('%', ''));
		const teamBData = parseFloat(team2_winrate.replace('%', ''));
		const gptment = game_analysis;

		console.log(teamAData); // 예: 55
		console.log(teamBData); // 예: 45
		console.log(gptment);

		// 차트 생성
		const ctx = document.getElementById('summaryChart').getContext('2d');
		const chart = new Chart(ctx, {
			type: 'bar',
			data: {
				labels: ['예측 승률'],
				datasets: [
					{
						label: team1name,
						data: [teamAData],
						backgroundColor: 'rgba(255, 99, 132, 0.2)',
						borderColor: 'rgba(255, 99, 132, 1)',
						borderWidth: 1,
						maxBarThickness: 70
					},
					{
						label: team2name,
						data: [teamBData],
						backgroundColor: 'rgba(54, 162, 235, 0.2)',
						borderColor: 'rgba(54, 162, 235, 1)',
						borderWidth: 1,
						maxBarThickness: 70
					}
				]
			},
			options: {
				responsive: false,
				scales: {
					y: {
						beginAtZero: true
					}
				}
			}
		});

		// GPTMENT 텍스트 추가
		const gptmentElement = document.getElementById('summaryText');
		gptmentElement.innerText = gptment;

	} catch (e) {
		console.error("처리 중 오류 발생:", e);
	}
});

function addComment() {
	// matchcode와 댓글 내용 가져오기
	let matchcode = document.getElementById('matchcode').value;
	const comment = document.getElementById('comment').value;

	// 댓글 내용이 없거나 빈 문자열인 경우
	if (comment == null || comment.trim() === '') {
		alert("댓글은 비워 둘 수 없습니다");
		return;
	}

	// 서버에 댓글 전송
	fetch('/board/comment', {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json'
		},
		body: JSON.stringify({
			matchcode: matchcode,
			memo: comment
		})
	})
		.then(response => response.json()) // JSON 응답 처리
		.then(data => {
			// 응답 상태에 따라 메시지 표시
			if (data.status === 'success') {
				alert(data.message);
			} else {
				alert(`오류 발생: ${data.message}`);
			}
		})
		.catch(error => {
			console.error('Error:', error);
			alert('서버와의 통신 중 오류가 발생했습니다.');
		});
}

