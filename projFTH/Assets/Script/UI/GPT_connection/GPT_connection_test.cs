using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GPTRequest : MonoBehaviour
{
    // OpenAI API 엔드포인트 URL
    private string gptEndpoint = "https://api.openai.com/v1/completions";

    // OpenAI API 인증 토큰
    private string apiKey = "sk-proj-3ZLbBHwylhtASxE4BIaMT3BlbkFJh6cUB6QhPVKBieezTqSg";

    // GPT 요청에 사용될 텍스트
    private string prompt = "Once upon a time,";

    void Start()
    {
        // GPT 요청 보내기
        StartCoroutine(SendGPTRequest());
    }

    IEnumerator SendGPTRequest()
    {
        // GPT에 보낼 JSON 데이터 생성
        string jsonData = "{\"model\":\"text-davinci-003\", \"prompt\":\"" + prompt + "\", \"max_tokens\":50}";

        // UnityWebRequest 생성
        UnityWebRequest request = new UnityWebRequest(gptEndpoint, "POST");

        // 요청 헤더 설정
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // JSON 데이터를 요청에 추가
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // 요청 보내기
        yield return request.SendWebRequest();

        // 요청이 성공적으로 완료되었는지 확인
        if (request.result == UnityWebRequest.Result.Success)
        {
            // 응답 텍스트 받기
            string responseText = request.downloadHandler.text;
            Debug.Log("GPT 응답: " + responseText);
        }
        else
        {
            // 요청이 실패한 경우 오류 메시지 출력
            Debug.LogError("GPT 요청 실패: " + request.error);
        }
    }
}
