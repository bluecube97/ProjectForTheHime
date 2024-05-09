using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using static StatusJson;
using System.Collections.Generic;

public class StatusJson : MonoBehaviour
{
    public Text outputNameData;
    public Text outputAgeData;
    public Text outputMbtiData;
    public Text outputHpData;
    public Text outputMpData;

    // JSON 역직렬화를 위한 클래스
    [System.Serializable]
    public class Daughter_status
    {
        public string name;
        public string sex;
        public int age;
        public string mbti;
        public int hp;
        public int mp;
        public string stress;
        public string fatigue;
        public int E;
        public int I;
        public int S;
        public int N;
        public int T;
        public int F;
        public int J;
        public int P;
    }

    [System.Serializable]
    public class nowStatus
    {
        public Daughter_status daughter;
    }


    void Start()
    {
        string scriptPath = Application.dataPath + "/JSON/conversationData/statusRecord.json";

        if (File.Exists(scriptPath))
        {
            // 파일에서 JSON 문자열 읽기
            string jsonData = File.ReadAllText(scriptPath);

            // JSON 데이터를 NowStatus 객체 배열로 역직렬화
            List<nowStatus> statusDataList = JsonConvert.DeserializeObject<List<nowStatus>>(jsonData);

            // 배열의 마지막 요소 접근
            if (statusDataList.Count > 0)
            {
                nowStatus lastStatus = statusDataList[statusDataList.Count - 1];

                // UI 컴포넌트에 데이터 할당
                if (outputNameData != null)
                    outputNameData.text = lastStatus.daughter.name;
                if (outputAgeData != null)
                    outputAgeData.text = lastStatus.daughter.age.ToString();
                if (outputMbtiData != null)
                    outputMbtiData.text = lastStatus.daughter.mbti;
                if (outputHpData != null)
                    outputHpData.text = lastStatus.daughter.hp.ToString();
                if (outputMpData != null)
                    outputMpData.text = lastStatus.daughter.mp.ToString();
            }
            else
            {
                Debug.LogError("JSON data array is empty.");
            }
        }
        else
        {
            Debug.LogError("Cannot find JSON file at: " + scriptPath);
        }
    }
}
