using Newtonsoft.Json;
using Script.ApiLibrary;
using Script.UI.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.Outing.ClothingStore
{
    public class ClothingDao : MonoBehaviour
    {
        private static WebRequestManager _wrm;

        private void Awake()
        {
            _wrm = FindObjectOfType<WebRequestManager>();
        }

        // 옷 목록을 가져오는 코루틴
        public IEnumerator GetClothingList(Action<List<Dictionary<string, object>>> callback)
        {
            string absoluteUrl = _wrm.GetAbsoluteUrl("api/outing/clothing/list");
            // HTTP GET 요청 생성
            UnityWebRequest request = UnityWebRequest.Get(absoluteUrl);
            // 요청 전송 및 응답 대기
            yield return request.SendWebRequest();

            // 요청이 성공했는지 확인
            if (request.result == UnityWebRequest.Result.Success)
            {
                // 응답 텍스트를 JSON 형식으로 가져오기
                string json = request.downloadHandler.text;
                // JSON 문자열을 리스트로 디시리얼라이즈
                List<Dictionary<string, object>> clothingList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                // 콜백 함수 호출하여 옷 목록 반환
                callback(clothingList);
            }
            else
            {
                // 요청이 실패하면 에러 메시지 출력
                Debug.LogError("Error: " + request.error);
            }
        }

        // 옷 구매하가 버튼 클릭시 담을 목록을 가져오는 코루틴
        public IEnumerator GetClothingBuyList(Action<List<Dictionary<string, object>>> callback)
        {
            string absoluteUrl = _wrm.GetAbsoluteUrl("api/outing/clothing/buy");
            // HTTP GET 요청 생성
            UnityWebRequest request = UnityWebRequest.Get(absoluteUrl);
            // 요청 전송 및 응답 대기
            yield return request.SendWebRequest();

            // 요청이 성공했는지 확인
            if (request.result == UnityWebRequest.Result.Success)
            {
                // 응답 텍스트를 JSON 형식으로 가져오기
                string json = request.downloadHandler.text;
                // JSON 문자열을 리스트로 디시리얼라이즈
                List<Dictionary<string, object>> clothingBuyList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                // 콜백 함수 호출하여 구매 목록 반환
                callback(clothingBuyList);
            }
            else
            {
                // 요청이 실패하면 에러 메시지 출력
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}

       
        /*
        public List<ClothingVO> _GetClothingBuyList()
        {
            List<ClothingVO> CltBuyList = new();


            string sql = "SELECT ti.ITEM_ID, ti.NAME, ti.`DESC`, ti.SELL_PRI, ti.BUY_PRI" +
                         " FROM TBL_ITEM ti " +
                         " WHERE ti.TYPE_ID = 3003 ";

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
                            ClothingVO cv = new();
                            cv.itemid = reader.GetString("ITEM_ID");
                            cv.itemnm = reader.GetString("NAME");
                            cv.itemdesc = reader.GetString("DESC");
                            cv.buyprice = reader.GetString("BUY_PRI");
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

      

        public string _GetUserInfoFromDB()
        {
            string Usercash = "";
            string sql = " SELECT CASH " +
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
                    string sql = " update TBL_USERINFO " +
                                 " set CASH = (@payment)" +
                                 " where PID = (@pid) ";
                    // DB에 유저 정보 저장
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");
                    cmd.Parameters.AddWithValue("@payment", payment);
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
                    string sql = " update TBL_INVEN " +
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
        */

       

