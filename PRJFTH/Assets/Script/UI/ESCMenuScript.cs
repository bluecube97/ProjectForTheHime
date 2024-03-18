using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCMenuScript : MonoBehaviour
{
    private static ESCMenuScript instance; // ESC메뉴의 인스턴스
    public GameObject ESCMenuBackGround; // 설정 패널 오브젝트
    private bool settingActive; // 설정 화면 활성화 여부

    // 싱글톤 패턴을 사용하여 ESC 메뉴 인스턴스를 가져오는 속성
    public static ESCMenuScript Instance
    {
        get
        {
            // 인스턴스가 없다면 새로 생성
            if (instance == null)
            {
                instance = FindObjectOfType<ESCMenuScript>();

                // 씬에 ESC메뉴가 없다면 새로 생성
                if (instance == null)
                {
                    var obj = new GameObject();
                    obj.name = "ESCMenuBackGround";
                    instance = obj.AddComponent<ESCMenuScript>();
                }
            }

            return instance;
        }
    }

    private void Start()
    {
        // 게임 시작 시 설정 패널을 비활성화
        ESCMenuBackGround.SetActive(false);
    }

    private void Update()
    {
        // ESC 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ESC메뉴가 활성화 되어있지 않다면
            if (!settingActive)
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
        settingActive = true; // 설정 화면이 활성화됨을 기록
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // 게임 재개
        settingActive = false; // 설정 화면이 비활성화됨을 기록
    }

    private void ActivateEscMenu()
    {
        ESCMenuBackGround.SetActive(true);
    }

    private void DeactivateEscMenu()
    {
        ESCMenuBackGround.SetActive(false);
    }
}