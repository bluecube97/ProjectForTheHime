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

        // 객체가 초기화될 때 호출되는 메서드
        private void Awake()
        {
            _connDB = new ConnDB(); // 데이터베이스 연결 객체 생성
        }

        /*public static void SetUserInfo(string name, string gender)
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
        }*/

        // Spring에서 세션 값을 가져오는 메서드
        public IEnumerator GetUserEmail(Action<Dictionary<string, object>> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/user/login");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // JSON 응답을 딕셔너리로 변환
                string json = request.downloadHandler.text;
                Dictionary<string, object> userinfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Debug.Log("json : " + json);
                Debug.Log("userinfo : " + userinfo);
                callback(userinfo); // 콜백 함수 호출
            }
            else
            {
                Debug.LogError("Error: " + request.error); // 오류 로그 출력
            }
        }

        // 세션 값이 DB에 저장되어 있는지 확인하는 메서드
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
                        callback(userCount); // 콜백 함수 호출
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

        // 세션 값이 없으면 DB에 값을 추가하는 메서드
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
                    Debug.LogError("Error: " + request.error); // 오류 로그 출력
                }
            }
        }

        // DB에 저장된 사용자 정보를 가져오는 메서드
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
                        callback(userInfo); // 콜백 함수 호출
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Failed to parse response: " + ex.Message);
                        callback(null); // 오류 발생 시 null 반환
                    }
                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                    callback(null); // 오류 발생 시 null 반환
                }
            }
        }

        // 사용자 상태를 설정하는 메서드
        public IEnumerator SetDstats(string pid, string jsontext)
        {
            string url = $"http://localhost:8080/api/user/dstats?pid={pid}";

            // JSON 데이터를 바이트 배열로 변환
            byte[] jsonToSend = Encoding.UTF8.GetBytes(jsontext);

            // UnityWebRequest를 사용하여 POST 요청 생성
            UnityWebRequest request = new(url, "POST")
            {
                uploadHandler = new UploadHandlerRaw(jsonToSend),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            // 응답 확인
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}"); // 오류 로그 출력
            }
            else
            {
                Debug.Log($"Response: {request.downloadHandler.text}"); // 응답 로그 출력
            }
        }
    }
}
