using MySql.Data.MySqlClient;
using UnityEngine;

namespace Script.UI.StartLevel.Dao
{
    public class StartLevelDao : MonoBehaviour
    {
        private readonly string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

        public void SetUserInfo(string name, string gender)
        {
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
                    cmd.Parameters.AddWithValue("@userName", name);
                    cmd.Parameters.AddWithValue("@userSex", gender);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}