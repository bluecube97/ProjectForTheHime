import json
import openai
import os

openai.api_key = "sk-proj-3ZLbBHwylhtASxE4BIaMT3BlbkFJh6cUB6QhPVKBieezTqSg"
previous_completion = None

def get_ment_from_unity():
    try:
        user_ment = input()
        if user_ment.strip() == "":
            return None
        return user_ment
    except EOFError as e:
        print(json.dumps({"error": str(e)}))

def make_whole_cov(path, message):
    if not os.path.exists(os.path.dirname(path)):
        os.makedirs(os.path.dirname(path))
    
    if not os.path.exists(path):
        whole_conversation = {"data": [message]}
    else:
        with open(path, 'r', encoding='utf-8') as file:
            whole_conversation = json.load(file)
        whole_conversation["data"].append(message)
    
    with open(path, 'w', encoding='utf-8') as file:
        json.dump(whole_conversation, file, indent=4)

def get_response():
    global previous_completion
    wc_path = "conversationData/whole_conversation.json"

    while True:
        question = get_ment_from_unity()
        if question is None or question.lower() == "close":
            break

        messages = [{"role": "user", "content": question}]
        if previous_completion:
            messages.append({"role": "assistant", "content": previous_completion.choices[0].message.content})

        completion = openai.ChatCompletion.create(
            model="gpt-3.5-turbo-1106",
            messages=messages
        )

        response = completion.choices[0].message.content.strip()
        json_response = json.dumps({"gpt_ment": response}, ensure_ascii=False)
        print(json_response)

        previous_completion = completion
        message = {"user_ment": question, "gpt_ment": response}
        #print(message)
        make_whole_cov(wc_path, message)

if __name__ == "__main__":
    get_response()
