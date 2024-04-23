import json
import openai
import mysql.connector

# OpenAI API 키 설정 공용으로 바뀌면 바뀔듯?
openai.api_key = "sk-proj-3ZLbBHwylhtASxE4BIaMT3BlbkFJh6cUB6QhPVKBieezTqSg"
# 이전 대화 결과 초기화
previous_completion = None
#대화 내용 저장용 리스트
conversation = []

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
with open('C:\\DevTool\\workspace\\UnityPJ\\ProjectForTheHime\\conversation.json', 'w', encoding='UTF-8') as file:
    json.dump(conversation, file, ensure_ascii=False, indent=4)

#sql connection out
if connection.is_connected():
    cur.close()
    connection.close()