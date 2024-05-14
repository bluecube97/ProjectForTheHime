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

            string sql = "SELECT i.ID AS ItemNo, i.Name AS ItemName, i.Description AS ItemDescription, inv.Quantity AS Quantity " +
                         "   FROM inventory AS inv" +
                         " INNER JOIN item AS i ON inv.ItemID = i.ID ";

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
                            inv.ItemNo = reader.GetInt32(0);
                            inv.ItemNm = reader.GetString(1);
                            inv.ItemDese  = reader.GetString(2);
                            inv.ItemCnt = reader.GetInt32(3);
                            InvenList.Add(inv);
                        }
                    }
                }
            }
            return InvenList;
        }
        public void BuyClothing(int balSlik, int balLine)
        {
            string sql =  " UPDATE inventory" +
                          "   SET Quantity = @slik  " +
                          " WHERE ItemID =18; " +
                          " UPDATE inventory" +
                          "   SET Quantity = @line  " +
                          " WHERE ItemID =19; ";
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
