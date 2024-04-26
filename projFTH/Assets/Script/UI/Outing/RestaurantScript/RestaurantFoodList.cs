using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantFoodList : MonoBehaviour
{
    public Text Food;

    List<Dictionary<string, object>> list = new();
    string con = "Server=localhost;Database=testdb;Uid=root;Pwd=1234;Charset=utf8mb4";


    
    public void OnClickEatMeueBtn()
    {
        var sql = "SELECT gr.FOODNO , gr.FOODNM , gr.FOODPRICE " +
                    "FROM game_restaurant gr ";
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
                       
                    }
                }
            }
        }
        string foodText = "";
        foreach (object item in list) {
             foodText += item +" ";

        }
        Food.text = foodText;
    }
}