import openai
import json
import os
from statusManager import Daughter as d, load_daughter_status as lds
import sys

# 전체 대화 내용 저장용 리스트
conversation_ = []

# OpenAI API 키 설정
api_key = 'sk-proj-wgBv7WJ3g4GD4gpjZQdxT3BlbkFJ0R2Kyg0bNNp2bnF7SVcF'
openai.api_key = api_key

#딸과의 대화 json 파일에 저장
def read_comm_file(question, response):
    commu = {"user_ment": question, "gpt_ment": response}
    conversation_path = os.path.join("conversationData", "conversation.json")

    # conversation.json 파일을 저장할 폴더가 없을 경우 폴더를 생성합니다.
    if not os.path.exists(os.path.dirname(conversation_path)):
        os.makedirs(os.path.dirname(conversation_path))

    # 파일이 존재하지 않는 경우 새로운 파일을 생성하여 데이터를 저장
    if not os.path.exists(conversation_path):
        conversation_.append(commu)
        with open(conversation_path, 'w', encoding='utf-8') as f:
            json.dump(conversation_, f, indent=4, ensure_ascii=False)
    else:
        # 파일이 존재하는 경우 기존 파일을 열어서 데이터를 읽고 덮어쓰기
        conversation_.append(commu)
        with open(conversation_path, 'r', encoding='utf-8') as f:  
            current_conversation = json.load(f)  
        current_conversation.append(commu)
        with open(conversation_path, 'w', encoding='utf-8') as f: 
            json.dump(current_conversation, f, indent=4, ensure_ascii=False)

def load_json_data(path):
    """ 파일 경로에서 JSON 데이터를 로드하고 문자열로 반환합니다. """
    try:
        with open(path, 'r', encoding='utf-8') as file:
            data = json.load(file)
        return json.dumps(data, ensure_ascii=False)
    except Exception as e:
        print(f"파일을 로드하거나 JSON 변환 중 오류가 발생했습니다: {e}")
        return None


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

        stat_str_ = json.loads(stat_str)
        d.name = stat_str_["daughter"]["name"]
        d.sex = stat_str_["daughter"]["sex"]
        d.age = stat_str_["daughter"]["age"]
        d.mbti = stat_str_["daughter"]["mbti"]
        d.hp = stat_str_["daughter"]["hp"]
        d.mp = stat_str_["daughter"]["mp"]
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

def get_ment_from_unity():
    try:
        user_ment = sys.stdin.readline().strip() #input말고 파이썬 모듈 sys의 내부 함수를 사용해 표준입력으로 한줄만 읽음.
        if user_ment == "END_OF_INPUT":
            return None
        if user_ment:
            return user_ment
        else:
            return None
    except EOFError as e:
        print(json.dumps({"error": str(e)}))

def ConnectionGpt(d_stat, set_d):
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

    stat_json = {
    "daughter": {
        "name": "더조은",
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

    set_text = (
                "1. You are role-playing a conversation with your father."
                "2. Be sure to answer according to the rules below."
                "3. You must always have to answer in Korean and you have to answer everything I say."
                "4. The daughter's answer is unconditionally 'GPT (Daughter): ' Please tell me through the form."
                "5. The father will act as the user, and GPT will act as the daughter."
                "	1)Conversation with the user occurs only once."
                "	2)E is extroversion, I is introversion, S is sensing, N is intuition"
                "	3) T is thought, F is emotion, J is judgment, and P is perception."
                "	4)Their names are criterions of MBTI, and the MBTI changes depending on the readings of this criterions."
                "	5)The mood index is set to five levels: happiness, good, normal, sadness, and depression."
                "	6)The stress index is set in five levels: very high, high, normal, low, and very low."
                "	7)The fatigue index is set to five levels: very refreshed, refreshed, normal, tired, and very tired."
                "	8)Depending on what the father says, the daughter’s stress and fatigue level changes."
                "	8-1)If you get angry, swear, or verbally abuse someone, your stress and fatigue will increase."
                "	8-2)If you give compliments, nice words, and gifts, stress and fatigue will decrease."
                "	8-3)Please refer to the initial value from the next line"
                    f"Your name is {d_stat.name,}"
                    f"Your age is {d_stat.age},"
                    f"Your sex is {d_stat.sex},"
                    f"Your MBIT is {d_stat.mbti},"
                    f"Your HP is {d_stat.hp},"
                    f"Your MP is {d_stat.mp},"
                    f"Your Mood is {d_stat.mood},"
                    f"Your Stress is {d_stat.stress},"
                    f"Your Fatigue is {d_stat.fatigue},"
                    f"Your MBTI(E) is {d_stat.E},"
                    f"Your MBTI(I) is {d_stat.I},"
                    f"Your MBTI(S) is {d_stat.S},"
                    f"Your MBTI(N) is {d_stat.N},"
                    f"Your MBTI(T) is {d_stat.T},"
                    f"Your MBTI(F) is {d_stat.F},"
                    f"Your MBTI(J) is {d_stat.J},"
                    f"Your MBTI(P) is {d_stat.P}"
                "   8-4)When changing values, please change daughter_status_json file from the initial value."
                "	9)And depending on your daughter’s behavior, her MBTI criterion will vary."
                "	10)If the father talks out of context or says something completely unrelated to the conversation, the daughter asks a counter question or changes the topic."
                "	11)E and I, S and N, T and F, J and P have different numbers out of 100 depending on their tendencies."
                "	12)The parameter number of daughter_status changes very subtly."
                "	13)My daughter is a game character and if she has an extroverted personality (it means MbTI criterion is E), she likes hunting and outdoor activities."
                "	14)However, if she has an introverted personality (it means MbTI criterion is I), she acts timid or doing something alone indoors."
                "	15)Her stress, fatigue, and mood levels change due to conversations with her father, or daughter's fatigue at work, or something stressful. It doesn't change in every conversation, it only changes in meaningful conversations."
                "   And at the end of your daughter’s answer, be sure to give me the whole parameter values like next line. "                    
                f"  {stat_json} Please change all single quotes in all parameter values here to double quotes. "
                "	17) When providing the parameter, use the format starts with '**' and ends with '**'."
                #f"{daughter_status_json}"
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
        try:
            response = openai.ChatCompletion.create(
                model='gpt-3.5-turbo',
                messages=messages,
                max_tokens=300,
                temperature=0.5
            )
            daughter_reply = response['choices'][0]['message']['content']
            messages.append({"role": "assistant", "content": f"{daughter_reply}"})
            #----  실질적인 output 유니티로 전달
            
            #print(daughter_reply)
            ment_ = get_origin_ment(daughter_reply)
            if ment_ is not None:
                extract_and_save_updated_status(daughter_reply, set_d)
                json_response = json.dumps({"gpt_ment" : ment_}, ensure_ascii=False)
                print(json_response)
                # response_data = json.dumps({"gpt_ment": "안녕하세요 아빠. 어떻게 지내세요?"}, ensure_ascii=False)
                # print(json.dumps(response_data))
            else:
                print("No valid response to process.")
        
        except Exception as e:
            print("error : ", e)
        # 아빠와 딸의 대화를 read_comm_file에 있는 conversation.json에 저장
        read_comm_file(user_request, daughter_reply)

def main():
    d_stat = lds()
    set_d = d()

    ConnectionGpt(d_stat, set_d)

if __name__ == "__main__":
    main()
