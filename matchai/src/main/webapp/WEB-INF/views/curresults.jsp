<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>경기일정</title>
<script src="/resource/js/curresults.js" defer></script>
</head>
<body>
    <h1>경기일정</h1>

    <label for="league">리그 선택:</label>
    <button id="mlb-button" value="mlb" onclick="selectLeague('mlb')">MLB</button>
    <button id="kbo-button" value="kbo" onclick="selectLeague('kbo')">KBO</button>

    <label for="year">년도 선택:</label>
    <select id="year" name="year" onchange="updateCalendar()">
    </select>

    <div class="month-selector">
        <button onclick="selectMonth(1)">1월</button>
        <button onclick="selectMonth(2)">2월</button>
        <button onclick="selectMonth(3)">3월</button>
        <button onclick="selectMonth(4)">4월</button>
        <button onclick="selectMonth(5)">5월</button>
        <button onclick="selectMonth(6)">6월</button>
        <button onclick="selectMonth(7)">7월</button>
        <button onclick="selectMonth(8)">8월</button>
        <button onclick="selectMonth(9)">9월</button>
        <button onclick="selectMonth(10)">10월</button>
        <button onclick="selectMonth(11)">11월</button>
        <button onclick="selectMonth(12)">12월</button>
    </div>

    <div class="box_type_board">
        <div class="item_box cw100">
            <div class="inner">
                <div class="sh_box">
                    <div class="box_head" id="month-header">7월</div>
                    <div class="box_cont">
                        <div class="calendar_area">
                            <table border="1">
                                <thead>
                                    <tr>
                                        <th>월</th>
                                        <th>화</th>
                                        <th>수</th>
                                        <th>목</th>
                                        <th>금</th>
                                        <th>토</th>
                                        <th>일</th>
                                    </tr>
                                </thead>
                                <tbody id="calendar-body">
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
