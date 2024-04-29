import json
import os
import openai

# OpenAI API 키 설정
api_key = 'sk-proj-SrmvFmvip9TvsU8P9i3oT3BlbkFJ9dN0aAhwog6sSQ8aXL22'
openai.api_key = api_key

def save_conversation(messages):
    conversation_path = os.path.join("conversationData", "conversation.json")
    if os.path.exists(conversation_path):
        with open(conversation_path, 'r') as f:
            conversation_data = json.load(f)
    else:
        conversation_data = []

    conversation_data.extend(messages)

    with open(conversation_path, 'w') as f:
        json.dump(conversation_data, f, indent=4)

def make_child_status():
    communication_path = os.path.join("conversationData", "daughter_status.json")

    # daughter_status.json 파일이 이미 존재하는지 확인
    if os.path.exists(communication_path):
        delete_existing = input("daughter_status.json 파일이 이미 존재합니다. 기존 데이터를 유지하시겠습니까? (yes/no): ")
        
        if delete_existing.lower() == "no":
            os.remove(communication_path)  # 기존 파일 삭제
        else:
            print("기존 데이터를 유지합니다.")
            return

    # 파일이 존재하지 않거나 삭제를 선택한 경우에만 초기화 및 새로운 데이터 입력
    daughter_data = {"daughter": {}}

    while True:
        key = input("추가할 데이터의 키를 입력하세요: (close 입력시 종료)")
        if key.lower() == 'close': 
            break
        
        value = input(f"추가할 데이터의 '{key}' 값을 입력하세요: ")

        # 사용자가 입력한 key와 value를 daughter_data에 추가
        daughter_data["daughter"][key] = value

    # daughter_status.json 파일에 daughter_data 저장
    with open(communication_path, 'w') as f:
        json.dump(daughter_data, f, indent=4)

    print("daughter_status.json 파일이 생성되었습니다.")

def father_chat(messages, daughter_status):  
    father_status = {
        "father": {
            "name": "Lain",
            "age": 45,
            "mbti": "ENTJ",
            "blood": "RH B+",
            "mood": "HAPPY",
            "money": "MIDDLE",
            "job": "SCIENTIST"
        }
    }
    
    while True:
        father_prompt = input("아빠의 말을 입력하세요 (종료하려면 'q' 입력): ")
        if father_prompt.lower() == 'q':
            break
        messages.append({"role": "user", "content": f"{father_status['father']['name']} (Father): {father_prompt}"})
        response = openai.ChatCompletion.create(
            model='gpt-3.5-turbo',
            messages=messages,
            max_tokens=100,
            temperature=0.7
        )
        daughter_reply = response['choices'][0]['message']['content']
        print("딸의 대답:", daughter_reply)
        daughter_response_json = {
            "father_message": father_prompt,
            "daughter_reply": daughter_reply
        }
        print("JSON 출력:", json.dumps(daughter_response_json, indent=2))
        
        if 'daughter' in daughter_status and 'name' in daughter_status['daughter']:
            daughter_name = daughter_status['daughter']['name']
        else:
            daughter_name = "Daughter"
        
        messages.append({"role": "assistant", "content": f"{daughter_name}: {daughter_reply}"})

    # 대화 기록
    save_conversation(messages)

def main():
    make_child_status()
    
    daughter_status_path = os.path.join("conversationData", "daughter_status.json")
    
    if os.path.exists(daughter_status_path):
        with open(daughter_status_path, 'r') as f:
            daughter_status = json.load(f)
        
        messages = [
            {"role": "system", "content": "You are a helpful assistant."},
            {"role": "user", "content": f"Role-play a conversation between your father or user. Here's your status: {json.dumps(daughter_status, ensure_ascii=False)}. And also I want to get your key's and value's in last sentence"}
        ]
        father_chat(messages, daughter_status)  # daughter_status를 father_chat 함수에 전달
        print("전체 대화 히스토리:")
        for message in messages:
            print(f"{message['role']}: {message['content']}")
    else:
        print("대화를 진행할 수 없습니다. daughter_status.json 파일이 존재하지 않습니다.")

if __name__ == "__main__":
    main()
