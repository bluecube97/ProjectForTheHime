using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class initUserManager : MonoBehaviour
{
    // mySql 연결

    public void OnClickEnterUserBtn()
    {
        // Load scene
        SceneManager.LoadScene("MainLevel_S");
    }
}