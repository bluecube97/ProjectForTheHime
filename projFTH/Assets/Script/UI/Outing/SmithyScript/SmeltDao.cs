using MySql.Data.MySqlClient;
using Script.UI.System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.UI.Outing.SmithyScript
{
    public class SmeltDao : MonoBehaviour
    {
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }

        //구매 리스트 받아오기
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
                            dic.Add("itemNo", reader["ITEM_ID"]);
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
            var sql = "SELECT ti.ITEM_ID, ti.NAME, ti.`DESC`, ti.SELL_PRI, ti.BUY_PRI " +
                      " FROM TBL_ITEM ti" +
                      " WHERE ti.TYPE_ID = 3000 ";
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
                            dic.Add("itemNo", reader["ITEM_ID"]);
                            dic.Add("itemNm", reader["NAME"]);
                            dic.Add("itemDesc", reader["DESC"]);
                            dic.Add("itemSellPr", reader["SELL_PRI"]);
                            dic.Add("itemBuyPr", reader["BUY_PRI"]);

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
        public void UpdateUserCash(int payment)
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
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502 ");

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
