using Newtonsoft.Json;
using System.Collections;
using UnityEngine.Networking;

namespace Script.UI.MainLevel
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainLevelManager : MonoBehaviour
    {
        private void Awake()
        {
            // 토큰을 가져옵니다.
            GetToken();
        }

        private void GetToken()
        {
            StartCoroutine(LoginAndGetToken());
        }
        public void GoOutBtn()
        {
            SceneManager.LoadScene("OutingScene");
        }

        public void StartTurnBtn()
        {
            SceneManager.LoadScene("StartTurnScene");
        }

        public void CommunicationBtn()
        {
            SceneManager.LoadScene("ConvScene");
        }

        private static IEnumerator LoginAndGetToken()
        {
            Debug.Log("호출했음?");
            // 토큰 요청을 보냄
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/token");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("1111" + request.result);
                // 응답 본문을 가져옴
                string responseText = request.downloadHandler.text;

                Debug.Log("responseText: " + responseText);

                // 토큰을 PlayerPrefs에 저장
                PlayerPrefs.SetString("token", responseText);
            }
            else
            {
                Debug.LogError("Login failed: " + request.error);
            }
        }

    }

    public class AuthenticationResponse
    {
        public string jwt;
    }
}