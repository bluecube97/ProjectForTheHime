using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.System
{
    public class SaveLoadDao
    {
        private ConnDB _connDB;

        // 객체가 초기화될 때 호출되는 메서드
        private void Awake()
        {
            _connDB = new ConnDB(); // 데이터베이스 연결 객체 생성
        }
        public IEnumerator SaveGame()
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/conv/save");
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
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/conv/load");
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