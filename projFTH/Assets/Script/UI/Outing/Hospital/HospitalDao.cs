using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UnityEngine;

namespace Script.UI.Outing.Hospital
{
    public class HospitalDao : MonoBehaviour
    {
        string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

        public List<Dictionary<string, object>> getSellList()
        {
            List<Dictionary<string, object>> SellList = new();
            var sql = "  select h.itemNo, h.itemNm, h.itemPrice, h.itemdesc " +
                      "  from hositemlist h  ";
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
                            dic.Add("itemNo", reader.GetInt32(0));
                            dic.Add("itemNm", reader.GetString(1));
                            dic.Add("itemPrice", reader.GetInt32(2));
                            dic.Add("itemDesc", reader.GetString(3));

                            SellList.Add(dic);
                        }
                    }
                }
            }

            return SellList;
        }
        public Dictionary<string,object> GetUserInfo()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            var sql = " select gu.USERCASH, gu.userHP, gu.userMaxHP " +
                      " from game_userinfo gu " +
                      "  where gu.SEQ =1";
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
                            dic.Add("userCash", reader.GetInt32(0));
                            dic.Add("userHP", reader.GetInt32(1));
                            dic.Add("userMaxHP", reader.GetInt32(2));

                        }
                    }
                }
            }
            return dic;
        }

  

        public void SetAfterHeal(int payCash, int userMaxHP)
        {
            var sql = " update game_userinfo " +
                      " set USERCASH = @payCash, " +
                      "     userHP = @userHP " +
                      " where SEQ = 1";
            using (MySqlConnection connection = new MySqlConnection(con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                
                    // DB에 유저 정보 저장
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@payCash", payCash);
                    cmd.Parameters.AddWithValue("@userHP", userMaxHP);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void SetBuyAfter(int payCash)
        {
            var sql = " update game_userinfo " +
                      " set USERCASH = @payCash " +
                      " where SEQ = 1";
            using (MySqlConnection connection = new MySqlConnection(con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
              
                    // DB에 유저 정보 저장
                    cmd.Parameters.Clear();

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@payCash", payCash);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
