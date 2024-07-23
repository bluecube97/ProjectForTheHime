# -*- coding: utf-8 -*-
import random

import openai
import json
import os
from statusVO import Daughter as d, load_daughter_status as lds
import sys

# 전체 대화 내용 저장용 리스트 테스트
conversation_ = []

# OpenAI API 키 설정
api_key = ""
openai.api_key = api_key


# user_ment = ""

# 딸과의 대화 json 파일에 저장
def read_comm_file(question, response):
    commu = {"user_ment": question, "gpt_ment": response}
    current_dir = os.path.dirname(os.path.abspath(__file__))
    relative_path = os.path.join(current_dir, '..', '..', 'json', 'game', 'conversation.json')
    conversation_path = relative_path

    if not os.path.exists(os.path.dirname(conversation_path)):
        os.makedirs(os.path.dirname(conversation_path))

    if not os.path.exists(conversation_path):
        conversation_.append(commu)
        with open(conversation_path, 'w', encoding='utf-8') as f:
            json.dump(conversation_, f, indent=4, ensure_ascii=False)
    else:
        conversation_.append(commu)
        with open(conversation_path, 'r', encoding='utf-8') as f:
            current_conversation = json.load(f)
        current_conversation.append(commu)
        with open(conversation_path, 'w', encoding='utf-8') as f:
            json.dump(current_conversation, f, indent=4, ensure_ascii=False)


# gpt응답에서 순수 대답 분리.
def get_origin_ment(daughter_reply):
    gpt_str = None  # gpt_str에 기본값 할당
    if "GPT (Daughter): " or "GPT (딸): " in daughter_reply:
        start_gpt = daughter_reply.find("GPT (Daughter): ") + len("GPT (Daughter): ")
        end_gpt = daughter_reply.find("**", start_gpt)
        gpt_str = daughter_reply[start_gpt:end_gpt].strip()
    return gpt_str


# gpt응답에서 변경 스테이터스 분리.
def extract_and_save_updated_status(daughter_reply, d):
    # daughter_reply 문자열에서 "**change"가 있는지 확인 하고 문자열 공백 제거 및 값 json파일 및 VO객체에 저장.
    if "**" in daughter_reply:
        start_index = daughter_reply.find("**") + len("**")
        end_index = daughter_reply.find("**", start_index)
        stat_str = daughter_reply[start_index:end_index].strip()

        stat_str_ = json.loads(stat_str)

        d.name = stat_str_["daughter"]["name"]
        d.age = stat_str_["daughter"]["age"]
        d.sex = stat_str_["daughter"]["sex"]
        d.mbti = stat_str_["daughter"]["mbti"]
        d.hp = stat_str_["daughter"]["hp"]
        d.mp = stat_str_["daughter"]["mp"]
        d.mood = stat_str_["daughter"]["mood"]
        d.stress = stat_str_["daughter"]["stress"]
        d.fatigue = stat_str_["daughter"]["fatigue"]
        d.E = stat_str_["daughter"]["E"]
        d.I = stat_str_["daughter"]["I"]
        d.S = stat_str_["daughter"]["S"]
        d.N = stat_str_["daughter"]["N"]
        d.T = stat_str_["daughter"]["T"]
        d.F = stat_str_["daughter"]["F"]
        d.J = stat_str_["daughter"]["J"]
        d.P = stat_str_["daughter"]["P"]

        present_status = {
            "daughter": {
                "name": d.name,
                "age": d.age,
                "sex": d.sex,
                "mbti": d.mbti,
                "hp": d.hp,
                "mp": d.mp,
                "mood": d.mood,
                "stress": d.stress,
                "fatigue": d.fatigue,
                "E": d.E,
                "I": d.I,
                "S": d.S,
                "N": d.N,
                "T": d.T,
                "F": d.F,
                "J": d.J,
                "P": d.P
            }
        }

        current_dir = os.path.dirname(os.path.abspath(__file__))
        relative_path = os.path.join(current_dir, '..', '..', 'json', 'game', 'statusRecord.json')
        update_path = relative_path

        with open(update_path, 'w', encoding='utf-8') as f:
            json.dump(present_status, f, indent=4, ensure_ascii=False)


# 부모 정보 받아오기.
def get_parent_status():
    try:
        current_dir = os.path.dirname(os.path.abspath(__file__))
        relative_path = os.path.join(current_dir, '..', '..', 'json', 'game', 'parent_status.json')
        parent_status_path = relative_path

        if os.path.exists(parent_status_path):
            with open(parent_status_path, 'r', encoding='utf-8') as f:
                parent_status = json.load(f)
                # print(parent_status) 디버그용
                return parent_status
        else:
            print("No Parent_Status.json file check the path or GameScene")
    except EOFError as e:
        print(json.dumps({"error": str(e)}))


# java 에서 입력 프롬프트 받아오기.
def get_ment_from_unity():
    try:
        # user_ment = sys.stdin.readline().strip() # input 말고 파이썬 모듈 sys 의 내부 함수를 사용해 표준 입력 으로 한줄만 읽음.
        user_ment = sys.argv[1]
        if user_ment == "END_OF_INPUT":
            return None
        if user_ment:
            return user_ment
        else:
            return None
    except EOFError as e:
        print(json.dumps({"error": str(e)}))


# gpt와 실질적인 연결.
def ConnectionGpt(d_stat, set_d):
    userInfo = get_parent_status()
    username = userInfo['username']
    usersex = userInfo['usersex']

    parent_status = {
        "parent": {
            "name": username,
            "sex": usersex
        }
    }

    stat_json = {
        "daughter": {
            "name": "name",
            "age": "20",
            "sex": "female",
            "mbti": "ISFJ",
            "hp": 70,
            "mp": 80,
            "mood": "happiness",
            "stress": "high",
            "fatigue": "tired",
            "E": 30,
            "I": 70,
            "S": 65,
            "N": 35,
            "T": 20,
            "F": 80,
            "J": 85,
            "P": 15
        }
    }

    # gpt 설정 추가.
    set_text = (
            "1. You are role-playing a conversation with your parent."
            "	1-1)If User's sex is male Gpt you call user by dad"
            "	1-2)Else if User's sex is female Gpt you call user by mom"
            "Your parent, in other words user's name is {}.".format(username) +
            "Your parent, in other words user's sex is {}.".format(usersex) +
            "2. Be sure to answer according to the rules below."
            "3. You must always have to answer in Korean and you have to answer everything I say."
            "4. The daughter's answer is unconditionally 'GPT (Daughter): ' Please tell me through the form."
            "5. The parent will act as the user, and GPT will act as the daughter."
            "	1)Conversation with the user occurs only once."
            "	2)E is extroversion, I is introversion, S is sensing, N is intuition"
            "	3) T is thought, F is emotion, J is judgment, and P is perception."
            "	4)Their names are criterions of MBTI, and the MBTI changes depending on the readings of this criterions."
            "	5)The mood index is set to five levels: happiness, good, normal, sadness, and depression."
            "	6)The stress index is set in five levels: very high, high, normal, low, and very low."
            "	7)The fatigue index is set to five levels: very refreshed, refreshed, normal, tired, and very tired."
            "	8)Depending on what the parent says, the daughter’s stress and fatigue level changes."
            "	8-1)If you get angry, swear, or verbally abuse someone, your stress and fatigue will increase."
            "	8-2)If you give compliments, nice words, and gifts, stress and fatigue will decrease."
            "	8-3)Please refer to the initial value from the next line" +
            "Your name is {},".format(d_stat.name) +
            "Your age is {},".format(d_stat.age) +
            "Your sex is {},".format(d_stat.sex) +
            "Your MBIT is {},".format(d_stat.mbti) +
            "Your HP is {},".format(d_stat.hp) +
            "Your MP is {},".format(d_stat.mp) +
            "Your Mood is {},".format(d_stat.mood) +
            "Your Stress is {},".format(d_stat.stress) +
            "Your Fatigue is {},".format(d_stat.fatigue) +
            "Your MBTI(E) is {},".format(d_stat.E) +
            "Your MBTI(I) is {},".format(d_stat.I) +
            "Your MBTI(S) is {},".format(d_stat.S) +
            "Your MBTI(N) is {},".format(d_stat.N) +
            "Your MBTI(T) is {},".format(d_stat.T) +
            "Your MBTI(F) is {},".format(d_stat.F) +
            "Your MBTI(J) is {},".format(d_stat.J) +
            "Your MBTI(P) is {},".format(d_stat.P) +
            "   8-4)When changing values, please change daughter_status_json file from the initial value."
            "	9)And depending on your daughter’s behavior, her MBTI criterion will vary."
            "	10)If the parent talks out of context or says something completely unrelated to the conversation, the daughter asks a counter question or changes the topic."
            "	11)E and I, S and N, T and F, J and P have different numbers out of 100 depending on their tendencies."
            "	12)The parameter number of daughter_status changes very subtly."
            "	13)My daughter is a game character and if she has an extroverted personality (it means MbTI criterion is E), she likes hunting and outdoor activities."
            "	14)However, if she has an introverted personality (it means MbTI criterion is I), she acts timid or doing something alone indoors."
            "	15)Her stress, fatigue, and mood levels change due to conversations with her parent, or daughter's fatigue at work, or something stressful. It doesn't change in every conversation, it only changes in meaningful conversations."
            "   And at the end of your daughter’s answer, be sure to give me the whole parameter values like next line. " +
            "  %s Please change all single quotes in all parameter values here to double quotes. " % stat_json +
            "	17) When providing the parameter, use the format starts with '**' and ends with '**'."
    )

    messages = [
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": set_text}
    ]

    while True:
        user_request = get_ment_from_unity()

        if user_request is None or user_request.lower() == 'close':
            break

        messages.append(
            {"role": "user", "content": "{} (parent): {}".format(parent_status['parent']['name'], user_request)})
        try:
            response = openai.ChatCompletion.create(
                model='gpt-4o-mini',
                messages=messages,
                max_tokens=300,  # gpt response의 최대값 갯수.
                temperature=0.6
            )
            daughter_reply = response['choices'][0]['message']['content']
            messages.append({"role": "assistant", "content": "{}".format(daughter_reply)})

            # ----  실질적인 output 출력, 이후 출력을 자바에서 읽음 ----
            ment_ = get_origin_ment(daughter_reply)
            if ment_ is not None:
                extract_and_save_updated_status(daughter_reply, set_d)
                json_response = json.dumps({"gpt_ment": ment_}, ensure_ascii=False)
                # print(json_response)
                print(ment_)
                # print(user_request)
            else:
                # 응답이 null이면 저장된 대화 출력.
                error_response()
            # ---------------------------------------

        except Exception as e:
            print("error : ", e)
        # 아빠와 딸의 대화를 read_comm_file에 있는 conversation.json에 저장
        read_comm_file(user_request, ment_)
        break


def error_response():
    case = random.randint(0, 5)

    if case == 0:
        print("죄송해요. 다시 말해주실수 있나요?")
    elif case == 1:
        print("다시 한번 말해주세요.")
    elif case == 2:
        print("다시 말해주시겠어요?")
    elif case == 3:
        print("죄송해요. 다시 말해주시겠어요?")
    elif case == 4:
        print("다시 한번 말해주시겠어요?")
    elif case == 5:
        print("죄송해요. 다시 말해주시겠어요?")
    else:
        print("다시 한번 말해주세요.")


def main():
    d_stat = lds()
    set_d = d()

    ConnectionGpt(d_stat, set_d)


if __name__ == "__main__":
    main()
    sys.stdout.flush()
