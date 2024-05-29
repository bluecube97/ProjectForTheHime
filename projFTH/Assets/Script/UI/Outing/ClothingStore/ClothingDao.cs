using MySql.Data.MySqlClient;
using Script.UI.System;
using System.Collections.Generic;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.UI.Outing.ClothingStore
{
    public class ClothingDao : MonoBehaviour
    {
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }

        public List<ClothingVO> GetClothingList()
        {
            List<ClothingVO> clothingList = new();
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
                         " WHERE ti.TYPE_ID = 1002 " +
                         " ORDER BY ti.ITEM_ID";

            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cmd.Parameters.Clear();

                        while (reader.Read())
                        {
                            ClothingVO cv = new ClothingVO();
                            cv.r_id = reader.GetString("RECIPE_ID");
                            cv.r_itemid = reader.GetString("ITEM_ID");
                            cv.r_name = reader.GetString("NAME");
                            cv.r_desc = reader.GetString("DESC");
                            cv.req_item = reader.GetString("REQ_ITEM");
                            cv.req_name = reader.GetString("REQ_ITEM_NAME");
                            cv.req_itemcnt = reader.GetString("R_ITEM_CNT");

                            clothingList.Add(cv);
                        }
                    }
                }
            }

            return clothingList;
        }

        public List<ClothingVO> GetClothingBuyList()
        {
            List<ClothingVO> CltBuyList = new();


            string sql = "SELECT ti.ITEM_ID, ti.NAME, ti.`DESC`, ti.SELL_PRI, ti.BUY_PRI" +
                         " FROM TBL_ITEM ti " +
                         " WHERE ti.TYPE_ID = 3003 ";

            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cmd.Parameters.Clear();

                        while (reader.Read())
                        {
                            ClothingVO cv = new ClothingVO();
                            cv.itemid = reader.GetString("ITEM_ID");
                            cv.itemnm = reader.GetString("NAME");
                            cv.itemdesc = reader.GetString("DESC");
                            cv.buyprice = reader.GetString("BUY_PRI");
                            CltBuyList.Add(cv);
                        }
                    }
                }
            }

            return CltBuyList;
        }

        public void Buyclothing(string itemid)
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
                    cmd.Parameters.AddWithValue("@usbl", "1");
                    cmd.Parameters.AddWithValue("@slot", "Cloth");

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string GetUserInfoFromDB()
        {
            string Usercash = "";
            var sql = "  SELECT CASH " +
                      "   FROM TBL_USERINFO " +
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
                    var sql = " update TBL_USERINFO " +
                              " set CASH = (@payment)" +
                              " where PID = (@pid) ";
                    // DB에 유저 정보 저장
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");
                    cmd.Parameters.AddWithValue("@payment", payment);
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
                    var sql = " update TBL_INVEN " +
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
    }
}