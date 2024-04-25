import json
import os

def conversation_script():
    # 상대경로 안먹어서 일단 절대경로로
    file_path = "E:/prj/ConnectionGpt/conversationData/conversation.json"

    if os.path.exists(file_path):
        with open(file_path, 'r', encoding='utf-8') as file:
            conversation = json.load(file)
    else:
        conversation = []

    while True:
        question = input("사용자: ")

        #close면 대화 종료
        if question.lower() == "close":
            print("대화를 종료합니다.")
            break

        response = input("AI: ")

        # 대화 저장 data저장 방식 함더 고쳐봐야할듯
        message = {"data": {"question": question, "response": response}}
        conversation.append(message)

        with open(file_path, 'w', encoding='utf-8') as file:
            json.dump(conversation, file, ensure_ascii=False, indent=4)

# 마지막 데이터 읽기
def read_last_data(file_path):
    try:
        with open(file_path, 'r', encoding='utf-8') as file:
            data = json.load(file)
            
            # 마지막 데이터 읽어오기
            last_data = data[-1] if data else None
            
            return last_data
    except FileNotFoundError:
        print("파일 없음")
    
conversation_script()

last_data = read_last_data("E:/prj/ConnectionGpt/conversationData/conversation.json")

if last_data:
    print("lastdata:")
    print("ques = ", last_data["data"]["question"])
    print("resp = ", last_data["data"]["response"])
else:
    print("파일이 존재 하지 않거나, 데이터가 없음.")