using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDao : MonoBehaviour
{
    private string con = "Server=localhost;Database=testdb;Uid=root;Pwd=1234;Charset=utf8mb4";
    public List<Dictionary<string, object>> GetInvenList()
    {
        List < Dictionary<string, object> >InvenList = new List < Dictionary<string, object> >();

        string sql = "SELECT i.Name AS ItemName, i.Description AS ItemDescription, inv.Quantity AS Quantity " +
                      " FROM Inventory AS inv " +
                     " INNER JOIN Item AS i ON inv.ItemID = i.ID ";
        using (MySqlConnection connection = new MySqlConnection(con))
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
                      Dictionary<string,object> dic = new Dictionary<string,object>();
                        dic["ItemName"] = reader["ItemName"];
                        dic["ItemDescription"] = reader["ItemDescription"];
                        dic["Quantity"] = reader["Quantity"];
                        InvenList.Add(dic);
                    }
                }
            }
        }
        return InvenList;
    }
}
