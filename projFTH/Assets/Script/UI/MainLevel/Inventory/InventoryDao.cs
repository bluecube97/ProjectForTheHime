using MySql.Data.MySqlClient;
using Script.UI.System;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.UI.MainLevel.Inventory
{
    public class InventoryDao : MonoBehaviour
    {
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }

        public List<InventoryVO> GetInvenList()
        {
            List<InventoryVO> InvenList = new();

            string sql = "SELECT i.ITEM_ID AS itemcode, i.NAME AS itemnm, i.DESC AS itemdesc, inv.CNT AS itemcnt " +
                         "   FROM TBL_INVEN AS inv" +
                         " INNER JOIN TBL_ITEM AS i ON inv.ITEM_ID = i.ITEM_ID ";

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
                            InventoryVO inv = new();
                            inv.ItemNo = reader.GetString(0);
                            inv.ItemNm = reader.GetString(1);
                            inv.ItemDese = reader.GetString(2);
                            inv.ItemCnt = reader.GetString(3);
                            InvenList.Add(inv);
                        }
                    }
                }
            }

            return InvenList;
        }

        public void ItemCraftInsert(string itemid, string usbl, string slot)
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
                    cmd.Parameters.AddWithValue("@usbl", usbl);
                    cmd.Parameters.AddWithValue("@slot", slot);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ItemCraftUpdate(string bitem, string itemid)
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

        public void ItemCraftPayment(string gitemid, string result)
        {
            string sql = " UPDATE TBL_INVEN" +
                         "   SET CNT = @result  " +
                         " WHERE ITEM_ID = @gitemid; ";

            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@result", result);
                    cmd.Parameters.AddWithValue("@gitemid", gitemid);

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
    

