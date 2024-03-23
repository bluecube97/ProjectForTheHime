using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitUserManager : MonoBehaviour
{
    // EnterUser 버튼 눌리면
    public void OnClickEnterUserBtn()
    {
        // 씬 불러오기
        SceneManager.LoadScene("MainLevel_S");
    }
}
