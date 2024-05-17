using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Script.UI.System;
using UnityEngine;

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
        public List<int> GetTodoNo(int year, int month)
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
            while (reader.Read()) todoList.Add((int)reader["TODONO"]);

            return todoList;
        }

        // TodoNO를 이용하여 TodoList를 가져와 리스트에 저장
        public List<Dictionary<string, object>> GetTodoList(List<int> noList)
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