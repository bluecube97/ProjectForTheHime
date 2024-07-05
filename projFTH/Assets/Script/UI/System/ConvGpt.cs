using Newtonsoft.Json;
using Script.UI.StartLevel.Dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Script.UI.System
{
    public class ConvGpt : MonoBehaviour
    {
        public InputField inputDataField; // Inspector에서 할당
        public Text outputDataText; // Inspector에서 할당
        private StartLevelDao _sld; // StartLevelDao를 사용하기 위한 변수
        private Dictionary<string, object> userinfo = new();//chatLog를 DB에 올리기 위한 userinfo를 담음
        private Dictionary<string, object> chatlog = new();//채팅로그를 담음

        public void OnClickSubmitButton()
        {
            outputDataText.text = "대화 생성 중...";
            // 필드 검증
            if (inputDataField == null || outputDataText == null)
            {
                Debug.LogError("InputField or OutputText is not assigned in the Inspector");
                return;
            }

            // 사용자 입력 검증
            if (string.IsNullOrEmpty(inputDataField.text))
            {
                Debug.LogError("Input field is empty");
                return;
            }

            string userConv = inputDataField.text;
            chatlog.Add("userment",userConv);
            StartCoroutine(GetConv(userConv, callback =>
            {
                //string gptConv = map["gpt_ment"].ToString();
                outputDataText.text = callback;
            }));

        }

        IEnumerator GetConv(string userConv, Action<string> callback)
        {
            const string url = "http://localhost:8080/api/conv/get";
            string jsonBody = JsonConvert.SerializeObject(userConv);
            chatlog.Add("gptment",jsonBody);
            byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);

            UnityWebRequest request = new(url, "POST")
            {
                uploadHandler = new UploadHandlerRaw(jsonToSend),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string conv = request.downloadHandler.text;
                //Dictionary<string, object> conv =
                    //JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                callback(conv);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
            StartCoroutine(_sld.GetUserEmail(info =>
            {
                userinfo = info;
                string pid = userinfo["useremail"].ToString();
                chatlog.Add("pid",pid);
                StartCoroutine(_sld.SetChatLog(chatlog));
            }));
        }

        public void ReturnMainLevel()
        {
            SceneManager.LoadScene("MainLevelScene");
        }
    }
}