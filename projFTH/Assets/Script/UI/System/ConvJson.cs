using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.System
{
    public class ConvJson : MonoBehaviour
    {
        public string filePath = "Assets/JSON/conversation.json";
        public InputField InputDataField;
        public Text OutputDataText;

        private RootObject rootObject;

        private void Start()
        {
            OutputDataText.text = "abc";
            LoadJsonData();
        }

        private void LoadJsonData()
        {
            string jsonContent = File.ReadAllText(filePath);
            rootObject = JsonUtility.FromJson<RootObject>(jsonContent);

            for (int i = 0; i < rootObject.data.Count; i++)
            {
                string uMent = rootObject.data[i].userMent;
                string gMent = rootObject.data[i].gptMent;
                Debug.Log("userMent " + (i + 1) + ": " + uMent);
                Debug.Log("gptMent " + (i + 1) + ": " + gMent);
            }
            OutputDataText.text = rootObject.data[0].gptMent;
        }

        public void OnButtonClick()
        {
            string inputText = InputDataField.text;
            rootObject.data.Add(new ObjectData { userMent = inputText});

            string jsonContent = JsonUtility.ToJson(rootObject);
            File.WriteAllText(filePath, jsonContent);
        }

    }
}

[Serializable]
public class ObjectData
{
    public string userMent, gptMent;
}

[Serializable]
public class RootObject
{
    public List<ObjectData> data;
}