using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Script.UI.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.StartLevel.Dao
{
    public class StartLevelDao : MonoBehaviour
    {
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }

        public static void SetUserInfo(string name, string gender)
        {
            const string sql = " INSERT INTO tbl_test (USERNAME, USERSEX) " +
                               " VALUES (@userName, @userSex) ";

            // DB 연결
            using MySqlConnection connection = new(ConnDB.Con);
            connection.Open();
            using MySqlCommand cmd = connection.CreateCommand();
            // DB에 유저 정보 저장
            cmd.CommandText = sql;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@userName", name);
            cmd.Parameters.AddWithValue("@userSex", gender);
            cmd.ExecuteNonQuery();
        }

        public static Dictionary<string, string> GetUserInfo()
        {
            Dictionary<string, string> userInfo = new();

            const string sql = "SELECT tt.USERNAME AS username, tt.USERSEX AS USERSEX " +
                               "  FROM tbl_test tt " +
                               " ORDER BY tt.SEQ DESC LIMIT 1 ";

            using MySqlConnection connection = new(ConnDB.Con);
            connection.Open();
            using MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Clear();

            using MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return userInfo;
            }

            userInfo["username"] = reader.GetString("username");
            userInfo["usersex"] = reader.GetString("usersex");

            return userInfo;
        }
        
        //spring에 있는 session값 들고 오는 구문 
        public IEnumerator GetUserEmail(Action<Dictionary<string, object>> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/user/login");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Dictionary<string, object> userinfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Debug.Log("json : "+json);
                Debug.Log("userinfo : "+ userinfo);
                callback(userinfo);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
        
        //session값이 DB에 저장되어 있는지 확인
        public IEnumerator SearchUserInfo(string email, Action<int> callback)
        {
            string url = "http://localhost:8080/api/user/search";
            WWWForm form = new WWWForm();
            form.AddField("email", email);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        // 서버로부터 반환된 JSON 문자열을 읽어 int로 변환
                        string responseText = request.downloadHandler.text;
                        int userCount = int.Parse(responseText); // 서버에서 반환된 값을 int로 변환
                        callback(userCount);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Failed to parse response: " + ex.Message);
                        callback(-1); // 오류 발생 시 -1 반환
                    }
                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                    callback(-1); // 오류 발생 시 -1 반환
                }
            }
        }
        
        //없다면 값을 insert함
        public IEnumerator InsertUserInfo(string userEmail, string userName, string userSex)
        {
            string url = "http://localhost:8080/api/user/insert";
            WWWForm form = new WWWForm();
            form.AddField("PID", userEmail);
            form.AddField("PNM", userName);
            form.AddField("PSEX", userSex);
            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
            }        
        }
        
        //DB에 저장돠어 있는 user정보 들고옴 
        public IEnumerator GetUser(string userEmail, Action<Dictionary<string, object>> callback)
        {
            string url = "http://localhost:8080/api/user/info";
            WWWForm form = new WWWForm();
            form.AddField("PID", userEmail);
      
            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        // 서버에서 반환된 JSON 문자열을 Dictionary로 변환
                        string jsonResponse = request.downloadHandler.text;
                        Dictionary<string, object> userInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);
                        callback(userInfo);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Failed to parse response: " + ex.Message);
                        callback(null);
                    }
                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                    callback(null);
                }
            }      
        }
        
        public IEnumerator SetDstate(string jsontext)
        {
            const string url = "http://localhost:8080/api/user/dstate";
            string jsonBody = JsonConvert.SerializeObject(jsontext);

            // JSON 데이터를 바이트 배열로 변환
            byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);

            // UnityWebRequest를 사용하여 POST 요청 생성
            UnityWebRequest request = new(url, "Get")
            {
                uploadHandler = new UploadHandlerRaw(jsonToSend),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();
        }
        
    }
}