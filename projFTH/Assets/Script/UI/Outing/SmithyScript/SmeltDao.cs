using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UnityEngine;

public class SmeltDao : MonoBehaviour
{
    private string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";


    public List<Dictionary<string, object>> GetSmeltList()
    {
        List<Dictionary<string, object>> SmeltList = new List<Dictionary<string, object>>();

        string sql = "select item_name , item_cost, item_id " 
                     + "from tbl_weapon";
                   

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
                        dic.Add("item_name", reader["item_name"]);
                        dic.Add("item_cost", reader["item_cost"]);
                    

                       SmeltList.Add(dic);
                    }
                }
            }
        }

        return SmeltList;
    }

        public List<Dictionary<string, object>> GetSubmitSmeltList()
    {
        List<Dictionary<string, object>> SmeltList = new List<Dictionary<string, object>>();

        string sql = "select item_name , item_cost, item_id " 
                     + "from tbl_weapon";

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
                        dic.Add("item_name", reader["item_name"]);
                        dic.Add("item_cost", reader["item_cost"]);

                        SmeltList.Add(dic);
                    }
                }
            }
        }

        return SmeltList;

    }
}
