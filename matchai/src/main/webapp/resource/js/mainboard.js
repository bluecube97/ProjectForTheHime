document.addEventListener('DOMContentLoaded', function() {
    const summaryButtons = document.querySelectorAll('.summary');
    let chart = null; // Chart 객체를 저장할 변수를 선언합니다.

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

    summaryButtons.forEach(function(button) {
        button.addEventListener('click', async function() {
            const team1 = this.getAttribute('data-team1');
            const team2 = this.getAttribute('data-team2');
            // console.log("Team1:", team1, "Team2:", team2); // team1과 team2가 제대로 출력되는지 확인합니다.

            // console.log("Team1:", team1, "Team2:", team2); // team1과 team2가 제대로 출력되는지 확인합니다.

            if (!team1 || !team2) {
                console.error('team1 or team2 data attribute is missing');
                return;
            }

            // Fetch API를 사용하여 서버에서 데이터를 가져옴
            fetch('/board/fetchGameData', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ team1Code: team1, team2Code: team2 }) // 팀 정보를 요청 본문에 포함
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            }) // 서버 응답을 JSON으로 변환
            .then(data => {

                if (!data) {
                    throw new Error('No data returned from server');
                }

                //console.log("Received data:", data);

                // 모달 열기
                const summaryModal = new bootstrap.Modal(document.getElementById('summaryModal'));
                summaryModal.show();

                // 기존 차트가 있다면 파괴
                if (chart) {
                    chart.destroy();
                }

                // 승률에서 %를 제거, 숫자 변환
                const team1WinRate = parseFloat(data.team1_winrate.replace('%', ''));
                const team2WinRate = parseFloat(data.team2_winrate.replace('%', ''));
                // 점수에서 '점' 제거, 숫자 편환
            	const team1Score = parseInt(data.team1_score.replace('점', ''));
                const team2Score = parseInt(data.team2_score.replace('점', ''));

                // 차트를 생성합니다.
                const ctx = document.getElementById('summaryChart').getContext('2d');
                chart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: ['예측 승률'],
                        datasets: [
                            {
                                label: data.team1name,
                                data: [team1WinRate], // db에서 가져온 데이터 사용
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                borderColor: 'rgba(255, 99, 132, 1)',
                                borderWidth: 1,
                                maxBarThickness: 70
                            },
                            {
                                label: data.team2name,
                                data: [team2WinRate], // db에서 가져온 데이터 사용
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

                // 서버에서 가져온 텍스트 데이터 사용
                const GPTMENT = document.getElementById('summaryText');
                GPTMENT.innerText = data.GAME_ANALYSIS;
                GPTMENT.scrollTop = 0;
                
				let team1Result = `${data.team1name} : ${team1Score} 점`;
                let team2Result = `${data.team2name} : ${team2Score} 점`;

                if (team1Score > team2Score) {
                    team1Result += ' 승리';
                    team2Result += ' 패배';
                } else if (team1Score < team2Score) {
                    team1Result += ' 패배';
                    team2Result += ' 승리';
                }

                document.getElementById('team1Score').innerText = team1Result;
                document.getElementById('team2Score').innerText = team2Result;
            })
            .catch(error => {
                console.error('Error fetching game data:', error);
                alert('경기 데이터를 가져오는 중 오류가 발생했습니다.');
            });
        });
    });

    // 모달이 닫힐 때 차트를 파괴합니다.
    document.getElementById('summaryModal').addEventListener('hidden.bs.modal', function () {
        if (chart) {
            chart.destroy();
            chart = null;
        }
    });
});

            // console.log("Team1:", team1, "Team2:", team2); // team1과 team2가 제대로 출력되는지 확인합니다.

            if (!team1 || !team2) {
                console.error('team1 or team2 data attribute is missing');
                return;
            }

            // Fetch API를 사용하여 서버에서 데이터를 가져옴
            fetch('/board/fetchGameData', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ team1Code: team1, team2Code: team2 }) // 팀 정보를 요청 본문에 포함
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            }) // 서버 응답을 JSON으로 변환
            .then(data => {

                if (!data) {
                    throw new Error('No data returned from server');
                }

                //console.log("Received data:", data);

                // 모달 열기
                const summaryModal = new bootstrap.Modal(document.getElementById('summaryModal'));
                summaryModal.show();

                // 기존 차트가 있다면 파괴
                if (chart) {
                    chart.destroy();
                }

                // 승률에서 %를 제거, 숫자 변환
                const team1WinRate = parseFloat(data.team1_winrate.replace('%', ''));
                const team2WinRate = parseFloat(data.team2_winrate.replace('%', ''));
                // 점수에서 '점' 제거, 숫자 편환
            	const team1Score = parseInt(data.team1_score.replace('점', ''));
                const team2Score = parseInt(data.team2_score.replace('점', ''));

                // 차트를 생성합니다.
                const ctx = document.getElementById('summaryChart').getContext('2d');
                chart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: ['예측 승률'],
                        datasets: [
                            {
                                label: data.team1name,
                                data: [team1WinRate], // db에서 가져온 데이터 사용
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                borderColor: 'rgba(255, 99, 132, 1)',
                                borderWidth: 1,
                                maxBarThickness: 70
                            },
                            {
                                label: data.team2name,
                                data: [team2WinRate], // db에서 가져온 데이터 사용
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

                // 서버에서 가져온 텍스트 데이터 사용
                const GPTMENT = document.getElementById('summaryText');
                GPTMENT.innerText = data.GAME_ANALYSIS;
                GPTMENT.scrollTop = 0;
                
				let team1Result = `${data.team1name} : ${team1Score} 점`;
                let team2Result = `${data.team2name} : ${team2Score} 점`;

                if (team1Score > team2Score) {
                    team1Result += ' 승리';
                    team2Result += ' 패배';
                } else if (team1Score < team2Score) {
                    team1Result += ' 패배';
                    team2Result += ' 승리';
                }

                document.getElementById('team1Score').innerText = team1Result;
                document.getElementById('team2Score').innerText = team2Result;
            })
            .catch(error => {
                console.error('Error fetching game data:', error);
                alert('경기 데이터를 가져오는 중 오류가 발생했습니다.');
            });
        });
    });

    // 모달이 닫힐 때 차트를 파괴합니다.
    document.getElementById('summaryModal').addEventListener('hidden.bs.modal', function () {
        if (chart) {
            chart.destroy();
            chart = null;
        }
    });
});

            if (!team1 || !team2) {
                console.error('team1 or team2 data attribute is missing');
                return;
            }

            // Fetch API를 사용하여 서버에서 데이터를 가져옴
            fetch('/board/fetchGameData', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ team1Code: team1, team2Code: team2 }) // 팀 정보를 요청 본문에 포함
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            }) // 서버 응답을 JSON으로 변환
            .then(data => {
                if (!data) {
                    throw new Error('No data returned from server');
                }

                //console.log("Received data:", data);

                // 모달 열기
                const summaryModal = new bootstrap.Modal(document.getElementById('summaryModal'));
                summaryModal.show();

                // 기존 차트가 있다면 파괴
                if (chart) {
                    chart.destroy();
                }

                // 승률에서 %를 제거, 숫자 변환
                const team1WinRate = parseFloat(data.TEAM1_WINRATE.replace('%', ''));
                const team2WinRate = parseFloat(data.TEAM2_WINRATE.replace('%', ''));
                // 점수에서 '점' 제거, 숫자 편환
            	const team1Score = parseInt(data.TEAM1_SCORE.replace('점', ''));
                const team2Score = parseInt(data.TEAM2_SCORE.replace('점', ''));

                // 차트를 생성합니다.
                const ctx = document.getElementById('summaryChart').getContext('2d');
                chart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: ['예측 승률'],
                        datasets: [
                            {
                                label: data.TEAM1NAME,
                                data: [team1WinRate], // db에서 가져온 데이터 사용
                                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                borderColor: 'rgba(255, 99, 132, 1)',
                                borderWidth: 1,
                                maxBarThickness: 70
                            },
                            {
                                label: data.TEAM2NAME,
                                data: [team2WinRate], // db에서 가져온 데이터 사용
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

                // 서버에서 가져온 텍스트 데이터 사용
                const GPTMENT = document.getElementById('summaryText');
                GPTMENT.innerText = data.GAME_ANALYSIS;
                GPTMENT.scrollTop = 0;
                
				let team1Result = `${data.TEAM1NAME} : ${team1Score} 점`;
                let team2Result = `${data.TEAM2NAME} : ${team2Score} 점`;

                if (team1Score > team2Score) {
                    team1Result += ' 승리';
                    team2Result += ' 패배';
                } else if (team1Score < team2Score) {
                    team1Result += ' 패배';
                    team2Result += ' 승리';
                }

                document.getElementById('team1Score').innerText = team1Result;
                document.getElementById('team2Score').innerText = team2Result;
            })
            .catch(error => {
                console.error('Error fetching game data:', error);
                alert('경기 데이터를 가져오는 중 오류가 발생했습니다.');
            });
        });
    });

    // 모달이 닫힐 때 차트를 파괴합니다.
    document.getElementById('summaryModal').addEventListener('hidden.bs.modal', function () {
        if (chart) {
            chart.destroy();
            chart = null;
        }
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

function startUnity(){
	window.location.href = '/board/unity';
}

function  loginPlz(){
	alert("로그인 후 사용가능 합니다.")
}

