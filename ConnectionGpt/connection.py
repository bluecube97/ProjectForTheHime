import json
import openai
import os

# OpenAI API 키 설정
openai.api_key = "sk-proj-3ZLbBHwylhtASxE4BIaMT3BlbkFJh6cUB6QhPVKBieezTqSg"

# communication.json 파일의 상대경로 설정
current_directory = os.path.dirname(__file__)
relative_path = "conversationData/communication.json"
communication_path = os.path.join(current_directory, relative_path)

# communication.json 파일 초기화 및 데이터 적재 함수
def read_comm_file(question, response):
    # communication.json 파일이 존재하지 않을 경우 초기화
    if not os.path.exists(communication_path):
        communication_data = []
    else:
        # 파일이 존재할 경우 기존 데이터 로드
        with open(communication_path, 'r') as f:
            communication_data = json.load(f)

    # 새로운 대화 데이터 적재
    new_entry = {"user_ment": question, "gpt_ment": response}
    communication_data.append(new_entry)

    # communication.json 파일에 데이터 적재
    with open(communication_path, 'w') as f:
        json.dump(communication_data, f, ensure_ascii=False, indent=4)

# 전체 대화 데이터 저장용 리스트
data = []

# 사용자와 GPT 대화 반복
while True:
    user_ment = input("사용자 질문을 입력하세요 (종료하려면 'close' 입력): ")
    if user_ment.lower() == "close":
        break
    gpt_ment = input("GPT 대답을 입력하세요: ")

    # 사용자 질문과 GPT 대답을 data 형식으로 구성하여 data 리스트에 추가
    message = {
        "user_ment": user_ment,
        "gpt_ment": gpt_ment
    }
    data.append(message)

    # communication.json 파일에 데이터 적재
    read_comm_file(user_ment, gpt_ment)

# 데이터가 쌓인 후에는 "data" 키를 가진 딕셔너리로 구성
data_dict = {"data": data}

# conversation.json 파일에 데이터 저장
file_path = os.path.join("conversationData", "conversation.json")
with open(file_path, 'w', encoding='UTF-8') as file:
    json.dump(data_dict, file, ensure_ascii=False, indent=4)

print("대화 데이터가 저장되었습니다.")

# 1. parameter값이 전체적으로 들어가는 : Parameter.json
# 2. ques 와 resp만 들어가는 : Conversation.json


# 전체적인 로직
#     1. 사용자가 프롬프트 입력
#     2. 프롬프트 값 json에 저장 -> 유니티에서
#     3. 파이썬에서 json파일 읽어서 유저가 입력한 프롬프트 값의 key값을 읽음
#     4. 읽은 key값을 변숮에 저장 후 날림
#     5. 날아 올때 모든 파라미터 값의 변경과, 대답을 가져옴
#     6. 데이터 정재
#         6.1 날아온 데이터에서 파라미터 값을 제외한 답안을 별도의 json파일에 저장
#         6.2 먼저 날아온 question을 별도의 json에 넣고 response만 다시 별도의 json에 넣어서
#         6.3 유니티로 반환
#     유니티에서 게임이 끝나거나 게임을 저장 혹은 분기가 끝났을 때 모든 프롬프트 값이 담긴 json 파일을 유니티로 넘겨줌
    # 유니티에서 db관리 
    # 게임이 재 시작 하면, 전체적인 파라미터 및 데이터가 담긴 json파일을 넘겨주는 메서드를 시작.


# 만들 메서드 목록
     # parameter 읽고 쓰기 함수, conversation 읽고 쓰기, 데이터 정재 함수