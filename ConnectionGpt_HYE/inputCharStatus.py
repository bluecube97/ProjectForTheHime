import openai
import json
import os

# OpenAI API 키 설정
api_key = 'sk-proj-SrmvFmvip9TvsU8P9i3oT3BlbkFJ9dN0aAhwog6sSQ8aXL22'
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
        daughter_data["daughter"][key] = value

    # daughter_status.json 파일에 daughter_data 저장
    with open(communication_path, 'w') as f:
        json.dump(daughter_data, f, indent=4)
    print("daughter_status.json 파일이 생성되었습니다.")


def load_daughter_status(path):
    with open(path, 'r', encoding='utf-8') as f:
        return json.load(f)

def updated_daughter_status(origin_path, update_path):
    # update_path에서 데이터를 읽고 origin_path에 덮어쓰기
    if os.path.exists(update_path):
        with open(update_path, 'r', encoding='utf-8') as update_file:
            update_data = json.load(update_file)
        with open(origin_path, 'w', encoding='utf-8') as origin_file:
            json.dump(update_data, origin_file, indent=4)
        print("Status_Update_successful")
    else:
        print("Updated JSON file does not exist")

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


def father_chat(daughter_status_path, daughter_update_path):
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
            "Here's your status: " f"{json.dumps(daughter_status_path, ensure_ascii=False)}."
        )

    messages = [
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": set_text}
    ]
    
    while True:
        updated_daughter_status(daughter_status_path, daughter_update_path)
        daughter_status = load_daughter_status(daughter_status_path)  # 상태 파일 불러오기

        father_prompt = input("당신의 딸과 이야기 해보세요 : (종료하려면 'q' 입력): ")
        if father_prompt.lower() == 'q':
            break

        messages.append({"role": "user", "content": f"{father_status['father']['name']} (Father): {father_prompt}"})
        response = openai.ChatCompletion.create(
            model='gpt-3.5-turbo',
            messages=messages,
            max_tokens=300,
            temperature=0.5
        )
        daughter_reply = response['choices'][0]['message']['content']
        print(daughter_reply)

        if extract_and_save_updated_status(daughter_reply, daughter_update_path):
            updated_daughter_status(daughter_status_path, daughter_update_path)

        if 'daughter' in daughter_status and 'name' in daughter_status['daughter']:
            daughter_name = daughter_status['daughter']['name']
        else:
            daughter_name = "Daughter"

        messages.append({"role": "assistant", "content": f"{daughter_name}: {daughter_reply}"})

def main():
    make_child_status()
    daughter_status_path = os.path.join("conversationData", "daughter_status.json")
    daughter_update_path = os.path.join("conversationData", "updated_daughter_status.json")
    
    if os.path.exists(daughter_status_path):
        father_chat(daughter_status_path, daughter_update_path)  # 경로를 전달합니다.
    else:
        print("대화를 진행할 수 없습니다. daughter_status.json 파일이 존재하지 않습니다.")


if __name__ == "__main__":
    main()
