using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Script.UI.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.Outing.Hospital
{
    public class HospitalDao : MonoBehaviour
    {
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }

        /*
        //구매 아이템 목록을 담음
        public List<Dictionary<string, object>> getBuyList()
        {
            List<Dictionary<string, object>> SellList = new();
            string sql = "SELECT ti.ITEM_ID, ti.NAME, ti.`DESC`, ti.SELL_PRI, ti.BUY_PRI " +
                          " FROM TBL_ITEM ti " + 
                         " WHERE TYPE_ID =3004 " +
                         "    or TYPE_ID= 2002";
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
                            Dictionary<string, object> dic = new();
                            dic.Add("itemNo", reader.GetString(0));
                            dic.Add("itemNm", reader.GetString(1));
                            dic.Add("itemDesc", reader.GetString(2));
                            dic.Add("itemPrice", reader.GetString(4));

                            SellList.Add(dic);
                        }
                    }
                }
            }
            return SellList;
        }

        //유저 정보를 담음
        public Dictionary<string, object> GetUserInfo()
        {
            Dictionary<string, object> dic = new();

            string sql = " select CASH, CHP, MAXHP " +
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
                            dic.Add("userCash", reader.GetString(0));
                            dic.Add("userHP", reader.GetString(1));
                            dic.Add("userMaxHP", reader.GetString(2));
                        }
                    }
                }
            }

            return dic;
        }
        
        //치료 후 정보 갱신
        public void SetAfterHeal(string payCash, string userMaxHP)
        {
            string sql = " update TBL_USERINFO " +
                         " set CASH = @payCash, " +
                         "     CHP = @userHP " +
                         " where PID = @pid";
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    // DB에 유저 정보 저장
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@payCash", payCash);
                    cmd.Parameters.AddWithValue("@userHP", userMaxHP);
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //구매 후 정보 갱신
        public void SetBuyAfter(string payCash)
        {
            string sql = " update TBL_USERINFO " +
                         " set CASH = @payCash " +
                         " where PID = @pid";
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    // DB에 유저 정보 저장
                    cmd.Parameters.Clear();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@payCash", payCash);
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateBuyThing(string bitem, string itemid)
        {
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    string sql = " update TBL_INVEN " +
                                 " set CNT = (@bitem)" +
                                 " where PID = (@pid) " +
                                 " AND ITEM_ID = (@itemid)";
                    // DB에 유저 정보 저장
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@bitem", bitem);
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");
                    cmd.Parameters.AddWithValue("@itemid", itemid);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertBuyThing(string itemid)
        {
            string sql = "  INSERT INTO TBL_INVEN (PID, ITEM_ID , CNT, USBL,USBL_SLOT) " +
                         " values (@pid, @itemid, @cnt, @usbl, @slot) ";
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");
                    cmd.Parameters.AddWithValue("@itemid", itemid);
                    cmd.Parameters.AddWithValue("@cnt", "1");
                    cmd.Parameters.AddWithValue("@usbl", "0");
                    cmd.Parameters.AddWithValue("@slot", "");

                    cmd.ExecuteNonQuery();
                }
            }
        }
        */

        public IEnumerator GetBuyLists(Action<List<Dictionary<string, object>>> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/outing/hospital/buy");
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
        public IEnumerator SetAfterHeals(string pid, string payCash, string userMaxHP)
        {
            string url = "http://localhost:8080/api/outing/hospital/heal";

            // WWWForm 생성
            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("payment", payCash);
            form.AddField("maxhp", userMaxHP);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
            }
        }
    }
}