import openai
import json
import os

# OpenAI API 키 설정
api_key = 'sk-proj-3ZLbBHwylhtASxE4BIaMT3BlbkFJh6cUB6QhPVKBieezTqSg'
openai.api_key = api_key

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


def father_chat(messages, daughter_status):  # daughter_status를 매개변수로 추가
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
    
    while True:
        father_prompt = input("아빠의 말을 입력하세요 (종료하려면 'q' 입력): ")
        if father_prompt.lower() == 'q':
            break
        messages.append({"role": "user", "content": f"{father_status['father']['name']} (Father): {father_prompt}"})
        response = openai.ChatCompletion.create(
            model='gpt-3.5-turbo',
            messages=messages,
            max_tokens=200,
            temperature=0.5
        )
        daughter_reply = response['choices'][0]['message']['content']
        print("딸의 대답:", daughter_reply)
        daughter_response_json = {
            "father_message": father_prompt,
            "daughter_reply": daughter_reply
        }

        character_status_update(daughter_reply)
        print("JSON 출력:", json.dumps(daughter_response_json, indent=2))
        
        if 'daughter' in daughter_status and 'name' in daughter_status['daughter']:
            daughter_name = daughter_status['daughter']['name']
        else:
            daughter_name = "Daughter"
        
        messages.append({"role": "assistant", "content": f"{daughter_name}: {daughter_reply}"})


def character_status_update(daughter_reply):
    if "```json" in daughter_reply:
        start_index = daughter_reply.find("```json") + len("```json")
        end_index = daughter_reply.find("```", start_index)
        json_str = daughter_reply[start_index:end_index].strip()

        try:
            updateStat = json.loads(json_str)
            # 파일 저장 경로 설정
            json_path = os.path.join("conversationData", "updated_daughter_status.json")
            with open(json_path, 'w', encoding='utf-8') as json_file:
                json.dump(updateStat, json_file, ensure_ascii=False, indent=4)
            print(f"Updated parameters are saved in {json_path}")
            return updateStat
        except json.JSONDecodeError:
            print("JSON 인코딩 에러가 발생했습니다.")
            return None
    else:
        print("JSON 데이터가 포함된 대답이 없습니다.")
        return None
    
def main():
    make_child_status()
    
    daughter_status_path = os.path.join("conversationData", "daughter_status.json")
    
    if os.path.exists(daughter_status_path):
        with open(daughter_status_path, 'r', encoding='utf-8') as f:
            daughter_status = json.load(f)
        
        json_file = {json.dumps(daughter_status, ensure_ascii=False)}

        # 대화 초기 설정.
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
            "When a father has an out-of-context conversation or a story that has nothing to do with the content of the conversation, the daughter asks a reverse question or changes the subject."
            "E and I, S and N, T and F, J and P change their numbers according to their propensity at 100 each other."
            "The parameter numbers in the daughter_status change very minutely."
            "Daughter is a game character and based on her MBTI tendencies, if she's an extrovert, she likes hunting or outdoor activities,"
            "if she's introverted, she acts based on her tendencies, like being timid or doing things alone indoors."
            "Also fine-tune paramter's yourself. "
            "And at the end of my daughter's answer, please give me both the changed and unchanged parameter values in the json file."
            "Here's your status: "
            f"{json.dumps(daughter_status, ensure_ascii=False)}."
        )

        messages = [
            {"role": "system", "content": "You are a helpful assistant."},
            {"role": "user", "content": set_text}
        ]

        father_chat(messages, daughter_status)  # daughter_status를 father_chat 함수에 전달
        print("전체 대화 히스토리:")
        for message in messages:
            print(f"{message['role']}: {message['content']}")
    else:
        print("대화를 진행할 수 없습니다. daughter_status.json 파일이 존재하지 않습니다.")

if __name__ == "__main__":
    main()