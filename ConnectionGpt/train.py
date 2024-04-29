import json
import openai
import os
import time
import random

# OpenAI API 키 설정
api_key = 'sk-proj-SrmvFmvip9TvsU8P9i3oT3BlbkFJ9dN0aAhwog6sSQ8aXL22'
openai.api_key = api_key

# 대화 기록을 저장할 경로
conversation_path = os.path.join("conversationData", "conversation.json")

def save_conversation(data):
    """대화 내용을 JSON 파일에 저장합니다."""
    if os.path.exists(conversation_path):
        with open(conversation_path, 'r', encoding='utf-8') as f:  # 파일을 읽을 때 UTF-8 인코딩 사용
            conversation_data = json.load(f)
    else:
        conversation_data = []

    conversation_data.append(data)

    with open(conversation_path, 'w', encoding='utf-8') as f:  # 파일을 쓸 때 UTF-8 인코딩 사용
        json.dump(conversation_data, f, indent=4, ensure_ascii=False)  # ensure_ascii=False로 설정하여 한글이 유니코드로 저장되지 않도록 함

def save_communication(data):
    """대화 내용을 JSON 파일에 저장합니다."""
    communication_path = os.path.join("conversationData", "communication.json")
    if os.path.exists(communication_path):
        with open(communication_path, 'r', encoding='utf-8') as f:
            communication_data = json.load(f)
    else:
        communication_data = []

    communication_data.append(data)

    with open(communication_path, 'w', encoding='utf-8') as f:
        json.dump(communication_data, f, ensure_ascii=False, indent=4)

def train_ai(daughter_status, user_ment, dad_ment):
    """AI를 훈련하고 응답을 반환합니다."""
    # set_text 생성
    set_text = (
        "You are Role-play a conversation between your father. "
        "The father act will be a user, and GPT you gonna act daughter. "
        "E is Extroversion, I is Introversion, S is Sensing, N is iNtuition, "
        "T is Thinking, F is Feeling, J is Judging, P is Perceiving. "
        "Their names are subtypes of MBTI, and MBTI changes according to the figures of this subtypes. "
        "The mood index is set in five levels: happiness, good, normal, sadness, and depression. "
        "The stress index is set in five levels: very high, high, moderate, low, and very low."
        "The fatigue index is set in five levels: very tired, tired, normal, refreshing, and very refreshing."
        "Daughter's Stress and fatigue levels change according to the father's words, "
        "and the level of the MBTI subtype and the MBTI change according to the daughter's behavior. "
        "E and I, S and N, T and F, J and P change their numbers according to their propensity at 100 each other."
        "The parameter numbers in the daughter_status change very minutely."
        "Daughter is a game character and based on her MBTI tendencies, if she's an extrovert, she likes hunting or outdoor activities,"
        "if she's introverted, she acts based on her tendencies, like being timid or doing things alone indoors."
        "And also I want to get your key's and value's except name, age, sex keys in last sentence. "
        "Also fine-tune paramter's yourself. "
        "And i want to know the parameter what changed. "
        "Here's your status: "
        f"{json.dumps(daughter_status, ensure_ascii=False)}."
    )

    # 대화 메시지 생성
    messages = [
        {"role": "system", "content": "도움이 필요하신가요?"},
        {"role": "user", "content": set_text},
        {"role": "user", "content": user_ment},  # 사용자 입력 추가
        {"role": "assistant", "content": dad_ment}  # 아빠의 발언 추가
    ]

    # AI 훈련
    response = openai.ChatCompletion.create(
        model='gpt-3.5-turbo',
        messages=messages,
        max_tokens=1000,
        temperature=0.7
    )

    # 결과 출력
    daughter_reply = response['choices'][0]['message']['content']
    print("AI 대답:", daughter_reply)

    # 대화 기록
    save_conversation({"role": "user", "content": user_ment})
    save_conversation({"role": "assistant", "content": dad_ment})
    save_conversation({"role": "daughter", "content": daughter_reply})

    return daughter_reply

def update_status(daughter_status, daughter_reply):
    """대화 응답에 따라 상태를 업데이트합니다."""
    # 예시로서 각 상태를 변경하는 로직을 여기에 추가할 수 있습니다.
    daughter_status["hp"] = "100"
    daughter_status["mp"] = "100"
    daughter_status["E"] = "40%"
    daughter_status["I"] = "60%"
    daughter_status["S"] = "70%"
    daughter_status["N"] = "30%"
    daughter_status["T"] = "50%"
    daughter_status["F"] = "60%"
    daughter_status["J"] = "40%"
    daughter_status["P"] = "50%"
    daughter_status["MBTI"] = "INFP"
    daughter_status["mood"] = "depression"
    daughter_status["fatigue"] = "100%"
    daughter_status["stress"] = "100%"

    # fatigue 및 stress 값을 100 사이로 제한
    fatigue_percentage = float(daughter_status["fatigue"].rstrip("%"))
    stress_percentage = float(daughter_status["stress"].rstrip("%"))
    fatigue_percentage = max(min(fatigue_percentage, 100), 0)
    stress_percentage = max(min(stress_percentage, 100), 0)

    # fatigue 및 stress 값을 daughter_status에 업데이트
    daughter_status["fatigue"] = f"{fatigue_percentage:.1f}%"
    daughter_status["stress"] = f"{stress_percentage:.1f}%"

    # MBTI를 daughter_reply에 따라 업데이트
    if "나쁜말" in daughter_reply:
        daughter_status["I"] = f"{int(daughter_status['I'].rstrip('%')) + 2}%"
    elif "반항" in daughter_reply:
        daughter_status["I"] = f"{int(daughter_status['I'].rstrip('%')) + 1}%"
    else:
        daughter_status["E"] = f"{int(daughter_status['E'].rstrip('%')) + 1}%"

    # moral_evaluation 값도 업데이트
    if "좋은말" in daughter_reply:
        daughter_status["moral_evaluation"] = max(min(float(daughter_status["moral_evaluation"]) - 1, 100), 0)
    else:
        daughter_status["moral_evaluation"] = max(min(float(daughter_status["moral_evaluation"]) + 1, 100), 0)

    # daughter_status를 JSON 파일에 다시 저장
    daughter_status_path = os.path.join("conversationData", "daughter_status.json")
    with open(daughter_status_path, 'w', encoding='utf-8') as f:
        json.dump(daughter_status, f, ensure_ascii=False, indent=4)

# main 함수에 save_communication 호출 추가
def main():
    # daughter_status.json 파일 경로
    daughter_status_path = os.path.join("conversationData", "daughter_status.json")
    
    if os.path.exists(daughter_status_path):
        with open(daughter_status_path, 'r', encoding='utf-8') as f:
            daughter_status = json.load(f)
        
        # 대화 횟수 설정
        conversation_count = 4
        for _ in range(conversation_count):
            # 랜덤한 사용자 입력 생성 (나쁜말, 반항, 좋은말 랜덤으로)
            user_ment = random.choice(["좋은 아침", "짜증나", "나는 너를 싫어해!", "너는 정말 착해", "시키면 좀해", "씨발"])
            # 랜덤한 아빠의 발언 생성
            dad_ment = random.choice(["어떻게 지내니?", "오늘 날씨가 좋아 보이는구나.", "이야기를 해보자.", "짜증나는 일이 있었니?", "내가 원하는 대로 행동하지 못했구나. 미안하다."])
            
            # AI 훈련
            daughter_reply = train_ai(daughter_status, user_ment, dad_ment)

            # 대화 내용 저장
            save_communication({"role": "daughter", "content": daughter_reply})

            # 상태 업데이트
            update_status(daughter_status, daughter_reply)

            # 상태 출력
            print("Updated Daughter Status:")
            print(json.dumps(daughter_status, indent=4))

            # 훈련 결과를 파일에 저장
            with open('trained_ai_response.txt', 'w', encoding='utf-8') as file:
                file.write(daughter_reply)
    else:
        print("대화를 진행할 수 없습니다. daughter_status.json 파일이 존재하지 않습니다.")

# main 함수 호출
if __name__ == "__main__":
    main()
