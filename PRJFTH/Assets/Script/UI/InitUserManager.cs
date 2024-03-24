using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class InitUserManager : MonoBehaviour
{
    private void Start()
    {
        // MySQL 연결 문자열 설정
        string con = "Server=localhost;Database=projectforthehime;Uid=root;Pwd=1234";
        
        // MySqlConnection 객체 생성
        MySqlConnection connection = new MySqlConnection(con);

        try
        {
            // MySQL DB 연결
            connection.Open();
            Debug.Log("MySQL Connection Success!");
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex);
            Debug.LogError("MySQL Connection error: " + ex.Message);
            throw;
        }
        finally
        {
            // 연결 종료
            connection.Close();
        }
    }


    // EnterUser 버튼 눌리면
    public void OnClickEnterUserBtn()
    {
        // 씬 불러오기
        SceneManager.LoadScene("MainLevel_S");
    }
}
