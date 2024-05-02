using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SendDataToPython : MonoBehaviour
{
    public InputField inputDataField;
    public Text outputDataText;

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
        MessageData dataToSend = new MessageData { user_ment = inputDataField.text };
        string json = JsonUtility.ToJson(dataToSend);

        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "python";
            process.StartInfo.Arguments = @"/Assets/JSON/connection.py";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            using (StreamWriter sw = process.StandardInput)
            {
                sw.WriteLine(json);
            }

            using (StreamReader sr = process.StandardOutput)
            {
                string output = sr.ReadToEnd(); // 전체 출력을 읽습니다.
                ResponseData response = JsonUtility.FromJson<ResponseData>(output);
                outputDataText.text = response.gpt_ment; // Unity UI에 표시

                string ment = response.gpt_ment; // 응답에서 gpt_ment 값을 추출하여 변수에 저장
                UnityEngine.Debug.Log("Received ment: " + ment); // 콘솔에 로그 출력
            }

        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Error: " + e.Message);
        }
    }
}
