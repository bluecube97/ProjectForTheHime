using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class OpenAIApiTest : MonoBehaviour
{
    // OpenAI API 키
    string apiKey = "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";

    // 테스트용 사용자 입력
    string userInput = "Hello, GPT!";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateConversation(userInput));
    }

    IEnumerator GenerateConversation(string prompt)
    {
        // 대화 생성 요청 보내기
        string url = "https://api.openai.com/v1/engines/text-davinci-003/completions";
        string jsonRequestBody = "{\"prompt\": \"" + prompt + "\", \"max_tokens\": 50, \"temperature\": 0.7}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // API 헤더 설정
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            // JSON 요청 본문 설정
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // 요청 보내기
            yield return request.SendWebRequest();

            // 요청 완료 후 처리
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("AI Response: " + responseText);
            }
            else
            {
                Debug.LogError("Error generating conversation: " + request.error);
            }
        }
    }
}
