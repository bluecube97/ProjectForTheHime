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
        private static WebRequestManager _wrm;

        private void Awake()
        {
            _wrm = FindObjectOfType<WebRequestManager>();
        }

        public IEnumerator SaveGame()
        {
            string absoluteUrl = _wrm.GetAbsoluteUrl("api/conv/save");
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
            string absoluteUrl = _wrm.GetAbsoluteUrl("api/conv/load");
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