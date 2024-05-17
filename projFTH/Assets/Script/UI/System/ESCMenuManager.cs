using UnityEngine.Serialization;

namespace Script.UI.System
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class EscMenuManager : MonoBehaviour
    {
        private static EscMenuManager _instance; // ESC메뉴의 인스턴스
        public GameObject escMenuBackGround; // 설정 패널 오브젝트
        private bool _settingActive; // 설정 화면 활성화 여부

        // 싱글톤 패턴을 사용하여 ESC 메뉴 인스턴스를 가져오는 속성
        public static EscMenuManager Instance
        {
            get
            {
                // 인스턴스가 없다면 새로 생성
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FindObjectOfType<EscMenuManager>();

                // 씬에 ESC메뉴가 없다면 새로 생성
                if (_instance != null)
                {
                    return _instance;
                }

                GameObject obj = new() { name = "ESCMenuBackGround" };
                _instance = obj.AddComponent<EscMenuManager>();

                return _instance;
            }
        }

        private void Start()
        {
            // 게임 시작 시 설정 패널을 비활성화
            escMenuBackGround.SetActive(false);
        }

        private void Update()
        {
            // ESC 키를 눌렀을 때
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }

            // ESC메뉴가 활성화 되어있지 않다면
            if (!_settingActive)
            {
                PauseGame(); // 게임 일시 정지
                ActivateEscMenu(); // ESC메뉴를 활성화
            }
            else
            {
                ResumeGame(); // 게임 재개
                DeactivateEscMenu(); // ESC메뉴를 비활성화
            }
        }

        public void OnClickConfig()
        {
            Debug.Log("설정창 열림");
        }

        public void OnClickLoad()
        {
            Debug.Log("로드씬 열림");
        }

        public void OnClickExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        private void PauseGame()
        {
            Time.timeScale = 0f; // 게임 일시 정지
            _settingActive = true; // 설정 화면이 활성화됨을 기록
        }

        private void ResumeGame()
        {
            Time.timeScale = 1f; // 게임 재개
            _settingActive = false; // 설정 화면이 비활성화됨을 기록
        }

        private void ActivateEscMenu()
        {
            escMenuBackGround.SetActive(true);
        }

        private void DeactivateEscMenu()
        {
            escMenuBackGround.SetActive(false);
        }
    }
}