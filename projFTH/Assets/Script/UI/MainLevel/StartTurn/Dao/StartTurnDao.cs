using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace Script.UI.MainLevel.StartTurn.Dao
{
    public class StartTurnDao : MonoBehaviour
    {
        // db 연결 정보
        private readonly string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

        // 현재 날짜의 연, 월을 입력받아 해당하는 TodoNO를 반환하여 리스트에 저장
        public List<int> GetTodoNo(int year, int month)
        {
            using (var connection = new MySqlConnection(con))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = " select distinct TODONO " +
                                      " from tododate " +
                                      " where YEAR = @year and MONTH = @month ";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@month", month);
                    using (var reader = cmd.ExecuteReader())
                    {
                        List<int> todoList = new();
                        while (reader.Read()) todoList.Add((int)reader["TODONO"]);

                        return todoList;
                    }
                }
            }
        }

        // TodoNO를 이용하여 TodoList를 가져와 리스트에 저장
        public List<Dictionary<string, object>> GetTodoList(List<int> noList)
        {
            using (var connection = new MySqlConnection(con))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = " select TODONAME, REWARD, LOSEREWARD, STATREWARD " +
                                      " from todolist " +
                                      " where TODONO = @todono ";

                    List<Dictionary<string, object>> todoList = new();
                    foreach (var num in noList)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@todono", num);
                        using (var reader = cmd.ExecuteReader())
                        {
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
                    }

                    return todoList;
                }
            }
        }
    }
}