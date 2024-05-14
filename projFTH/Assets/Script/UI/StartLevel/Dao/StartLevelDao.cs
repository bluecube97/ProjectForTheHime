using MySql.Data.MySqlClient;
using Script.UI.System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.UI.StartLevel.Dao
{
    public class StartLevelDao : MonoBehaviour
    {
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }

        public static void SetUserInfo(string name, string gender)
        {
            const string sql = " INSERT INTO tbl_test (USERNAME, USERSEX) " +
                               " VALUES (@userName, @userSex) ";

            // DB 연결
            using MySqlConnection connection = new(ConnDB.Con);
            connection.Open();
            using MySqlCommand cmd = connection.CreateCommand();
            // DB에 유저 정보 저장
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@userName", name);
            cmd.Parameters.AddWithValue("@userSex", gender);
            cmd.ExecuteNonQuery();
        }

        public Dictionary<string, string> GetUserInfo()
        {
            Dictionary<string, string> userInfo = new Dictionary<string, string>();

            const string sql = "SELECT tt.USERNAME AS username, tt.USERSEX AS USERSEX "+
                               "  FROM tbl_test tt "+
                               " ORDER BY tt.SEQ DESC LIMIT 1 ";

            using MySqlConnection connection = new(ConnDB.Con);
            connection.Open();
            using MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Clear();

            using MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return userInfo;
            }

            userInfo["username"] = reader.GetString("username").ToString();
            userInfo["usersex"] = reader.GetString("usersex").ToString();

            return userInfo;
        }
    }
}