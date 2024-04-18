using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MySql.Data.MySqlClient;

public class initUserManager : MonoBehaviour
{
    public static
    // mySql 연결
    private void Start()
    {

    }

    public void OnClickEnterUserBtn()
    {
        // Load scene
        SceneManager.LoadScene("MainLevel_S");
    }
}