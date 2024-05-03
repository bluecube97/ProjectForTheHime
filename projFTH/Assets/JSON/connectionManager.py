import openai
import json
import os
<<<<<<< Updated upstream:ConnectionGpt_LJ/connectionManager.py
=======
from statusManager import Daughter as d, load_daughter_status as lds
import sys

# 전체 대화 내용 저장용 리스트
conversation_ = []
>>>>>>> Stashed changes:projFTH/Assets/JSON/connectionManager.py

# OpenAI API 키 설정
api_key = 'sk-proj-3ZLbBHwylhtASxE4BIaMT3BlbkFJh6cUB6QhPVKBieezTqSg'
openai.api_key = api_key

def load_json_data(path):
    """ 파일 경로에서 JSON 데이터를 로드하고 문자열로 반환합니다. """
    try:
        with open(path, 'r', encoding='utf-8') as file:
            data = json.load(file)
        return json.dumps(data, ensure_ascii=False)
    except Exception as e:
        print(f"파일을 로드하거나 JSON 변환 중 오류가 발생했습니다: {e}")
        return None

def update_daughter_status(update_path, update_data):
    try:
        # 데이터를 JSON 파일로 저장
        with open(update_path, 'w', encoding='utf-8') as file:
            json.dump(update_data, file, indent=4)
        print("Data has been saved successfully to the update file.")
    except Exception as e:
        print(f"Data saving failed: {e}")

<<<<<<< Updated upstream:ConnectionGpt_LJ/connectionManager.py
def extract_and_save_updated_status(daughter_reply, update_path):
    if "```json" in daughter_reply:
        start_index = daughter_reply.find("```json") + len("```json")
        end_index = daughter_reply.find("```", start_index)
        json_str = daughter_reply[start_index:end_index].strip()

        try:
            update_data = json.loads(json_str)
            with open(update_path, 'w', encoding='utf-8') as file:
                json.dump(update_data, file, indent=4)
            print("Updated daughter status saved to JSON.")
            return True
        except json.JSONDecodeError:
            print("Failed to decode JSON from daughter's reply.")
            return False
    else:
        print("No JSON data found in daughter's reply.")
        return False

def ConnectionGpt(daughter_status_path, daughter_update_path):
=======
def get_origin_ment(daughter_reply) :
    if "GPT (Daughter): " in daughter_reply:
        start_gpt = daughter_reply.find("GPT (Daughter): ") + len("GPT (Daughter): ")
        end_gpt = daughter_reply.find("**", start_gpt)
        gpt_str = daughter_reply[start_gpt:end_gpt].strip()
    return gpt_str

def extract_and_save_updated_status(daughter_reply, d):

    # daughter_reply 문자열에서 "**change"가 있는지 확인.
    if "**" in daughter_reply:
        # "**change" 다음의 문자열을 찾아서 인덱스 설정
        start_index = daughter_reply.find("**") + len("**")
        # "**" 문자열을 찾아서 인덱스 설정
        end_index = daughter_reply.find("**", start_index)
        # 문자열을 추출하여 공백 제거
        stat_str = daughter_reply[start_index:end_index].strip()

        # "name"을 키워드로 사용하여 값을 추출
        name_start = stat_str.find('"name": "') + len('"name": "')
        name_end = stat_str.find('"', name_start + 1)
        name_value = stat_str[name_start:name_end]
        #d.name(name_value)
        d.name(name_value)
        
        # "age"을 키워드로 사용하여 값을 추출
        age_start = stat_str.find('"age": "') + len('"age": "')
        age_end = stat_str.find('"', age_start + 1)
        age_value = stat_str[age_start:age_end]
        d.age(age_value)

        # "sex"을 키워드로 사용하여 값을 추출
        sex_start = stat_str.find('"sex": "') + len('"sex": "')
        sex_end = stat_str.find('"', sex_start + 1)
        sex_value = stat_str[sex_start:sex_end]
        d.sex(sex_value)

        # "mbti"을 키워드로 사용하여 값을 추출
        mbti_start = stat_str.find('"mbti": "') + len('"mbti": "')
        mbti_end = stat_str.find('"', mbti_start + 1)
        mbti_value = stat_str[mbti_start:mbti_end]
        d.mbti(mbti_value)

        # "hp"을 키워드로 사용하여 값을 추출
        hp_start = stat_str.find('"hp": "') + len('"hp": "')
        hp_end = stat_str.find('"', hp_start + 1)
        hp_value = stat_str[hp_start:hp_end]
        d.hp(hp_value)

        # "mp"을 키워드로 사용하여 값을 추출
        mp_start = stat_str.find('"mp": "') + len('"mp": "')
        mp_end = stat_str.find('"', mp_start + 1)
        mp_value = stat_str[mp_start:mp_end]
        d.mp(mp_value)

        # "mood"을 키워드로 사용하여 값을 추출
        mood_start = stat_str.find('"mood": "') + len('"mood": "')
        mood_end = stat_str.find('"', mood_start + 1)
        mood_value = stat_str[mood_start:mood_end]
        d.mood(mood_value)

        # "stress"을 키워드로 사용하여 값을 추출
        stress_start = stat_str.find('"stress": "') + len('"stress": "')
        stress_end = stat_str.find('"', stress_start + 1)
        stress_value = stat_str[stress_start:stress_end]
        d.stress(stress_value)

        # "fatigue"을 키워드로 사용하여 값을 추출
        fatigue_start = stat_str.find('"fatigue": "') + len('"fatigue": "')
        fatigue_end = stat_str.find('"', fatigue_start + 1)
        fatigue_value = stat_str[fatigue_start:fatigue_end]
        d.fatigue(fatigue_value)

        # "E"을 키워드로 사용하여 값을 추출
        E_start = stat_str.find('"E": "') + len('"E": "')
        E_end = stat_str.find('"', E_start + 1)
        E_value = stat_str[E_start:E_end]
        d.E(E_value)

        # "I"을 키워드로 사용하여 값을 추출
        I_start = stat_str.find('"I": "') + len('"I": "')
        I_end = stat_str.find('"', I_start + 1)
        I_value = stat_str[I_start:I_end]
        d.I(I_value)

        # "S"을 키워드로 사용하여 값을 추출
        S_start = stat_str.find('"S": "') + len('"S": "')
        S_end = stat_str.find('"', S_start + 1)
        S_value = stat_str[S_start:S_end]
        d.S(S_value)

        # "N"을 키워드로 사용하여 값을 추출
        N_start = stat_str.find('"N": "') + len('"N": "')
        N_end = stat_str.find('"', N_start + 1)
        N_value = stat_str[N_start:N_end]
        d.N(N_value)

        # "T"을 키워드로 사용하여 값을 추출
        T_start = stat_str.find('"T": "') + len('"T": "')
        T_end = stat_str.find('"', T_start + 1)
        T_value = stat_str[T_start:T_end]
        d.T(T_value)

        # "F"을 키워드로 사용하여 값을 추출
        F_start = stat_str.find('"F": "') + len('"F": "')
        F_end = stat_str.find('"', F_start + 1)
        F_value = stat_str[F_start:F_end]
        d.F(F_value)

        # "J"을 키워드로 사용하여 값을 추출
        J_start = stat_str.find('"J": "') + len('"J": "')
        J_end = stat_str.find('"', J_start + 1)
        J_value = stat_str[J_start:J_end]
        d.J(J_value)

        # "P"을 키워드로 사용하여 값을 추출
        P_start = stat_str.find('"P": "') + len('"P": "')
        P_end = stat_str.find('"', P_start + 1)
        P_value = stat_str[P_start:P_end]
        d.P(P_value)
        
def get_ment_from_unity():
    try:
        user_ment = input()
        if user_ment.strip() == "":
            return None
        return user_ment
    except EOFError as e:
        print(json.dumps({"error": str(e)}))

def ConnectionGpt(daughter_status_path, daughter_update_path, d_stat, set_d):
>>>>>>> Stashed changes:projFTH/Assets/JSON/connectionManager.py
    father_status = {
        "father": {
            "name": "Lain",
            "age": 45,
            "mbti": "ENTJ",
            "blood": "RH B+",
            "mood": "HAPPY",
            "money": "MIDDLE",
            "job": "Teacher"
        }
    }
<<<<<<< Updated upstream:ConnectionGpt_LJ/connectionManager.py
    daughter_status_json = load_json_data(daughter_status_path)
=======

    #daughter_status_json = load_json_data(daughter_status_path)

    stat_json = {
    "daughter": {
        "name": "더조은",
        "age": 20,
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
>>>>>>> Stashed changes:projFTH/Assets/JSON/connectionManager.py

    set_text = (
            "You are Role-play a conversation between your father. "
            "The father act will be a user, and GPT you gonna act daughter. "
            "The conversation is exchanged with the user only once."
            "E is Extroversion, I is Introversion, S is Sensing, N is iNtuition, "
            "T is Thinking, F is Feeling, J is Judging, P is Perceiving. "
            "Their names are subtypes of MBTI, and MBTI changes according to the figures of this subtypes. "
            "The mood index is set in five levels: happiness, good, normal, sadness, and depression. "
            "The stress index is set in five levels: very high, high, normal, low, and very low."
            "The fatigue index is set in five levels: very refreshing, refreshing, normal, tired, and very tired."
            "Daughter's Stress and fatigue levels change according to the father's words, "
            "and the level of the MBTI subtype and the MBTI change according to the daughter's behavior. "
            "When a father has an out-of-context conversation or a story that has nothing to do with the content of the conversation, the daughter asks a reverse question or changes the subject."
            "E and I, S and N, T and F, J and P change their numbers according to their propensity at 100 each other."
            "The parameter numbers in the daughter_status change very minutely."
            "Daughter is a game character and based on her MBTI tendencies, if she's an extrovert, she likes hunting or outdoor activities,"
            "if she's introverted, she acts based on her tendencies, like being timid or doing things alone indoors."
            "Her stress, fatigue, and mood levels change because of conversations with her father or because of her daughter's tiredness from work or her stressful work. But not all conversations change, they change in meaningful conversations. "
            "Also fine-tune paramter's yourself. "
            "And at the end of my daughter's answer, please only give me the changed parameter values as JSON file."
            "If you make json file start with ```json and end to ```" # gpt가 json파일 방식으로 뱉도록
            f"Here's your status: {daughter_status_json}"
        )
    
    messages = [
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": set_text}
    ]

    while True:
        user_request = get_ment_from_unity()

        if user_request is None or user_request.lower() == 'close' :
            break

        messages.append({"role": "user", "content": f"{father_status['father']['name']} (Father): {user_request}"})
<<<<<<< Updated upstream:ConnectionGpt_LJ/connectionManager.py
        try:
            response = openai.ChatCompletion.create(
                model='gpt-3.5-turbo',
                messages=messages,
                max_tokens=300,
                temperature=0.5
            )
            daughter_reply = response['choices'][0]['message']['content']
            print(daughter_reply)
            messages.append({"role": "assistant", "content": f"Daughter: {daughter_reply}"})

            # Try to extract and save the updated status if present in the reply
            if extract_and_save_updated_status(daughter_reply, daughter_update_path):
                update_daughter_status(daughter_status_path, daughter_update_path)
        except Exception as e:
            print(f"An error occurred during API interaction: {e}")

def main():
    daughter_status_path = os.path.join("conversationData", "daughter_status.json")
=======
    
        response = openai.ChatCompletion.create(
            model='gpt-3.5-turbo',
            messages=messages,
            max_tokens=300,
            temperature=0.5
        )
        daughter_reply = response['choices'][0]['message']['content']
        #print(daughter_reply)
        messages.append({"role": "assistant", "content": f"Daughter: {daughter_reply}"})
        #----  실질적인 output 유니티로 전달
        ment = get_origin_ment(daughter_reply)
        json_response = json.dumps({"gpt_ment": ment}, ensure_ascii=False)
        print(json_response)

        #---
        # Try to extract and save the updated status if present in the reply
        # if extract_and_save_updated_status(daughter_reply, daughter_update_path, set_d):
        #     update_daughter_status(daughter_status_path, daughter_update_path)
        
        # 아빠와 딸의 대화를 read_comm_file에 있는 conversation.json에 저장
        read_comm_file(user_request, daughter_reply)

def main():
    d_stat = lds()
    set_d = d()
    dsp = r"conversationData\daughter_status.json"
    daughter_status_path = os.path.join(dsp)
>>>>>>> Stashed changes:projFTH/Assets/JSON/connectionManager.py
    daughter_update_path = os.path.join("conversationData", "updated_daughter_status.json")

    if os.path.exists(daughter_status_path):
       ConnectionGpt(daughter_status_path, daughter_update_path)
    else : 
        print("대화를 진행할 수 없습니다. daughter_status.json 파일이 존재하지 않습니다.")

if __name__ == "__main__":
    main()
