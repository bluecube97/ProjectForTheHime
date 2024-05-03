import openai
import json
import os

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
    daughter_status_json = load_json_data(daughter_status_path)

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
        user_request = input("당신의 딸과 이야기 해보세요 (종료하려면 'q' 입력): ")
        if user_request.lower() == 'q':
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
            print(daughter_reply)
            messages.append({"role": "assistant", "content": f"Daughter: {daughter_reply}"})

            # Try to extract and save the updated status if present in the reply
            if extract_and_save_updated_status(daughter_reply, daughter_update_path):
                update_daughter_status(daughter_status_path, daughter_update_path)
        except Exception as e:
            print(f"An error occurred during API interaction: {e}")

def main():
    daughter_status_path = os.path.join("conversationData", "daughter_status.json")
    daughter_update_path = os.path.join("conversationData", "updated_daughter_status.json")

    if os.path.exists(daughter_status_path):
       ConnectionGpt(daughter_status_path, daughter_update_path)
    else : 
        print("대화를 진행할 수 없습니다. daughter_status.json 파일이 존재하지 않습니다.")

if __name__ == "__main__":
    main()
