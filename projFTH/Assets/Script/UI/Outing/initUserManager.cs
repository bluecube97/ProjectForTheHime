using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class initUserManager : MonoBehaviour
{
    public InputField inputUserNameField;
    public Dropdown inputUserSexDropDown;

    string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

     public void OnClickEnterUserBtn()
     {
         // initUserScene에서 userName과 userSex 값을 받아옴
         var userName = inputUserNameField.text;
         var userSex = inputUserSexDropDown.options[inputUserSexDropDown.value].text;

         var sql = " INSERT INTO tbl_test (USERNAME, USERSEX) " +
                   " VALUES (@userName, @userSex) ";

         // DB 연결
         using (MySqlConnection connection = new MySqlConnection(con))
         {
             connection.Open();
             using (MySqlCommand cmd = connection.CreateCommand())
             {
                 // DB에 유저 정보 저장
                 cmd.CommandText = sql;
                 cmd.Parameters.AddWithValue("@userName", userName);
                 cmd.Parameters.AddWithValue("@userSex", userSex);
                 cmd.ExecuteNonQuery();
             }
         }

         // Load scene
         SceneManager.LoadScene("MainLevel_S");
    }
}