using Script.UI.StartLevel.Dao;
using Script.UI.System;
using System.Collections;
using System.Collections.Generic;

namespace Script.UI.StartLevel.Manager
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    public class MainMenuManager : MonoBehaviour
    {
        private StartLevelDao _sld; // StartLevelDao를 사용하기 위한 변수
        private SaveLoadDao sld;//SaveLoad를 사용하기 위한 변수

        private Dictionary<string, object> userinfo = new Dictionary<string, object>();

        public void Awake()
        {
            _sld = GetComponent<StartLevelDao>();
            sld = GetComponent<SaveLoadDao>();

        }
        
        
        // 새로 시작하기 버튼을 클릭하면 호출
        public void OnClickNewStart()
        {
            // 씬 불러오기 (씬 전환)
            SceneManager.LoadScene("InitUserScene");
        }

        // 게임 불러오기 버튼을 클릭하면 호출
        public void OnClickLoadStart()
        {
            StartCoroutine(OnClickLoadStartCoroutine());
            StartCoroutine(sld.LoadGame());

        }
        private IEnumerator  OnClickLoadStartCoroutine()
        {
            bool userInfoFetched = false;
            //Spring에 있는 session값 받아옴
            StartCoroutine(_sld.GetUserEmail(list =>
            {
                userinfo = list;
                userInfoFetched = true;

            }));
            yield return new WaitUntil(() => userInfoFetched);

            string userEmail = userinfo["useremail"].ToString();
            //session값 있는지 확인
            StartCoroutine(_sld.SearchUserInfo(userEmail, count =>
            {
                //새션값이 없다면
                if (count <= 0 )
                {                    
                    SceneManager.LoadScene("InitUserScene");


                }
                else
                {
                    SceneManager.LoadScene("MainLevelScene");
                }
            }));
            
        }

        // 설정 버튼을 클릭하면 호출
        // public void OnClickConfig()
        // {
        //     SceneManager.LoadScene("Config");
        // }

        // 게임 종료 버튼을 클릭하면 호출
        public void OnClickExitGame()
        {
            #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}