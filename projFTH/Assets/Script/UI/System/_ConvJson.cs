using Newtonsoft.Json;
using Script.UI.StartLevel.Dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Script.UI.System
{
    public class _SendDataToPython : MonoBehaviour
    {
        public InputField inputDataField; // Inspector에서 할당
        public Text outputDataText; // Inspector에서 할당
        private StartLevelDao _sld; // StartLevelDao를 사용하기 위한 변수
        private Dictionary<string, object> userinfo = new();//chatLog를 DB에 올리기 위한 userinfo를 담음
        private Dictionary<string, object> chatlog = new();//채팅로그를 담음

        // JSON 직렬화를 위한 클래스
        [global::System.Serializable]
        public class MessageData
        {
            public string user_ment;
        }

        // JSON 역직렬화를 위한 클래스
        [global::System.Serializable]
        public class ResponseData
        {
            public string gpt_ment;
        }

        public void OnButtonClick()
        {
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

            // JSON 데이터 생성
            MessageData dataToSend = new MessageData { user_ment = inputDataField.text };
            string json = JsonUtility.ToJson(dataToSend);

            StartCoroutine(GetPythonPath(path =>
            {
                StartCoroutine(GetPythonWorkSpace(pythonWorkSpace =>
                {
                    if (string.IsNullOrEmpty(pythonWorkSpace))
                    {
                        Debug.LogError("Failed to get Python workspace");
                        return;
                    }
                    try
                    {
                        Process process = new();
                        Debug.Log("Script path: " + path);
                        Debug.Log("Working directory: " + pythonWorkSpace);

                        process.StartInfo.FileName = "python";
                        process.StartInfo.Arguments = "\"" + path + "\"";
                        process.StartInfo.WorkingDirectory = pythonWorkSpace; // 'Assets' 폴더를 작업 디렉토리로 설정
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardInput = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();

                        // Python 스크립트에 데이터 전송
                        using (StreamWriter sw = new(process.StandardInput.BaseStream, Encoding.UTF8))
                        {
                            sw.WriteLine(json);
                            sw.WriteLine("END_OF_INPUT"); // 종료 신호
                        }

                        // Python 스크립트 출력 받기
                        using (StreamReader sr = new(process.StandardOutput.BaseStream, Encoding.UTF8))
                        {
                            string output = sr.ReadToEnd();

                            Debug.Log("Received Raw Data: " + output); // 받은 데이터 로깅
                            try
                            {
                                ResponseData response = JsonConvert.DeserializeObject<ResponseData>(output);
                                if (response == null || string.IsNullOrEmpty(response.gpt_ment))
                                {
                                    Debug.LogError("Failed to parse response or response is empty");
                                    return;
                                }

                                outputDataText.text = response.gpt_ment;
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("JSON 파싱 오류: " + e.Message);
                                string[] dummyResponse = new string[4] { "뭐라구요?", "다시 말해줘요!", "뭐라는거야!", "흥~" };
                                int randomIndex = UnityEngine.Random.Range(0, dummyResponse.Length);
                                outputDataText.text = dummyResponse[randomIndex];
                            }
                        }
                      
                        process.WaitForExit();
                        process.Close();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error: " + e.Message);
                    }
                }));
            }));
        }

        private IEnumerator GetPythonPath(Action<string> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/conv/pythonPath");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string pythonPath = request.downloadHandler.text;
                Debug.Log("IEnumerator " + pythonPath);
                callback(pythonPath);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }

        private IEnumerator GetPythonWorkSpace(Action<string> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/conv/pythonWorkSpace");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string pythonWorkSpace = request.downloadHandler.text;
                Debug.Log("IEnumerator " + pythonWorkSpace);
                callback(pythonWorkSpace);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }

        public void ReturnMainLevel()
        {
            SceneManager.LoadScene("MainLevelScene");
        }
    }
}