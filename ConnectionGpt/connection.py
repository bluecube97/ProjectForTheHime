import json
import openai
import mysql.connector

# OpenAI API 키 설정 공용으로 바뀌면 바뀔듯?
openai.api_key = "sk-proj-3ZLbBHwylhtASxE4BIaMT3BlbkFJh6cUB6QhPVKBieezTqSg"
# 이전 대화 결과 초기화
previous_completion = None

#전체 대화 내용 저장용 리스트
conversation = []
#현재 대화 내용 저장용 리스트 json파일이 덮어씌워지도록.
current_conversation = []

#sql connection
connection = mysql.connector.connect(
    host="localhost",
    user="root",
    password="1111",
    database="testdb"
)

#gpt connection
while (True):
    question = input("user: ")

    # 이전 대화 결과를 다음 대화의 입력으로 사용
    messages = [{"role": "user", "content": question}]
    if previous_completion:
        messages.append({"role": "assistant", "content": previous_completion.choices[0].message.content})

    # OpenAI API를 사용하여 대화 생성 요청 보내기
    completion = openai.ChatCompletion.create(
        model="gpt-3.5-turbo-1106",  # 사용할 모델 설정 openai에서 버전이 바뀌면 업데이트 해야함
        messages=messages
    )
    response = completion.choices[0].message.content.strip()
    print("gpt:", response)

    previous_completion = completion

    #대화 내용을 dict형태로 저장
    message = {"user_ment": question, 
                "gpt_ment": response}
    
    #저장한 dict을 리스트에 넣음
    conversation.append(message)

    
    try:
        cur = connection.cursor()

        sql = "INSERT INTO tbl_gpt (gpt_ment, user_ment) VALUES (%s, %s)"
        values = (response, question)

        cur.execute(sql, values)

        connection.commit()
        print("data_insert_success")

    except mysql.connector.Error as e:
        print("Error:", e)

    # 대화 종료 이벤트
    if question.lower() == "close":
        break

# 절대경로로 json파일 경로 저장 
# 이쪽은 root지정해주는거 찾아보기
with open('C:\\DevTool\\workspace\\UnityPJ\\ProjectForTheHime\\conversationData\\conversation.json', 'w', encoding='UTF-8') as file:
    json.dump(conversation, file, ensure_ascii=False, indent=4)

#sql connection out
if connection.is_connected():
    cur.close()
    connection.close()


# db커넥션 필요 없이 json으로만 폴더 남기면될듯 함.
# 전체적인 대화와 스테이터스를 저장할 리스트와 현재의 대화를 저장 할 리스트를 따로 만들어서
    #현재의 대화json파일을 계속 파싱하는걸로.
    #gpt에게 기본 응답을 스테이터스도 같이 뱉도록하고, 나온 값을 split하여 값을 저장 하도록 함.

# current_conversation.json파일 읽고 덮어쓰기
def readCurConv(question, response):
    file_path = 'C:\\DevTool\\workspace\\UnityPJ\\ProjectForTheHime\\conversationData\\current_conversation.json'

    # 파일이 존재하지 않는 경우 새로운 파일을 생성하여 데이터를 저장
    if not os.path.exists(file_path):
        cur_data = {
            'user_ment': question,
            'gpt_ment': response
        }
        with open(file_path, 'w') as f:
            json.dump(cur_data, f)
    else:
        # 파일이 존재하는 경우 기존 파일을 열어서 데이터를 읽고 덮어쓰기
        with open(file_path, 'r') as f:
            cur_data = json.load(f)

        cur_data['user_ment'] = question
        cur_data['gpt_ment'] = response

        with open(file_path, 'w') as f:
            json.dump(cur_data, f)


# db 커넥션빼기, 지피티 연결, 클래스 분리, 스크립트 정리.