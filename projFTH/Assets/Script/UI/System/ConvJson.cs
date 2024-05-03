using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEditor;
using System.Text;


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
        if (inputDataField == null || outputDataText == null)
        {
            UnityEngine.Debug.LogError("InputField or OutputText is not assigned in the Inspector");
            return;
        }

        MessageData dataToSend = new MessageData { user_ment = inputDataField.text };
        string json = JsonUtility.ToJson(dataToSend);

        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "python";
            process.StartInfo.Arguments = @"./Assets/JSON/connectionManager.py";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            using (StreamWriter sw = new StreamWriter(process.StandardInput.BaseStream, Encoding.UTF8))
            {
                sw.WriteLine(json);
            }

            using (StreamReader sr = new StreamReader(process.StandardOutput.BaseStream, Encoding.UTF8))
            {
                string output = sr.ReadToEnd();
                int endIndex = output.IndexOf("}") + 1;
                string validJson = output.Substring(0, endIndex);

                try
                {
                    ResponseData response = JsonUtility.FromJson<ResponseData>(validJson);
                    outputDataText.text = response.gpt_ment;
                    UnityEngine.Debug.Log("gpt_ment: " + response.gpt_ment);
                }
                catch (Exception parseException)
                {
                    UnityEngine.Debug.LogError("JSON 파싱오류: " + parseException.Message);
                    UnityEngine.Debug.LogError("gpt_ment: " + output);
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