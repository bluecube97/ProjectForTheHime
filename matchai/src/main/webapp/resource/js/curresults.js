let selectedLeague = 'kbo'; // Default league

document.addEventListener("DOMContentLoaded", function() {
    populateYearOptions();
    selectMonth(new Date().getMonth() + 1);
    document.getElementById('year').addEventListener('change', updateCalendar);
});

function populateYearOptions() {
    const currentYear = new Date().getFullYear();
    const yearSelect = document.getElementById('year');
    
    for (let year = 2000; year <= currentYear; year++) {
        let option = document.createElement('option');
        option.value = year;
        option.text = year;
        yearSelect.appendChild(option);
    }
    yearSelect.value = currentYear;
}

function selectLeague(league) {
    selectedLeague = league;
    console.log("selectLeague : ", selectedLeague);
    updateCalendar();
}

function selectMonth(month) {
    const monthNames = ["1월", "2월", "3월", "4월", "5월", "6월", "7월", "8월", "9월", "10월", "11월", "12월"];
    document.getElementById('month-header').innerText = monthNames[month - 1];
    console.log("selectMonth : ", month);
    populateCalendar(month);
}

function updateCalendar() {
    const selectedMonth = document.getElementById('month-header').innerText.replace('월', '').trim();
    console.log("updateCalendar : ", selectedMonth);
    selectMonth(parseInt(selectedMonth));
}

function populateCalendar(month) {
    const yearSelect = document.getElementById('year');
    const selectedYear = yearSelect.value;
    const daysInMonth = new Date(selectedYear, month, 0).getDate();
    const firstDay = new Date(selectedYear, month - 1, 1).getDay();

    let calendarBody = '';
    let dayCount = 1;

    for (let i = 0; i < 6; i++) {
        calendarBody += '<tr>';
        for (let j = 0; j < 7; j++) {
            if (i === 0 && j < firstDay) {
                calendarBody += '<td><div class="inner"></div></td>';
            } else if (dayCount <= daysInMonth) {
                calendarBody += `<td><span class="day">${dayCount}</span><div class="inner"><ul></ul></div></td>`;
                dayCount++;
            } else {
                calendarBody += '<td><div class="inner"></div></td>';
            }
        }
        calendarBody += '</tr>';
    }

    document.getElementById('calendar-body').innerHTML = calendarBody;
    fetchGameResults(selectedYear, month);
}

function fetchGameResults(year, month) {
    const data = {
        selLeague: selectedLeague,
        selYear: String(year),
        selMonth: month < 10 ? '0' + month : String(month) // Ensure month is a string
    };

    console.log("컨트롤러에 보내는 data : ", data);
    fetch('/board/getCurResults', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json(); // 응답을 JSON으로 파싱
    })
    .then(results => {
        console.log('Fetched game results:', results);
        updateCalendarWithResults(results);
    })
    .catch(error => console.error('Error fetching game results:', error));
}

function updateCalendarWithResults(results) {
    const year = document.getElementById('year').value;
    const month = document.getElementById('month-header').innerText.replace('월', '').trim();
    const days = document.querySelectorAll('.day');

    days.forEach(dayElement => {
        const day = dayElement.innerText;
        const date = `${year}-${month < 10 ? '0' + month : month}-${day < 10 ? '0' + day : day}`;

        results.forEach(game => {
            if (game.game_date === date) {
                const inner = dayElement.nextElementSibling.querySelector('.inner ul');
                const gameInfo = `${game.winteam} ${game.winscore} - ${game.losescore} ${game.loseteam}`;
                const li = document.createElement('li');
                li.textContent = gameInfo;
                inner.appendChild(li);
            }
        });
    });
}
