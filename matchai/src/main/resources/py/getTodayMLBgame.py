import json
import statsapi
from datetime import datetime, timedelta
import os

# 한국 시간 기준으로 현재 날짜 가져오기
now_kst = datetime.now() + timedelta(hours=14)
today_kst = now_kst.strftime('%Y-%m-%d')

# 오늘 날짜의 경기 일정 가져오기
games = statsapi.schedule(start_date=today_kst, end_date=today_kst)

# 경기 일정 JSON 형식으로 변환
games_json = []
if games:
    for game in games:
        games_json.append({
            "away_team": game['away_id'],
            "home_team": game['home_id'],
            "game_date": game['game_date']
        })
else:
    games_json = {"message": "오늘은 경기가 없습니다."}

# 상대 경로로 파일 경로 및 이름 설정
base_dir = os.path.dirname(__file__)
file_path = os.path.join(base_dir, 'json', 'todaysGames', f'{now_kst.strftime("%Y%m%d")}MLBgames.json')

# JSON 데이터를 파일로 저장
os.makedirs(os.path.dirname(file_path), exist_ok=True)
with open(file_path, 'w', encoding='utf-8') as f:
    json.dump(games_json, f, ensure_ascii=False, indent=4)

print(f"경기 일정이 '{file_path}' 파일에 저장되었습니다.")