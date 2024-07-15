import json
import os
import pymysql
import sys
from statusVO import Daughter, load_daughter_status

def get_db_connection():
    """
    데이터베이스 연결을 설정하는 함수.
    """
    return pymysql.connect(
        host='192.168.0.78',  # DB 서버 호스트
        user='studyuser',  # DB 사용자 이름
        password='1111',  # DB 비밀번호
        database='baseball'  # 데이터베이스 이름
    )

def get_pid_java():
    """
    Java에서 PID를 입력 받아오는 함수.
    """
    if len(sys.argv) > 1:
        pid = sys.argv[1]
        print("pid들옴")
        print("pid는 " + pid)

        if pid == "END_OF_INPUT":
            return None
        if pid:
            return pid
    return None

def load_log():
    current_dir = os.path.dirname(os.path.abspath(__file__))
    json_dir = os.path.join(current_dir, '..', '..', 'json', 'game')
    json_file = os.path.join(json_dir, 'conversation.json')

    try:
        with open(json_file, 'r', encoding='utf-8') as file:
            return json.load(file)
    except FileNotFoundError:
        print(f"JSON file not found at: {json_file}")
        return None
    except json.JSONDecodeError as e:
        print(f"Error decoding JSON file {json_file}: {e}")
        return None

def load_status():
    """
    JSON 파일에서 딸의 정보를 불러오는 함수.
    """
    current_dir = os.path.dirname(os.path.abspath(__file__))
    json_dir = os.path.join(current_dir, '..', '..', 'json', 'game')
    json_file = os.path.join(json_dir, 'statusRecord.json')

    if os.path.exists(json_file):
        with open(json_file, 'r', encoding='utf-8') as f:
            return json.load(f)
    else:
        print(f"No JSON file found at: {json_file}")
        return None

def save_log(pid, data):
    connection = get_db_connection()
    try:
        with connection.cursor() as cursor:
            sql = """
            INSERT INTO TBL_UNT_GPTLOG_NT01 (PID, CHAT_LOG)
            VALUES (%s, %s)
            """
            cursor.execute(sql, (pid, json.dumps(data, ensure_ascii=False)))
        connection.commit()
    finally:
        connection.close()

def save_status(daughter, pid):
    """
    DB에 딸의 정보를 삽입하는 함수.
    """
    if daughter is None or pid is None:
        print("Daughter data or PID is missing. Cannot save status.")
        return

    required_fields = ["name", "age", "sex", "mbti", "hp", "mp", "mood", "stress", "fatigue", "E", "I", "S", "N", "T", "F", "J", "P"]
    missing_fields = [field for field in required_fields if getattr(daughter, field, None) is None]

    if missing_fields:
        print(f"Missing required fields: {', '.join(missing_fields)}")
        return

    connection = get_db_connection()
    try:
        with connection.cursor() as cursor:
            sql = """
                INSERT INTO TBL_UNT_DSTATS_NT01 (
                    PID, D_NAME, AGE, SEX, MBTI, CHP, CMP, MOOD, STRESS, FATIGUE, E, I, S, N, T, F, J, P
                ) VALUES (
                    %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s
                )
            """
            cursor.execute(sql, (
                pid,
                daughter.name,
                daughter.age,
                daughter.sex,
                daughter.mbti,
                daughter.hp,
                daughter.mp,
                daughter.mood,
                daughter.stress,
                daughter.fatigue,
                daughter.E,
                daughter.I,
                daughter.S,
                daughter.N,
                daughter.T,
                daughter.F,
                daughter.J,
                daughter.P
            ))
        connection.commit()
    finally:
        connection.close()

def main():
    pid = get_pid_java()  # DB사용 시  KEY값을 담음
    if pid is None:
        print("No PID provided.")
        return

    data = load_log()  # 대화 기록이 저장되어있는 json파일 값을 담음
    if data is None:
        print("No log data loaded.")
        return

    save_log(pid, data)  # 대화 기록 insert

    dstatus = load_status()  # 변경된 딸의 스탯을 담음
    if dstatus is None:
        print(json.dumps({"error": "Failed to retrieve user information from database."}))
        return
    user_info = dstatus["daughter"]

    daughter = Daughter(
        name=user_info["name"],
        age=user_info["age"],
        sex=user_info["sex"],
        mbti=user_info["mbti"],
        hp=user_info["hp"],
        mp=user_info["mp"],
        mood=user_info["mood"],
        stress=user_info["stress"],
        fatigue=user_info["fatigue"],
        E=user_info["E"],
        I=user_info["I"],
        S=user_info["S"],
        N=user_info["N"],
        T=user_info["T"],
        F=user_info["F"],
        J=user_info["J"],
        P=user_info["P"]
    )
    save_status(daughter, pid)

if __name__ == "__main__":
    main()
