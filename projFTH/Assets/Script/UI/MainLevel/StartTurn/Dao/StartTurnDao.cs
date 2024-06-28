using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Script.UI.System;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Script.UI.MainLevel.StartTurn.Dao
{
    public class StartTurnDao : MonoBehaviour
    {
        // db 연결 정보
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }

        // 현재 날짜의 연, 월을 입력받아 해당하는 TodoNO를 반환하여 리스트에 저장
        public IEnumerator GetTodoNo(int year, int month, Action<List<int>> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/lifetime/todono/" + year + "/" + month);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log("json " + json);
                List<int> todoNoList = JsonConvert.DeserializeObject<List<int>>(json);
                callback(todoNoList);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }

        // 현재 날짜의 연, 월을 입력받아 해당하는 TodoNO를 반환하여 리스트에 저장
        public List<int> _GetTodoNo(int year, int month)
        {
            using MySqlConnection connection = new(ConnDB.Con);
            connection.Open();
            using MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = " select distinct TODONO " +
                              " from tododate " +
                              " where YEAR = @year and MONTH = @month ";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@month", month);
            using MySqlDataReader reader = cmd.ExecuteReader();
            List<int> todoList = new();
            while (reader.Read())
            {
                todoList.Add((int)reader["TODONO"]);
            }

            return todoList;
        }

        // TodoNO를 이용하여 TodoList를 가져와 리스트에 저장
        public IEnumerator GetTodoList(List<int> list, Action<List<Dictionary<string, object>>> callback)
        {
            const string url = "http://localhost:8080/api/lifetime/todolist";
            string jsonBody = JsonConvert.SerializeObject(list);

            // JSON 데이터를 바이트 배열로 변환
            byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);

            // UnityWebRequest를 사용하여 POST 요청 생성
            UnityWebRequest request = new(url, "POST")
            {
                uploadHandler = new UploadHandlerRaw(jsonToSend),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            // 요청 결과 처리
            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                List<Dictionary<string, object>> todoNoList =
                    JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                callback(todoNoList);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }

        // TodoNO를 이용하여 TodoList를 가져와 리스트에 저장
        public List<Dictionary<string, object>> _GetTodoList(List<int> noList)
        {
            using MySqlConnection connection = new(ConnDB.Con);
            connection.Open();
            using MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = " select TODONAME, REWARD, LOSEREWARD, STATREWARD " +
                              " from todolist " +
                              " where TODONO = @todono ";

            List<Dictionary<string, object>> todoList = new();
            foreach (int num in noList)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@todono", num);
                using MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Dictionary<string, object> dic = new();
                    dic.Add("TODONAME", reader["TODONAME"]);
                    dic.Add("REWARD", reader["REWARD"]);
                    dic.Add("LOSEREWARD", reader["LOSEREWARD"]);
                    dic.Add("STATREWARD", reader["STATREWARD"]);
                    dic.Add("TODONO", num);
                    todoList.Add(dic);
                }
            }

            return todoList;
        }
    }
}