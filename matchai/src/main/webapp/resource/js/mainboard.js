document.addEventListener('DOMContentLoaded', function() {
    // 로고 버튼 클릭 시
    document.getElementById('btn-logo').addEventListener('click', function() {
        window.location.href = '/board/main';
    });

    /*// 모든 summarybtn 버튼에 클릭 이벤트 리스너 추가
    const summaryButtons = document.querySelectorAll('.summary');
    if (summaryButtons.length > 0) {
        summaryButtons.forEach(function(button, index) {
            button.addEventListener('click', function() {
                console.log(`Button ${index} clicked`);
                window.location.href = "/board/gameanalysis"; // 이동할 페이지 URL
            });
        });
    } else {
        console.error('No summary buttons found');
    }*/

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

function startUnity() {
    window.location.href = '/board/unity';
}

function loginPlz() {
    alert("로그인 후 사용가능 합니다.")
}

function openFreeBoard(){
    window.location.href = '/board/freeboard';
}

function openDataCenter(){
    window.location.href = '/board/datacenter';
}
