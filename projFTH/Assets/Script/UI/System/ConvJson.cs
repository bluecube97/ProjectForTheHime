using System.Diagnostics;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class SendDataToPython : MonoBehaviour
{
    public InputField inputDataField;  // Inspector에서 할당
    public Text outputDataText;        // Inspector에서 할당

    // JSON 직렬화를 위한 클래스
    [System.Serializable]
    public class MessageData
    {
        public string user_ment;
    }

    // JSON 역직렬화를 위한 클래스
    [System.Serializable]
    public class ResponseData
    {
        public string gpt_ment;
    }

    public void OnButtonClick()
    {
        // 필드 검증
        if (inputDataField == null || outputDataText == null)
        {
            UnityEngine.Debug.LogError("InputField or OutputText is not assigned in the Inspector");
            return;
        }

        // 사용자 입력 검증
        if (string.IsNullOrEmpty(inputDataField.text))
        {
            UnityEngine.Debug.LogError("Input field is empty");
            return;
        }

        // JSON 데이터 생성
        MessageData dataToSend = new MessageData { user_ment = inputDataField.text };
        string json = JsonUtility.ToJson(dataToSend);

        try
        {
            Process process = new Process();
            string scriptPath = Application.dataPath + "/JSON/connectionManager.py";  // 수정된 스크립트 경로

            process.StartInfo.FileName = "python";
            process.StartInfo.Arguments = "\"" + scriptPath + "\"";  // 경로에 공백이 있을 수 있으므로 인용부호 추가
            process.StartInfo.WorkingDirectory = Application.dataPath;  // 'Assets' 폴더를 작업 디렉토리로 설정
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            UnityEngine.Debug.Log("Script path: " + scriptPath);
            UnityEngine.Debug.Log("Working directory: " + Application.dataPath);

            // Python 스크립트에 데이터 전송
            using (StreamWriter sw = new StreamWriter(process.StandardInput.BaseStream, Encoding.UTF8))
            {
                sw.WriteLine(json);
                sw.WriteLine("END_OF_INPUT");  // 종료 신호
            }

            // Python 스크립트 출력 받기
            using (StreamReader sr = new StreamReader(process.StandardOutput.BaseStream, Encoding.UTF8))
            {
                string output = sr.ReadToEnd();
                UnityEngine.Debug.Log("Received Raw Data: " + output);  // 받은 데이터 로깅
                try
                {
                    ResponseData response = JsonConvert.DeserializeObject<ResponseData>(output);
                    if (response == null || string.IsNullOrEmpty(response.gpt_ment))
                    {
                        UnityEngine.Debug.LogError("Failed to parse response or response is empty");
                        return;
                    }
                    outputDataText.text = response.gpt_ment;
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError("JSON 파싱 오류: " + e.Message);
                }
            }

            process.WaitForExit();
            process.Close();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Error: " + e.Message);
        }
    }
}
