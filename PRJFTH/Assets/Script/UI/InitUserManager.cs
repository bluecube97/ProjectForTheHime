using System;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitUserManager : MonoBehaviour
{
    // EnterUser 버튼 눌리면
    public void OnClickEnterUserBtn()
    {
        // MySQL 연결 문자열 설정
        var con = "Server=localhost;Database=projectforthehime;Uid=root;Pwd=1234";

        // MySqlConnection 객체 생성
        var connection = new MySqlConnection(con);
        try
        {
            // 쿼리문
            var sql = " INSERT INTO userdb(USERNAME, USERSEX) " +
                      " VALUE ('?', '?') ";
            
            // MySqlCommand 객체 설정
            var cmd = new MySqlCommand(sql, connection);
            
            // 값을 입력할 컬럼
            cmd.Parameters.AddWithValue("@USERNAME", '?');
            cmd.Parameters.AddWithValue("@USERSEX", '?');

            // MySQL DB 연결
            connection.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            connection.Close();


        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex);
            Debug.LogError("MySQL Connection error: " + ex.Message);
            throw;
        }
        
        // 씬 불러오기
        SceneManager.LoadScene("MainLevel_S");
    }
}