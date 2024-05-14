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


            string sql = "SELECT CLONO, CLONM, SILKCNT, LINECNT, BUYFLAG" +
                         "  FROM clothing c ";

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
                            cv.cloNo = reader.GetInt32("CLONO");
                            cv.cloNm = reader.GetString("CLONM");
                            cv.slikCnt = reader.GetInt32("SILKCNT");
                            cv.linCnt = reader.GetInt32("LINECNT");
                            cv.buyFlag = reader.GetString("BUYFLAG");
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


            string sql = "  SELECT SEQ, i.ID AS ItemNo, i.Name AS ItemName, i.Description AS ItemDescription, cts.SELLPRICE AS SellPrice " +
                         " FROM clothingsell AS cts " +
                         " INNER JOIN item AS i ON cts.ItemID = i.ID ";

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
                            cv.seq = reader.GetInt32("SEQ");
                            cv.clsNo = reader.GetInt32("ItemNo");
                            cv.clsNm = reader.GetString("ItemName");
                            cv.clsDes = reader.GetString("ItemDescription");
                            cv.clspri = reader.GetInt32("SellPrice");
                            CltBuyList.Add(cv);
                        }
                    }
                }
            }

            return CltBuyList;
        }

        public void Buyclothing(int index)
        {
            string sql = "  UPDATE clothing " +
                         " SET BUYFLAG = 'Y' " +
                         " WHERE CLONO =  @clothingNo";
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@clothingNo", index + 1);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public int GetUserInfoFromDB()
        {
            int Usercash = 0;
            var sql = "  SELECT gu.USERCASH " +
                      "   FROM game_userinfo gu ";
            using (MySqlConnection connection = new(ConnDB.Con))
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
        public void UpdateUserCash(int payment)
        {

            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    var sql = " update game_userinfo " +
                              " set USERCASH = (@payment)" +
                              " where SEQ = 1 ";
                    // DB에 유저 정보 저장
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@payment", payment);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}