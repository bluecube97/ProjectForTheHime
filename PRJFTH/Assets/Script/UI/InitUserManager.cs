using System;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InitUserManager : MonoBehaviour
{
    public InputField InputUserNameField;
    public Dropdown InputUserSexDropDown;

    // MySQL 연결 문자열 설정
    private string con = "Server=localhost;Database=projectforthehime;Uid=root;Pwd=1234";

    // EnterUser 버튼 눌리면
    public void OnClickEnterUserBtn()
    {
        var userName = InputUserNameField.text;
        var userSex = InputUserSexDropDown.options[InputUserSexDropDown.value].text;

        // MySqlConnection 객체 생성
        using (var connection = new MySqlConnection(con))
        {
            try
            {
                // 쿼리문
                var sql = " INSERT INTO userdb(USERNM, USERSEX) " +
                          " VALUES (@userNm, @userSex) ";

                // MySqlCommand 객체 설정
                using (var cmd = new MySqlCommand(sql, connection))
                {
                    // 값을 입력할 컬럼

                    cmd.Parameters.AddWithValue("@userNm", userName);
                    cmd.Parameters.AddWithValue("@userSex", userSex);

                    // MySQL DB 연결
                    connection.Open();
                    var rowsAffected = cmd.ExecuteNonQuery();
                    Debug.Log("Inserted " + rowsAffected + " rows into userdb.");
                    connection.Close();
                }
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
}