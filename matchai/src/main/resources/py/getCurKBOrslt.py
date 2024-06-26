import requests
from bs4 import BeautifulSoup
from datetime import datetime, timedelta
import json
import os

# 팀 이름과 팀 코드를 매핑하는 딕셔너리
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

def get_game_results(year, month, day):
    url = f"https://statiz.sporki.com/schedule/?year={year}&month={month}"
    response = requests.get(url)
    response.raise_for_status()

    soup = BeautifulSoup(response.text, 'html.parser')

    results = []

    # 날짜에 해당하는 span 태그를 찾습니다.
    day_span = soup.find('span', class_='day', string=str(day))
    if not day_span:
        print(f"No data found for {year}-{month:02}-{day:02}")
        return results

    day_td = day_span.find_parent('td')
    if not day_td:
        print(f"No td element found for the day {day}")
        return results
    
    # 게임 정보를 포함하는 div.games 요소를 찾습니다.
    games_div = day_td.find('div', class_='games')
    if not games_div:
        print("No games div found")
        return results

    # 각 경기 정보를 포함하는 ul 태그를 찾습니다.
    ul_tags = games_div.find_all('ul')
    if not ul_tags:
        print("No ul tags found inside games div")
        return results

    for ul in ul_tags:
        li_tags = ul.find_all('li')

        for li in li_tags:
            game_info = {}
            a_tag = li.find('a')
            if a_tag:
                # 이긴 팀과 진 팀 정보를 포함하는 span 태그를 찾습니다.
                team_spans = li.find_all('span', class_='team')
                score_spans = li.find_all('span', class_='score')
                lead_span = li.find('span', class_='score lead')

                if len(team_spans) == 2 and len(score_spans) == 2 and lead_span:
                    # winning_team_span을 찾을 때, style 속성의 공백을 제거하고 정확히 일치하도록 수정
                    winning_team_span = next((span for span in team_spans if 'color:#FFFFFF' in span.get('style', '').replace(' ', '')), None)
                    if winning_team_span:
                        winning_team = winning_team_span.get_text(strip=True)
                        winning_score = lead_span.get_text(strip=True)
                        losing_team = None
                        losing_score = None
                        
                        for span in team_spans:
                            if span != winning_team_span:
                                losing_team = span.get_text(strip=True)
                                break

                        for span in score_spans:
                            if span != lead_span:
                                losing_score = span.get_text(strip=True)
                                break

                        # 팀 이름을 팀 코드로 매핑
                        winning_team_code = str(team_id_map.get(winning_team, "Unknown"))
                        losing_team_code = str(team_id_map.get(losing_team, "Unknown"))

                        game_info = {
                            "DATE": f"{year}-{month:02}-{day:02}",
                            "WINTEAM": winning_team_code,
                            "LOSETEAM": losing_team_code,
                            "WINSCORE": winning_score,
                            "LOSESCORE": losing_score
                        }

                        results.append(game_info)

    return results

# 어제 날짜 계산
yesterday = datetime.today() - timedelta(days=1)
year = yesterday.year
month = yesterday.month
day = yesterday.day

game_results = get_game_results(year, month, day)

# JSON 파일로 저장
file_name = f"{year}{month:02}{day:02}KBOresult.json"
base_dir = os.path.dirname(__file__)
file_path = os.path.join(base_dir, 'json', 'todaysGames', file_name)
os.makedirs(os.path.dirname(file_path), exist_ok=True)

with open(file_path, 'w', encoding='utf-8') as json_file:
    if game_results:
        json.dump(game_results, json_file, ensure_ascii=False, indent=4)
        print(f"결과가 {file_path}에 저장되었습니다.")
    else:
        json.dump([], json_file)
        print("경기 결과가 없어서 빈 JSON 파일을 생성했습니다.")