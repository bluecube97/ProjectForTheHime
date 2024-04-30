using MySql.Data.MySqlClient;
using UnityEngine;

namespace Script.UI.Smithy.Dao
{
    public class SmithyDao : MonoBehaviour
    {
        private readonly string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

        public void SetItemInfo(int itemno, string itemname, int itemprice, int seq)
        {
            var sql = " INSERT INTO smithy (ITEMNO, ITEMNAME, ITEMPRICE, SEQ) " +
                      " VALUES (@itemno, @itemname, @itemprice, @seq) ";
                      

            // DB 연결
            using (MySqlConnection connection = new MySqlConnection(con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    // DB에 유저 정보 저장
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@itemno", itemno);
                    cmd.Parameters.AddWithValue("@itemname", itemname);
                    cmd.Parameters.AddWithValue("@itemprice", itemprice);
                    cmd.Parameters.AddWithValue("@seq", seq);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}