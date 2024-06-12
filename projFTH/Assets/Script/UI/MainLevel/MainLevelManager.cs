using Script.UI.System;
using System;

namespace Script.UI.MainLevel
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainLevelManager : MonoBehaviour
    {
        public void GoOutBtn()
        {
            SceneManager.LoadScene("OutingScene");
            Debug.Log("외출하기()");
            SceneManager.LoadScene("OutingScene");
        }

        public void StartTurnBtn()
        {
            SceneManager.LoadScene("StartTurnScene");
        }

        public void CommunicationBtn()
        {
            Debug.Log("대화하기()");
            SceneManager.LoadScene("TestJson");
        }
    }
}