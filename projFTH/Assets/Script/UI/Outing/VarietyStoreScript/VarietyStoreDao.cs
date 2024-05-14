using MySql.Data.MySqlClient;
using Script.UI.System;
using UnityEngine;

public class VarietyStoreDAO : MonoBehaviour
{
    private ConnDB _connDB;

    private void Awake()
    {
        _connDB = new ConnDB();
    }

    public int GetUserInfo()
    {
        int Usercash = 0;
        string sql = " SELECT USERCASH " +
                     " FROM game_userinfo ";
        using (MySqlConnection connection = new(ConnDB.Con))
        {
            connection.Open();
            using (MySqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Usercash = reader.GetInt32(0);
                        Debug.Log(Usercash);
                    }
                }
            }
        }

        return Usercash;
    }

    public void UpdateUserCash(int payment)
    {
        using (MySqlConnection connection = new(ConnDB.Con))
        {
            connection.Open();
            using (MySqlCommand cmd = connection.CreateCommand())
            {
                string sql = " update game_userinfo " +
                             " set USERCASH = (@payment)" +
                             " where SEQ = 1 ";
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@payment", payment);
                cmd.ExecuteNonQuery();
            }
        }
    }
}