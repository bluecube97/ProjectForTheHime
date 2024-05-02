using JetBrains.Annotations;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using Script.UI.StartLevel.Dao;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SmithyDao : MonoBehaviour
{
    string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

    public List<Dictionary<string, object>> GetWeaponList()
    {
        List<Dictionary<string, object>> WeaponList = new List<Dictionary<string, object>>();
        var sql = "select item_name , item_cost, item_id from tbl_weapon";
        using (MySqlConnection connection = new MySqlConnection(con))
        {
            connection.Open();
            using (MySqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        string weaponName = (string)reader["item_name"];
                        int weaponPrice = (int)reader["item_cost"];
                        
                        dic.Add("WEAPONNM", weaponName);
                        dic.Add("WEAPONPRICE", weaponPrice);

                        WeaponList.Add(dic);
                    }
                }
            }
        }
       
        return WeaponList;
    }



    public int  GetUserInfoFromDB()
    {
        int Usercash = 0;
        var sql = "  SELECT gu.USERCASH " +
                 "   FROM game_userinfo gu ";
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