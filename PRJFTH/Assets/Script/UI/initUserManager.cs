using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class initUserManager : MonoBehaviour
{
    public InputField inputUserNameField;
    public Dropdown inputUserSexDropDown;

     public void OnClickEnterUserBtn()
     {
         // initUserScene에서 userName과 userSex 값을 받아옴
         var userName = inputUserNameField.text;
         var userSex = inputUserSexDropDown.options[inputUserSexDropDown.value].text;

         // DB 연결
         string dbName = "projectForTheHimeDB.db";
         string dbPath = Path.Combine(Application.streamingAssetsPath, dbName);
         string connection = "Data Source=" + dbPath + ";Version=3;";
         IDbConnection dbConnection = new SqliteConnection(connection);
         dbConnection.Open();

         var sql = " insert into TBLUSER (USERNM, USERSEX) " +
                   " values (?, ?) "; // 쿼리문

         // valuse 값 설정
         var dbCommand = dbConnection.CreateCommand();
         dbCommand.CommandText = sql;
         dbCommand.Parameters.Add(new SqliteParameter { Value = userName });
         dbCommand.Parameters.Add(new SqliteParameter { Value = userSex });

         // 쿼리문 실행
         dbCommand.ExecuteNonQuery();

         // DB 연결 해제
         dbCommand.Dispose();
         dbConnection.Close();

         // Load scene
         SceneManager.LoadScene("MainLevel_S");
    }
}