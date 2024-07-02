using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Script.UI.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.Outing.RestaurantScript
{
    public class RestaurantDao : MonoBehaviour
    {
        private RestaurantManager restaurantManager;
        private ConnDB _connDB;

        public void Awake()
        {
            restaurantManager = GetComponent<RestaurantManager>(); // 현재 게임 오브젝트에 붙어 있는 RestaurantFoodList 스크립트를 가져옴
            _connDB = new ConnDB();
        }
        
        //음식 정보 받아옴
        public List<FoodListVO> GetFoodListFromDB()
        {
            List<FoodListVO> FoodList = new List<FoodListVO> ();
            var sql = "SELECT ITEM_ID, NAME, `DESC`, SELL_PRI, BUY_PRI " +                     
                           "  FROM TBL_ITEM " +                        
                          "  WHERE TYPE_ID = 2000 " ;
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FoodListVO fv = new();
                            fv.FoodNo = (string)reader["ITEM_ID"];
                            fv.FoodNm = (string)reader["DESC"];
                            fv.FoodPr = (string)reader["BUY_PRI"];

                            FoodList.Add(fv);
                        }
                    }
                }
            }
            return FoodList;
        }
        
        //유저 정보 받아오기
        public string  GetUserInfoFromDB()
        {
            string Usercash = "";
            var sql = "  SELECT gu.CASH " +
                      "   FROM TBL_USERINFO gu ";
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
                            Usercash = reader.GetString(0);
                            Debug.Log(Usercash);
                        }
                    }
                }
            }
            return Usercash;
        }
        
        //결제 후 잔액 업데이트
        public void UpdateUserCash(string payment)
        {
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    var sql = " update TBL_USERINFO " +
                              " set CASH = (@payment)" +
                              " where PID = @pid ";
                    // DB에 유저 정보 저장
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@payment", payment);
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");
                    cmd.ExecuteNonQuery();
                }
            }

        }
        public IEnumerator GetFoodList(Action<List<Dictionary<string, object>>> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/outing/restaurant/list");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                List<Dictionary<string, object>> foodlist = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                callback(foodlist);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }

        
    }
}