using Newtonsoft.Json;
using Script.ApiLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.System
{
    public class SaveLoadDao : MonoBehaviour
    {
        public IEnumerator SaveGame()
        {
            string absoluteUrl = WebRequestManager.GetAbsoluteUrl("api/conv/save");
            UnityWebRequest request = UnityWebRequest.Get(absoluteUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("저장 되었습니다");
            }
            else
            {
                Debug.LogError("Error: " + request.error); // 오류 로그 출력
            }
        }

        public IEnumerator LoadGame()
        {
            string absoluteUrl = WebRequestManager.GetAbsoluteUrl("api/conv/load");
            Debug.Log("url: " + absoluteUrl);
            UnityWebRequest request = UnityWebRequest.Get(absoluteUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("불러오기 완료 되었습니다.");
            }
            else
            {
                Debug.LogError("Error: " + request.error); // 오류 로그 출력
            }
        }
    }
}