using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.UI.MainLevel.StartTurn.Dao
{
    public class _StartTurnDao : MonoBehaviour
    {
        private readonly string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

        // 현재 날짜의 연, 월을 입력받아 해당하는 TodoNO를 반환
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

        public List<Dictionary<string, object>> GetTodoDayTime(int todoNo)
        {
            using (var connection = new MySqlConnection(con))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = " SELECT DATE, ROUTINE " +
                                      " FROM tododate " +
                                      " WHERE TODONO = @todono ";

                    List<Dictionary<string, object>> todoDayTime = new();

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@todono", todoNo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> dic = new();
                                dic.Add("DATE", reader["DATE"]);
                                dic.Add("ROUTINE", reader["ROUTINE"]);
                                todoDayTime.Add(dic);
                            }
                    }
                    return todoDayTime;
                }
            }
        }

        public List<Dictionary<string, object>> GetTodoDayTimeList(List<int> noList)
        {
            using (var connection = new MySqlConnection(con))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = " SELECT DATE, ROUTINE " +
                                      " FROM tododate " +
                                      " WHERE TODONO = @todono ";

                    List<Dictionary<string, object>> todoDayTimeList = new();

                    foreach (var todoNo in noList)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@todono", todoNo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> dic = new();
                                dic.Add("DATE", reader["DATE"]);
                                dic.Add("ROUTINE", reader["ROUTINE"]);
                                todoDayTimeList.Add(dic);
                            }
                        }
                    }

                    return todoDayTimeList;
                }
            }
        }

        public List<Dictionary<string, object>> GetTodoDay(List<int> noList)
        {
            using (var connection = new MySqlConnection(con))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = " SELECT DATE " +
                                      " FROM tododate " +
                                      " WHERE TODONO = @todono ";

                    List<Dictionary<string, object>> todoDayList = new();

                    foreach (var todoNo in noList)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@todono", todoNo);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> dic = new();
                                dic.Add("TODONO", todoNo);
                                dic.Add("DATE", reader["DATE"]);
                                todoDayList.Add(dic);
                            }
                        }
                    }

                    return todoDayList;
                }
            }
        }
    }
}