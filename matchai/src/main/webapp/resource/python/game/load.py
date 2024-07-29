import json
import os
import psycopg2
import sys
from statusVO import Daughter, load_daughter_status
from psycopg2.extras import DictCursor

# 데이터베이스 연결 설정 함수
def get_db_connection():
    """
    데이터베이스 연결을 설정하는 함수.
    """
    return psycopg2.connect(
        host='13.125.238.85',  # DB 서버 호스트
        user='baseball',  # DB 사용자 이름
        password='1111',  # DB 비밀번호
        database='baseball'  # 데이터베이스 이름
    )


# Java에서 PID 입력 프롬프트 받아오기
def get_pid_java():
    """
    Java에서 PID를 입력 받아오는 함수.
    """
    if len(sys.argv) > 1:
        pid = sys.argv[1]
        print("pid들옴")
        print("pid는 "+pid)

        if pid == "END_OF_INPUT":
            return None
        if pid:
            return pid
    return None

# DB에서 특정 사용자의 최신 MBTI 통계 정보를 가져오는 함수
def get_d_stats(pid):
    """
    DB에서 PID에 해당하는 사용자의 최신 MBTI 통계 정보를 가져오는 함수.
    """
    connection = get_db_connection()
    try:
        with connection.cursor(cursor_factory=DictCursor) as cursor:
            sql = """
                SELECT D_NAME AS name,
                       AGE AS age, 
                       SEX AS sex, 
                       MBTI AS mbti, 
                       CHP AS hp, 
                       CMP AS mp,
                       MOOD AS mood, 
                       STRESS AS stress,
                       FATIGUE AS fatigue,
                       E AS e,
                       I AS i,
                       S AS s,
                       N AS n,
                       T AS t,
                       F AS f,
                       J AS j,
                       P AS p
                  FROM baseball.TBL_UNT_DSTATS_NT01
                 WHERE PID = %s
                 ORDER BY CREATED_AT DESC
                 LIMIT 1
            """
            cursor.execute(sql, (pid,))
            result = cursor.fetchone()
            if result:
                return result
            else:
                print(f"No data found for PID: {pid}")
                return None
    finally:
        connection.close()

def get_chatLog(pid):
    connection = get_db_connection()
    try:
        with connection.cursor(cursor_factory=DictCursor) as cursor:
            sql = """
             SELECT CHAT_LOG
            FROM TBL_UNT_GPTLOG_NT01
            WHERE PID = %s
            ORDER BY CREATED_AT DESC
            LIMIT 1
            """
            cursor.execute(sql, (pid))
            result = cursor.fetchone()
            if result:
                chat_log = result.get('CHAT_LOG')
                if chat_log:
                    try:
                        return json.loads(chat_log)
                    except json.JSONDecodeError:
                        print(f"Error decoding JSON from CHAT_LOG for PID: {pid}")
                        return None
                else:
                    print(f"No CHAT_LOG found for PID: {pid}")
                    return None
            else:
                print(f"No data found for PID: {pid}")
                return None
    finally:
        connection.close()


# JSON 파일 저장 및 기존 파일 삭제
def load_to_json(daughter):
    """
    딸의 정보를 JSON 파일에 저장하고, 기존 파일이 있을 경우 삭제하는 함수.
    """
    current_dir = os.path.dirname(os.path.abspath(__file__))
    json_dir = os.path.join(current_dir, '..', '..', 'json', 'game')
    json_file = os.path.join(json_dir, 'daughter_status.json')

    # 기존 파일 삭제
    if os.path.exists(json_file):
        os.remove(json_file)

    # 새로운 JSON 파일 생성
    with open(json_file, 'w', encoding='utf-8') as f:
        json.dump({
            "daughter": {
                "name": daughter.name,
                "age": daughter.age,
                "sex": daughter.sex,
                "mbti": daughter.mbti,
                "hp": daughter.hp,
                "mp": daughter.mp,
                "mood": daughter.mood,
                "stress": daughter.stress,
                "fatigue": daughter.fatigue,
                "E": daughter.E,
                "I": daughter.I,
                "S": daughter.S,
                "N": daughter.N,
                "T": daughter.T,
                "F": daughter.F,
                "J": daughter.J,
                "P": daughter.P
            }
        }, f, ensure_ascii=False, indent=4)


def load_chat(log):
    """
    DB에 저장되어 있는 채팅 로그 불러와서 저장
    """
    current_dir = os.path.dirname(os.path.abspath(__file__))
    json_dir = os.path.join(current_dir, '..', '..', 'json', 'game')
    json_file = os.path.join(json_dir, 'conversation.json')

    # 기존 파일 삭제
    if os.path.exists(json_file):
        os.remove(json_file)

    with open(json_file, 'w', encoding='utf-8') as f:
        json.dump(log, f, ensure_ascii=False, indent=4)




def main():
    pid = get_pid_java()
    print("Received PID:", pid)

    if not pid:
        print(json.dumps({"error": "No PID provided or PID is invalid."}))
        return

    user_info = get_d_stats(pid)

    if user_info is None:
        print(json.dumps({"error": "Failed to retrieve user information from database."}))
        return

    daughter = Daughter(
        name=user_info.get("name"),
        age=user_info.get("age"),
        sex=user_info.get("sex"),
        mbti=user_info.get("mbti"),
        hp=user_info.get("hp"),
        mp=user_info.get("mp"),
        mood=user_info.get("mood"),
        stress=user_info.get("stress"),
        fatigue=user_info.get("fatigue"),
        E=user_info.get("e"),
        I=user_info.get("i"),
        S=user_info.get("s"),
        N=user_info.get("n"),
        T=user_info.get("t"),
        F=user_info.get("f"),
        J=user_info.get("j"),
        P=user_info.get("p")
    )

    log = get_chatLog(pid)
    if log is None:
        print(json.dumps({"error": "Failed to retrieve chat log from database."}))
        return

    load_to_json(daughter)
    load_chat(log)
    print(json.dumps({"success": "Daughter information saved to JSON file."}))

if __name__ == "__main__":
    main()
