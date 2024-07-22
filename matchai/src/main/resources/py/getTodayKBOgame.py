import requests
from bs4 import BeautifulSoup
from datetime import datetime
import json
import os

# 팀 이름을 ID로 매핑
team_id_map = {
    "두산": 6002,
    "LG": 5002,
    "KT": 12001,
    "SSG": 9002,
    "NC": 11001,
    "KIA": 2002,
    "롯데": 3001,
    "삼성": 1001,
    "한화": 7002,
    "키움": 10001
}

# 오늘 날짜 가져오기
now = datetime.now()
year = now.year
month = now.month
day = now.day
today = f"{day}"
today_str = now.strftime("%Y-%m-%d")

# URL 요청 및 HTML 파싱
url = f"https://statiz.sporki.com/schedule/?year={year}&month={month}"
response = requests.get(url)
soup = BeautifulSoup(response.content, 'html.parser')

# 경기 일정 추출
schedule_table = soup.find('table')
tds = schedule_table.find_all('td')

# 오늘 날짜의 경기 일정 필터링 및 출력
today_schedule = []

for td in tds:
    day_span = td.find('span', {'class': 'day'})
    if day_span and day_span.text.strip() == today:
        games_div = td.find('div', {'class': 'games'})
        if games_div:
            games = games_div.find_all('li')
            for game in games:
                team_spans = game.find_all('span', {'class': 'team'})
                teams = [team.text.strip() for team in team_spans]
                if len(teams) == 2:
                    away_team, home_team = teams
                    game_info = {
                        "away_team": team_id_map.get(away_team),
                        "home_team": team_id_map.get(home_team),
                        "game_date": today_str
                    }
                    today_schedule.append(game_info)

# JSON 파일로 저장
base_dir = os.path.dirname(__file__)
file_path = os.path.join(base_dir, 'json', 'todaysGames', f'{now.strftime("%Y%m%d")}KBOgames.json')
os.makedirs(os.path.dirname(file_path), exist_ok=True)

with open(file_path, "w", encoding="utf-8") as json_file:
    json.dump(today_schedule, json_file, ensure_ascii=False, indent=4)

if today_schedule:
    print(f"오늘 날짜의 경기 일정이 {file_path}에 JSON 파일로 저장되었습니다.")
else:
    print(f"오늘 날짜에 경기 일정이 없어서 빈 JSON 파일이 {file_path}에 저장되었습니다.")