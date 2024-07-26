using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Script.ApiLibrary;
using Script.UI.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.Outing.SmithyScript
{
    public class SmeltDao : MonoBehaviour
    {
        private static WebRequestManager _wrm;

        private void Awake()
        {
            _wrm = FindObjectOfType<WebRequestManager>();
        }
        public IEnumerator GetBuyLists(Action<List<Dictionary<string, object>>> callback)
        {
            string absoluteUrl = _wrm.GetAbsoluteUrl("api/outing/smithy/buy");
            UnityWebRequest request = UnityWebRequest.Get(absoluteUrl);
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
        public IEnumerator GetSmeltLists(Action<List<Dictionary<string, object>>> callback)
        {
            string absoluteUrl = _wrm.GetAbsoluteUrl("api/outing/smithy/list");
            UnityWebRequest request = UnityWebRequest.Get(absoluteUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                List<Dictionary<string, object>> smeltList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                callback(smeltList);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
       
        /*//구매 리스트 받아오기
        public List<Dictionary<string, object>> GetBuyList()
        {
            List<Dictionary<string, object>> BuyList = new List<Dictionary<string, object>>();

            string sql =
                "SELECT ti.ITEM_ID, ti.NAME, ti.`DESC`, ti.SELL_PRI, ti.BUY_PRI " +
                " FROM TBL_ITEM ti " +
                "WHERE ti.TYPE_ID = 3000 " +
                "  OR  ti.TYPE_ID = 3001 " +
                "  OR  ti.TYPE_ID = 3002 ";
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
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("itemId", reader["ITEM_ID"]);
                            dic.Add("itemNm", reader["NAME"]);
                            dic.Add("itemDesc", reader["DESC"]);
                            dic.Add("itemSellPr", reader["SELL_PRI"]);
                            dic.Add("itemBuyPr", reader["BUY_PRI"]);


                            BuyList.Add(dic);
                        }
                    }
                }
            }
            return BuyList;
        }
        //재련 LIST 받아오기
        public List<Dictionary<string, object>> GetSmeltList()
        {
            List<Dictionary<string, object>> SmeltList = new List<Dictionary<string, object>>();
            string sql = "SELECT  tr.RECIPE_ID, " +
                         " ti.ITEM_ID, " +
                         " ti.NAME, " +
                         "ti.`DESC` ," +
                         " tr.REQ_ITEM, " +
                         " ti1.NAME AS REQ_ITEM_NAME, " +
                         " tr.R_ITEM_CNT " +
                         " FROM TBL_ITEM ti " +
                         " INNER JOIN TBL_RECIPE tr " +
                         " ON ti.ITEM_ID = tr.ITEM_ID " +
                         " LEFT JOIN TBL_ITEM ti1 " +
                         " ON tr.REQ_ITEM = ti1.ITEM_ID " +
                         " WHERE ti.TYPE_ID = 1000 " +
                           "  OR ti.TYPE_ID = 1001 " +
                         " ORDER BY ti.ITEM_ID";
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
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("recipeId", reader["RECIPE_ID"]);
                            dic.Add("itemId", reader["ITEM_ID"]);
                            dic.Add("itemNm", reader["NAME"]);
                            dic.Add("itemDesc", reader["DESC"]);
                            dic.Add("req_item", reader["REQ_ITEM"]);
                            dic.Add("req_name", reader["REQ_ITEM_NAME"]);
                            dic.Add("req_itemcnt", reader["R_ITEM_CNT"]);
                         

                            SmeltList.Add(dic);
                        }
                    }
                }
            }
            return SmeltList;
        }
        //결재를 위한 USERINFO에서 보유 현금 들고 오기
        public string GetUserInfoFromDB()
        {
            string Usercash = "";
            var sql = " select CASH " +
                             " from TBL_USERINFO  " +
                           "  where PID = @pid";
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {                   
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");
                    cmd.CommandText = sql;
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
        
        //결재 후 남은 잔액 DB SET
        public void UpdateUserCash(string payment)
        {
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    var sql = " update TBL_USERINFO " +
                              " set CASH = (@payment)" +
                              " where PID = @pid ";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@payment", payment);
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");

                    cmd.ExecuteNonQuery();
                }
            }
        }*/

    }
}
