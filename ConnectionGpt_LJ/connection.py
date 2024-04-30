import json
import openai
import os

# 현재 스크립트의 경로
current_directory = os.path.dirname(__file__)

# conversation.json 상대경로
relative_path = "conversationData/conversation.json"
# 절대경로 변환
absolute_path = os.path.join(current_directory, relative_path)

# OpenAI API 키 설정
openai.api_key = "sk-proj-3ZLbBHwylhtASxE4BIaMT3BlbkFJh6cUB6QhPVKBieezTqSg"
# 이전 대화 결과 초기화
previous_completion = None

# 전체 대화 내용 저장용 리스트
conversation = []

# 현재 대화 내용 저장용 리스트 파일이 초기화 되도록 설계
communication_ = []

# communication.json 파일 읽고 덮어쓰기
def read_comm_file(question, response):
    commu = {"user_ment": question, "gpt_ment": response}
    communication_path = os.path.join(current_directory, "conversationData", "communication.json")

    # communication.json 파일을 저장할 폴더가 없을 경우 폴더를 생성합니다.
    if not os.path.exists(os.path.dirname(communication_path)):
        os.makedirs(os.path.dirname(communication_path))

    # 파일이 존재하지 않는 경우 새로운 파일을 생성하여 데이터를 저장
    if not os.path.exists(communication_path):
        communication_.append(commu)
        with open(communication_path, 'w') as f:
            json.dump(communication_, f, indent=4)
    else:
        # 파일이 존재하는 경우 기존 파일을 열어서 데이터를 읽고 덮어쓰기
        communication_.append(commu)
        with open(communication_path, 'r') as f:
            current_communication = json.load(f)
        current_communication.append(commu)
        with open(communication_path, 'w') as f:
            json.dump(current_communication, f, indent=4)

# gpt 대화
while (True):
    question = input("user: ")

    # 이전 대화 결과를 다음 대화의 입력으로 사용
    messages = [{"role": "user", "content": question}]
    if previous_completion:
        messages.append({"role": "assistant", "content": previous_completion.choices[0].message.content})

    # OpenAI API를 사용하여 대화 생성 요청 보내기
    completion = openai.ChatCompletion.create(
        model="gpt-3.5-turbo-1106",  # 사용할 모델
        messages=messages
    )
    response = completion.choices[0].message.content.strip()
    print("gpt:", response)

    previous_completion = completion

    # 대화 내용을 전체파일로 저장할때 사용 parameter 값 추가될 예정.
    message = {"data": 
               {"user_ment": question,
                 "gpt_ment": response}
                 }

    read_comm_file(question, response)
    
    conversation.append("{data}")
    conversation.append(message)

    # 대화 종료 이벤트
    if question.lower() == "close":
        break

# 전체 대화 내용 json파일 저장
with open(absolute_path, 'w', encoding='UTF-8') as file:
    json.dump(conversation, file, ensure_ascii=False, indent=4)

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