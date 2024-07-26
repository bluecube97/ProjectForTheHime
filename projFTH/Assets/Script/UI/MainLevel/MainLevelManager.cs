using Newtonsoft.Json;
using System.Collections;
using UnityEngine.Networking;

namespace Script.UI.MainLevel
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainLevelManager : MonoBehaviour
    {
        private void Awake()
        {
            // 게임의 전체 프레임 레이트를 설정
            Application.targetFrameRate = 60; // 60 FPS로 설정
        }
        public void GoOutBtn()
        {
            SceneManager.LoadScene("OutingScene");
        }

        public void StartTurnBtn()
        {
            SceneManager.LoadScene("StartTurnScene");
        }

        public void CommunicationBtn()
        {
            SceneManager.LoadScene("ConvScene");
        }
    }
}