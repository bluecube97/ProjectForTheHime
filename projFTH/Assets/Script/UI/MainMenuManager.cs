using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // 새로 시작하기 버튼을 클릭하면 호출
    public void OnClickNewStart()
    {
        // 씬 불러오기 (씬 전환)
        SceneManager.LoadScene("InitUserScene");
    }

    // 게임 불러오기 버튼을 클릭하면 호출
    public void OnClickLoadStart()
    {
        SceneManager.LoadScene("MainLevel_S");
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
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}