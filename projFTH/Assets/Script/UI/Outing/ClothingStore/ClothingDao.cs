using MySql.Data.MySqlClient;
using Script.UI.System;
using System.Collections.Generic;
using UnityEngine;

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
            string sql = "SELECT ti.ITEM_ID, ti.NAME, ti.`DESC`, ti.SELL_PRI, ti.BUY_PRI, ti.TYPE_ID " + 
                          " FROM TBL_ITEM ti " +
                         " WHERE ti.TYPE_ID = 1002 " +
                         "    OR ti.TYPE_ID = 3003";

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
                            cv.sellprice = reader.GetString("SELL_PRI");
                            cv.buyprice = reader.GetString("BUY_PRI");
                            cv.typeid = reader.GetString("TYPE_ID");

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
                         " WHERE ti.TYPE_ID = 1002 ";

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
                              " where PID = (@pid) " ;
                    // DB에 유저 정보 저장
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");
                    cmd.Parameters.AddWithValue("@payment", payment);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}