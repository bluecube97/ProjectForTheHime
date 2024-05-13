using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UnityEngine;

namespace Script.UI.StartLevel.Dao
{
    public class StartLevelDao : MonoBehaviour
    {
        private readonly string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1111;Charset=utf8mb4";

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
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@userName", name);
                    cmd.Parameters.AddWithValue("@userSex", gender);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Dictionary<string, string> GetUserInfo()
        {
            Dictionary<string, string> userInfo = new Dictionary<string, string>();

            var sql = "SELECT tt.USERNAME AS username, tt.USERSEX AS USERSEX "+
                "  FROM tbl_test tt "+
                " ORDER BY tt.SEQ DESC LIMIT 1 ";

            using (MySqlConnection connection = new MySqlConnection(con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userInfo["username"] = reader.GetString("username").ToString();
                            userInfo["usersex"] = reader.GetString("usersex").ToString();
                        }
                    }
                }
            }
            return userInfo;
        }
    }
}