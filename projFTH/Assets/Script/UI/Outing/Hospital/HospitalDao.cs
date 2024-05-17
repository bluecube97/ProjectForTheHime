using MySql.Data.MySqlClient;
using Script.UI.System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.UI.Outing.Hospital
{
    public class HospitalDao : MonoBehaviour
    {
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }

        public List<Dictionary<string, object>> getSellList()
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
    }
}