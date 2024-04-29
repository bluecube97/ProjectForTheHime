import json
import os

# 현재 스크립트의 경로
current_directory = os.path.dirname(__file__)

# conversation.json 상대경로
relative_path = "conversationData/conversation.json"
# 절대경로 변환
absolute_path = os.path.join(current_directory, relative_path)

def conversation_script():


    if os.path.exists(relative_path):
        with open(relative_path, 'r', encoding='utf-8') as file:
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

        with open(relative_path, 'w', encoding='utf-8') as file:
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