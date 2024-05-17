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
                            inv.ItemDese  = reader.GetString(2);
                            inv.ItemCnt = reader.GetString(3);
                            InvenList.Add(inv);
                        }
                    }
                }
            }
            return InvenList;
        }
        public void BuyClothing(string balSlik, string balLine)
        {
            string sql =  " UPDATE TBL_INVEN" +
                          "   SET CNT = @slik  " +
                          " WHERE ITEM_ID =18; " +
                          " UPDATE TBL_INVEN" +
                          "   SET CNT = @line  " +
                          " WHERE ITEM_ID =19; ";
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@slik", balSlik);
                    cmd.Parameters.AddWithValue("@line", balLine);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
