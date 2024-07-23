using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Script.UI.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.MainLevel.Inventory
{
    public class InventoryDao : MonoBehaviour
    {
        private ConnDB _connDB; // 데이터베이스 연결을 위한 변수

        private void Awake()
        {
            _connDB = new ConnDB(); // ConnDB 인스턴스 생성
        }

        public IEnumerator GetInventoryList(string pid, Action<List<Dictionary<string, object>>> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/inven/list?pid=" + pid); // GET 요청 생성

            Debug.Log("인벤 리스트 출력 아이디 : " + pid);
            yield return request.SendWebRequest(); // 요청 전송
            Debug.Log("12341234");

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text; // 서버로부터 받은 JSON 데이터
                Debug.Log("인벤 리스트 값 " + json);
                List<Dictionary<string, object>> inventorylist = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json); // JSON 데이터를 리스트로 변환
                callback(inventorylist); // 콜백 함수 호출
            }
            else
            {
                Debug.LogError("Error: " + request.error); // 에러 로그 출력
            }
        }

      public IEnumerator ItemCraftInserts(string pid, string itemid, string itemcnt)
        {
            string url = "http://localhost:8080/api/inven/create/insert"; // 서버 URL

            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("itemid", itemid);
            form.AddField("itemcnt", itemcnt);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form)) // POST 요청 생성
            {
                yield return request.SendWebRequest(); // 요청 전송

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error); // 에러 로그 출력
                }
            }
        }

        public IEnumerator ItemCraftUpdates(string pid, string itemid, string itemcnt)
        {
            string url = "http://localhost:8080/api/inven/create/update"; // 서버 URL

            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("itemid", itemid);
            form.AddField("itemcnt", itemcnt);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form)) // POST 요청 생성
            {
                yield return request.SendWebRequest(); // 요청 전송

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error); // 에러 로그 출력
                }
            }
        }

        public IEnumerator ItemCraftPayments(string pid, string itemid, string itemcnt)
        {
            string url = "http://localhost:8080/api/inven/create/payment"; // 서버 URL

            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("itemid", itemid);
            form.AddField("itemcnt", itemcnt);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form)) // POST 요청 생성
            {
                yield return request.SendWebRequest(); // 요청 전송

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error); // 에러 로그 출력
                }
            }
        }

        public IEnumerator UpdateUserCashs(string pid, string payment)
        {
            string url = "http://localhost:8080/api/inven/purchase/payment"; // 서버 URL

            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("payment", payment);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form)) // POST 요청 생성
            {
                yield return request.SendWebRequest(); // 요청 전송

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error); // 에러 로그 출력
                }
            }
        }

        /*public IEnumerator GetUserInfoFromDB(Action<Dictionary<string, object>> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/inven/cash");
            yield return request.SendWebRequest();

            Debug.Log(request);
            Debug.Log(request.ToString());
            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Dictionary<string, object> userinfo = JsonConvert.DeserializeObject<Dictionary<string,object>>(json);
                callback(userinfo);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }*/
       
         public IEnumerator UpdateSellThings(string itemcnt, string itemid, string pid)
        {
            WWWForm form = new WWWForm();
            form.AddField("itemcnt", itemcnt);
            form.AddField("itemid", itemid);
            form.AddField("pid", pid);

            using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:8080/api/inven/sell", form)) // POST 요청 생성
            {
                yield return request.SendWebRequest(); // 요청 전송

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error); // 에러 로그 출력
                }
            }
        }

        public IEnumerator UpdateBuyThings(string bitem, string itemid, string pid)
        {
            WWWForm form = new WWWForm();
            form.AddField("bitem", bitem);
            form.AddField("itemid", itemid);
            form.AddField("pid", pid);

            using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:8080/api/inven/purchase/update", form)) // POST 요청 생성
            {
                yield return request.SendWebRequest(); // 요청 전송

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error); // 에러 로그 출력
                }
            }
        }

        public IEnumerator InsertBuyThings(string itemid, string cnt, string pid)
        {
            string url = "http://localhost:8080/api/inven/purchase/insert"; // 서버 URL

            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("itemid", itemid);
            form.AddField("cnt", cnt);
            form.AddField("usbl", "0");
            form.AddField("slot", "");

            using (UnityWebRequest request = UnityWebRequest.Post(url, form)) // POST 요청 생성
            {
                yield return request.SendWebRequest(); // 요청 전송

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error); // 에러 로그 출력
                }
            }
        }
        
        /*public List<InventoryVO> GetInvenList()
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
         public void ItemCraftInsert( string itemid, string cnt)
        {
            string sql = "  INSERT INTO TBL_INVEN (PID, ITEM_ID , CNT, USBL,USBL_SLOT) " +
                         " values (@pid, @itemid, @cnt, (select USBL from TBL_ITEM where TBL_ITEM.ITEM_ID = @itemid), " +
                         "(SELECT tit.TYPE_NM " +
                         "FROM TBL_ITEM ti  " +
                         "INNER JOIN TBL_ITEM_TYPE tit " +
                           "ON tit.TYPE_ID = ti.TYPE_ID " +
                         "WHERE ti.ITEM_ID = @itemid))";
            using (MySqlConnection connection = new(ConnDB.Con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@pid", "ejwhdms502");
                    cmd.Parameters.AddWithValue("@itemid", itemid);
                    cmd.Parameters.AddWithValue("@cnt", cnt);
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
        }*/
    }
}
    

