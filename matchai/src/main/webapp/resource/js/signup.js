// 로고 버튼 클릭 시
document.getElementById('btn-logo').addEventListener('click', function() {
	window.location.href = '/board/main';
});

// 이메일 중복 확인 함수
async function mailCheck() {
	let smail = document.getElementById('smail').value;

	// 이메일 입력 여부 확인
	if (smail === "" || smail.length <= 0) {
		alert("이메일을 입력해주세요");
		document.getElementById('smail').classList.remove('valid');
		document.getElementById('smail').classList.add('invalid');
		return; // 유효성 검사 실패 시 함수 종료
	}

	try {
		let response = await fetch('/user/emailcheck', {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({ smail: smail })
		});
		let resp = await response.json();

		if (resp.connection === "O") {
			document.getElementById('smail').classList.remove('invalid');
			document.getElementById('smail').classList.add('valid');
			return true; // 유효성 검사 성공 시 true 반환
		} else {
			alert(resp.ment);
			document.getElementById('smail').classList.remove('valid');
			document.getElementById('smail').classList.add('invalid');
			return false; // 유효성 검사 실패 시 false 반환
		}
	} catch (error) {
		console.error('Error', error);
		return false; // 에러 발생 시 false 반환
	}
}

// 비밀번호 유효성 검사 함수
function passCheck() {
	let spass = document.querySelector('#spass').value;
	let regex = /^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[!@#$%^&*?_]).{8,16}$/;

	// 비밀번호가 유효한지 정규식을 사용해 확인
	if (spass == "" || spass <= 0) {
		alert("비밀번호를 입력하세요.");
		document.getElementById('spass').classList.remove('valid');
		document.getElementById('spass').classList.add('invalid');
		return false; // 유효성 검사 실패 반환
	} else if (!regex.test(spass)) {
		alert("8자리 이상 특수문자를 사용한 비밀번호만 사용 가능 합니다.");
		document.getElementById('spass').classList.remove('valid');
		document.getElementById('spass').classList.add('invalid');
		return false; // 유효성 검사 실패 반환
	} else {
		document.getElementById('spass').classList.remove('invalid');
		document.getElementById('spass').classList.add('valid');
		return true; // 유효성 검사 성공 반환
	}
}

/**
 * 비밀번호 확인 함수
 */
function repassCheck() {
	let srepass = document.querySelector('#srepass').value;
	let spass = document.querySelector('#spass').value;

	// 비밀번호와 비밀번호 확인 필드가 일치하는지 확인
	if (srepass == "" || srepass <= 0) {
		alert("비밀번호 확인을 입력하세요.");
		document.getElementById('srepass').classList.remove('valid');
		document.getElementById('srepass').classList.add('invalid');
		return false; // 유효성 검사 실패 반환
	} else if (srepass === spass) {
		document.getElementById('srepass').classList.remove('invalid');
		document.getElementById('srepass').classList.add('valid');
		return true; // 유효성 검사 성공 반환
	} else {
		alert("비밀번호를 다시 입력해주세요.");
		document.getElementById('srepass').classList.remove('valid');
		document.getElementById('srepass').classList.add('invalid');
		return false; // 유효성 검사 실패 반환
	}
}

/**
 * 이름 유효성 검사 함수
 */
function nameCheck() {
	let sname = document.querySelector('#sname').value;
	let leng = sname.length;

	// 이름이 입력되었는지 및 길이 조건을 확인
	if (sname == "" || leng <= 0) {
		alert("이름을 입력하세요.");
		document.getElementById('sname').classList.remove('valid');
		document.getElementById('sname').classList.add('invalid');
		return false; // 유효성 검사 실패 반환
	} else if (leng > 20) {
		alert("이름을 20글자 이내로 입력하세요.");
		document.getElementById('sname').classList.remove('valid');
		document.getElementById('sname').classList.add('invalid');
		return false; // 유효성 검사 실패 반환
	} else {
		document.getElementById('sname').classList.remove('invalid');
		document.getElementById('sname').classList.add('valid');
		return true; // 유효성 검사 성공 반환
	}
}

/**
 * 닉네임 유효성 검사 함수
 */
function nickCheck() {
	let snick = document.querySelector('#snick').value;
	let leng = snick.length;

	// 닉네임이 입력되었는지 및 길이 조건을 확인
	if (snick == "" || leng <= 0) {
		alert("닉네임을 입력하세요.");
		document.getElementById('snick').classList.remove('valid');
		document.getElementById('snick').classList.add('invalid');
		return false; // 유효성 검사 실패 반환
	} else if (leng > 20) {
		alert("닉네임을 20글자 이내로 입력하세요.");
		document.getElementById('snick').classList.remove('valid');
		document.getElementById('snick').classList.add('invalid');
		return false; // 유효성 검사 실패 반환
	} else {
		document.getElementById('snick').classList.remove('invalid');
		document.getElementById('snick').classList.add('valid');
		return true; // 유효성 검사 성공 반환
	}
}

async function signUp() {
	//유효성 검사 함수들 호출
	let emailValid = mailCheck();
	if (!emailValid) return;

	let passValid = passCheck();
	if (!passValid) return;

	let repassValid = repassCheck();
	if (!repassValid) return;

	let nameValid = nameCheck();
	if (!nameValid) return;

	let nickValid = nickCheck();
	if (!nickValid) return;

	// 모든 필드가 유효한 경우에만 회원가입 요청을 서버에 전송
	let smail = document.getElementById('smail').value;
	let spass = document.getElementById('spass').value;
	let srepass = document.getElementById('srepass').value;
	let sname = document.getElementById('sname').value;
	let snick = document.getElementById('snick').value;
	let sage = document.getElementById('sage').value;

	// 선호 리그와 팀 정보를 가져오기
	let league = document.getElementById('league').value;
	let teams = []; // 선택된 팀 목록을 담을 배열

	let selectedTeams = document.querySelectorAll('#chsleagues option');

	// 선택된 팀들을 teams 배열에 추가
	for (let i = 0; i < selectedTeams.length; i++) {
		let teamOption = selectedTeams[i];
		let team = {
			league: league, // 리그 코드 추가
			teamCode: teamOption.value, // 팀 코드
			teamName: teamOption.textContent // 팀 이름
		};
		teams.push(team);
	}

	// 서버에 전송할 데이터 객체 생성
	let data = {
		smail: smail,
		spass: spass,
		srepass: srepass,
		sname: sname,
		snick: snick,
		sage: sage,
		league: league,
		teams: teams
	};

	// 회원가입 요청을 서버에 전송
	fetch('/user/signup', {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify(data)
	})
		.then(response => response.json())
		.then(resp => {
			// 회원가입 성공 시 리다이렉트
			if (resp.connection === "O") {
				window.location.href = resp.redirect;
			} else {
				alert(resp.ment);
			}
		})
		.catch(error => console.error('Error', error)); // 에러 처리
}


/**
 * 페이지 로드 시 기본 리그의 팀 목록을 가져오는 함수
 */
document.addEventListener('DOMContentLoaded', function() {
	// 페이지 로드될 때 기본 리그의 팀 목록을 가져오기
	getTeams('KB'); // 예를 들어 KBO 리그의 팀 목록을 초기화로 설정

	// 이벤트 핸들러 등록 (리그 선택이 변경될 때마다 호출)
	document.getElementById('league').addEventListener('change', function() {
		let selectedLeague = this.value;
		getTeams(selectedLeague); // 선택된 리그에 따른 팀 목록 가져오기
	});
});

/**
 * 서버에 리그에 해당하는 팀 정보를 요청하는 함수
 */
function getTeams(league) {
	fetch('/user/getteams', {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' }, // 요청의 헤더를 설정합니다.
		body: JSON.stringify({ league: league }) // 요청 본문에 리그 정보를 JSON 형식으로 포함시킵니다.
	})
		.then(response => response.json()) // 응답을 JSON 형식으로 파싱합니다.
		.then(resp => {
			let listleagues = document.getElementById('listleagues');
			listleagues.innerHTML = ''; // 기존 팀 목록을 초기화합니다.

			// 서버에서 받은 팀 목록을 반복하여 옵션 요소를 생성하고 추가합니다.
			for (let i = 0; i < resp.teams.length; i++) {
				let team = resp.teams[i];

				// 각 팀을 선택할 수 있는 옵션 요소를 생성합니다.
				let option = document.createElement('option');
				option.value = team.TEAM_CODE; // 값은 팀 코드입니다.
				option.text = team.TEAMNAME_KR; // 텍스트는 팀 이름입니다.

				// 팀 목록을 표시할 DOM 요소에 옵션을 추가합니다.
				listleagues.appendChild(option);
			}
		})
		.catch(error => console.error('Error', error)); // 에러가 발생하면 콘솔에 에러 메시지를 출력합니다.
}

/**
 * 선택된 팀을 추가하는 함수
 */
function addteam() {
	let listleagues = document.getElementById('listleagues');
	let chsleagues = document.getElementById('chsleagues');  // 선택한 리그가 담길 변수

	// 선택된 옵션들을 선택된 팀 목록에 추가합니다.
	for (let i = 0; i < listleagues.options.length; i++) {
		let option = listleagues.options[i];
		if (option.selected) {
			// 이미 존재하는지 확인
			let exists = false;
			for (let j = 0; j < chsleagues.options.length; j++) {
				if (chsleagues.options[j].value === option.value) {
					exists = true;
					break;
				}
			}

			// 존재하지 않을 때만 추가
			if (!exists) {
				let newOption = document.createElement('option');
				newOption.value = option.value;
				newOption.text = option.text;
				chsleagues.appendChild(newOption);
			}
		}
	}
}

/**
 * 선택된 팀을 삭제하는 함수
 */
function delteam() {
	var right_list = document.getElementById("chsleagues");

	// 이동된 아이템을 right_list에서 삭제
	for (var i = right_list.options.length - 1; i >= 0; i--) {
		if (right_list.options[i].selected) {
			right_list.remove(i);
		}
	}
}
