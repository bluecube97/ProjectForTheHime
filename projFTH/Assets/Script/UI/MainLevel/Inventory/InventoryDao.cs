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
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }
        public IEnumerator GetInventoryList(string pid, Action<List<Dictionary<string, object>>> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/inven/list?pid="+pid);
   
            Debug.Log("인벤 리스트 출력 아이디 : "+ pid);
            yield return request.SendWebRequest();
            Debug.Log("12341234");
            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log("인벤 리스트 값 "+json);
                List<Dictionary<string, object>> inventorylist = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                callback(inventorylist);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
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
                            inv.ItemDese = reader.GetString(2);
                            inv.ItemCnt = reader.GetString(3);
                            InvenList.Add(inv);
                        }
                    }
                }
            }

            return InvenList;
        }

        public IEnumerator ItemCraftInserts(string pid, string itemid, string itemcnt)
        {
            string url = "http://localhost:8080/api/inven/create/insert";

            // WWWForm 생성
            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("itemid", itemid);
            form.AddField("itemcnt", itemcnt);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
            }
        }
        public IEnumerator ItemCraftUpdates(string pid, string itemid, string itemcnt)
        {
            string url = "http://localhost:8080/api/inven/create/update";

            // WWWForm 생성
            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("itemid", itemid);
            form.AddField("itemcnt", itemcnt);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
            }
        }
        public IEnumerator ItemCraftPayments(string pid ,string itemid , string itemcnt)
        {
                string url = "http://localhost:8080/api/inven/create/payment";

            // WWWForm 생성
            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("itemid", itemid);
            form.AddField("itemcnt", itemcnt);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
            }
        }
        public IEnumerator UpdateUserCashs(string pid, string payment)
        {
            string url = "http://localhost:8080/api/inven/purchase/payment";

            // WWWForm 생성
            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("payment", payment);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
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
        }
        public IEnumerator UpdateSellThings(string itemcnt, string itemid, string pid)
        {
            WWWForm form = new WWWForm();
            form.AddField("itemcnt", itemcnt);
            form.AddField("itemid", itemid);
            form.AddField("pid",pid);
            using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:8080/api/inven/sell", form))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
            }
        }
        public IEnumerator UpdateBuyThings(string bitem, string itemid, string pid)
        {
            WWWForm form = new WWWForm();
            form.AddField("bitem", bitem);
            form.AddField("itemid", itemid);
            form.AddField("pid",pid);
            using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:8080/api/inven/purchase/update", form))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
            }
        }
        public IEnumerator InsertBuyThings(string itemid, string cnt,string pid)
        {
            string url = "http://localhost:8080/api/inven/purchase/insert";

            // WWWForm 생성
            WWWForm form = new WWWForm();
            form.AddField("pid", pid);
            form.AddField("itemid", itemid);
            form.AddField("cnt", cnt);
            form.AddField("usbl", "0");
            form.AddField("slot", "");

            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
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
    }
}
    

