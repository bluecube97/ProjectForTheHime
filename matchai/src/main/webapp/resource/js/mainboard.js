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

	// summarybtn 클릭 시 모달을 열고 막대 그래프를 표시
	const summaryButtons = document.querySelectorAll('.summary');

	summaryButtons.forEach(function(button) {
		button.addEventListener('click', function() {
			const summaryModal = new bootstrap.Modal(document.getElementById('summaryModal'));
			summaryModal.show();

			const ctx = document.getElementById('summaryChart').getContext('2d');
			const chart = new Chart(ctx, {
				type: 'bar',
				data: {
					labels: ['예측 승률'],
					datasets: [
						{
							label: '팀 A',
							data: [80], // 실제 데이터로 변경
							backgroundColor: 'rgba(255, 99, 132, 0.2)',
							borderColor: 'rgba(255, 99, 132, 1)',
							borderWidth: 1,
							maxBarThickness: 70
						},
						{
							label: '팀 B',
							data: [20], // 실제 데이터로 변경
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
					},
					plugins: {
						tooltip: {
							backgroundColor: 'rgba(0, 0, 0, 0.4)',
							bodyFont: { size: 20 },
						}
					}
				}
			});
			console.log(chart);

			// 여기에 원하는 텍스트 추가
			const gptment = ' 분석a 끗입니다요 끗끗끗';
			const GPTMENT = document.getElementById('summaryText');
			GPTMENT.innerText = gptment;
			GPTMENT.scrollTop = 0;
		});
	});
});

document.addEventListener('DOMContentLoaded', function() {
	// 오늘의 경기 KBO 클릭 시, KBO컨텐츠 ON, MLB컨텐츠 OFF
	document.getElementById('kbo-tab').addEventListener('click', function() {
		document.getElementById('kbo-content').style.display = 'block';
		document.getElementById('mlb-content').style.display = 'none';
	});
	// 오늘의 경기 MLB 클릭 시, MLBO컨텐츠 ON, KBO컨텐츠 OFF
	document.getElementById('mlb-tab').addEventListener('click', function() {
		document.getElementById('kbo-content').style.display = 'none';
		document.getElementById('mlb-content').style.display = 'block';
	});
});


function startUnity() {
	window.location.href = '/board/unity';
}
