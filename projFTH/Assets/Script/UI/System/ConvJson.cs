using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.UI.System
{
    public class ConvJson : MonoBehaviour
    {
        public string filePath = "Assets/JSON/conversation.json";
        public InputField inputDataField;
        public Text outputDataText;

        private RootObject _rootObject;

        private void Start()
        {
            outputDataText.text = "abc";
            LoadJsonData();
        }

        private void LoadJsonData()
        {
            string jsonContent = File.ReadAllText(filePath);
            _rootObject = JsonUtility.FromJson<RootObject>(jsonContent);

            for (int i = 0; i < _rootObject.data.Count; i++)
            {
                string uMent = _rootObject.data[i].userMent;
                string gMent = _rootObject.data[i].gptMent;
                Debug.Log("userMent " + (i + 1) + ": " + uMent);
                Debug.Log("gptMent " + (i + 1) + ": " + gMent);
            }
            outputDataText.text = _rootObject.data[0].gptMent;
        }

        public void OnButtonClick()
        {
            string inputText = inputDataField.text;
            _rootObject.data.Add(new ObjectData { userMent = inputText});

            string jsonContent = JsonUtility.ToJson(_rootObject);
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