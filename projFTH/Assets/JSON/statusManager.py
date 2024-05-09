import json
import os

def get_present_status():
    base_dir = os.path.dirname(os.path.abspath(__file__))
    status_record_path = os.path.join(base_dir, "conversationData", "statusRecord.json")

    try:
        if status_record_path is not os.path.exists :
            with open(status_record_path, 'r', encoding='utf-8') as f:
                status = json.load(f)
                present_status = status[-1] #가장 최근의 스테이터스 불러오기
                print(present_status)
        else : 
            print("스테이터스가 업데이트된 내역이 없습니다.")
    except Exception as e:
        print(f"Error to load statusRecord.json. ErrorLog : ", e)
        print(None)

def main():
    get_present_status()

if __name__ == "__main__":
    main()