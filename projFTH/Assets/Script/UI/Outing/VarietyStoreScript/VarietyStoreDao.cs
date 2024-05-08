using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;

public class VarietyStoreDAO : MonoBehaviour
{
 private string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

  public int GetUserInfo()
{
int Usercash = 0;
 var sql = " SELECT USERCASH " +
            " FROM game_userinfo ";
using (MySqlConnection connection = new MySqlConnection(con))
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

        using (MySqlConnection connection = new MySqlConnection(con))
        {
            connection.Open();
            using (MySqlCommand cmd = connection.CreateCommand())
            {
                var sql = " update game_userinfo " +
                            " set USERCASH = (@payment)" +
                           " where SEQ = 1 ";
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@payment", payment);
                cmd.ExecuteNonQuery();
            }
        }

    }



}
