using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.System
{
    public class ConnDB
    {
        public const string Con = "Server=192.168.0.78;Database=projfth;Uid=studyuser;Pwd=1111;Charset=utf8mb4";
        private const string ServerUrl = "http://localhost:8080/board/unity";

        public static IEnumerator GetData()
        {
            UnityWebRequest request = UnityWebRequest.Get(ServerUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Received: " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Error: " + request.error);
            }
        }
    }
}