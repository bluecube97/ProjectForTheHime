using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Script.UI.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.Outing.VarietyStoreScript
{
    public class VarietyStoreDao : MonoBehaviour
    {
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }
        public List<ItemListVO> LoadData()
        {
            List<ItemListVO> itemList = new List<ItemListVO>();
            var sql = "SELECT ti.ITEM_ID, TYPE_ID, ti.NAME, ti.`DESC`, ti.SELL_PRI, ti.BUY_PRI " +
                      " FROM TBL_ITEM ti " + 
                      " WHERE TYPE_ID =2001 " +
                      "    or TYPE_ID= 3005";
            using (MySqlConnection connection = new(ConnDB.Con))
            {   
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ItemListVO iv = new ItemListVO();
                            iv.ITEMNO = (string)reader["ITEM_ID"];
                            iv.TYPEID = (string)reader["TYPE_ID"];
                            iv.ITEMNAME = (string)reader["NAME"];
                            iv.ITEMDESC = (string)reader["DESC"];
                            iv.ITEMPR = (string)reader["BUY_PRI"];
                            itemList.Add(iv);
                        }
                    }
                }
            }
            return itemList;
        }

        public string GetUserInfo()
        {
            string Usercash = "";
            string sql = " select CASH " +
                         " from TBL_USERINFO  " +
                         "  where PID = @pid";
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Usercash = reader.GetString(0);
                            Debug.Log(Usercash);
                        }
                    }
                }
            }

            return Usercash;
        }

        public void UpdateUserCash(string payment)
        {
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    string sql = " update TBL_USERINFO " +
                                 " set CASH = @payCash " +
                                 " where PID = @pid";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@payCash", payment);
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerator GetBuyList(Action<List<Dictionary<string, object>>> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/outing/varstory/buy");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                List<Dictionary<string, object>> buylist = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                callback(buylist);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}