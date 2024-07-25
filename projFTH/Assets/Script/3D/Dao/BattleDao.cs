using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Script._3D.Dao
{
    public class BattleDao : MonoBehaviour
    {
        public static IEnumerator GetMobList(List<int> appearMobList, Action<List<Dictionary<string, object>>> callback)
        {
            const string url = "http://localhost:8080/api/battle/moblist";
            string jsonBody = JsonConvert.SerializeObject(appearMobList);

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
                string json = request.downloadHandler.text;
                List<Dictionary<string, object>> mobList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                callback(mobList);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}