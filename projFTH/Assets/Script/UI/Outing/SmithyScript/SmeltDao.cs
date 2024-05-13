using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UnityEngine;

namespace Script.UI.Outing.SmithyScript
{
    public class SmeltDao : MonoBehaviour
    {
        private string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

        //구매 리스트 받아오기
        public List<Dictionary<string, object>> GetBuyList()
        {
            List<Dictionary<string, object>> BuyList = new List<Dictionary<string, object>>();

            string sql = "select s.EqNo, s.EqNm, s.EqPrice " +
                         "  from smelt s ";
            using (MySqlConnection connection = new MySqlConnection(con))
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
                            dic.Add("itemNo", reader["EqNo"]);
                            dic.Add("itemNm", reader["EqNm"]);
                            dic.Add("itemPrice", reader["EqPrice"]);


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
            var sql = "select s.EqNo, s.EqNm, s.EqMatNm, s.EqMatCnt " +
                      "  from smelt s ";
            using (MySqlConnection connection = new MySqlConnection(con))
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
                            dic.Add("itemNo", reader["EqNo"]);
                            dic.Add("itemNm", reader["EqNm"]);
                            dic.Add("itemValNm", reader["EqMatNm"]);
                            dic.Add("itemValCnt", reader["EqMatCnt"]);

                            SmeltList.Add(dic);
                        }
                    }
                }
            }
            return SmeltList;
        }
        //결재를 위한 USERINFO에서 보유 현금 들고 오기
        public int GetUserInfoFromDB()
        {
            int Usercash = 0;
            var sql = "  SELECT gu.USERCASH " +
                      "   FROM game_userinfo gu " +
                      " WHERE gu.SEQ = 1";
            using (MySqlConnection connection = new MySqlConnection(con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Usercash = reader.GetInt32(0);
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
            using (MySqlConnection connection = new MySqlConnection(con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    var sql = " update game_userinfo " +
                              " set USERCASH = (@payment)" +
                              " where SEQ = 1 ";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@payment", payment);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
