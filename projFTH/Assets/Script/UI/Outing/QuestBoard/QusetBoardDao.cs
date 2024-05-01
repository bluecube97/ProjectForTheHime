using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UnityEngine;

public class QusetBoardDao : MonoBehaviour
{
    private string con = "Server=localhost;Database=testdb;Uid=root;Pwd=1234;Charset=utf8mb4";


    public List<QuestBoardVO> GetQuestBoardList()
    {
        List<QuestBoardVO> QuestList = new List<QuestBoardVO>();

        string sql = "SELECT gq.QUESTNO, gq.QUESTNM, gq.QUESTMEMO, gq.SUBMITFALG, gq.COMPLETEFLAG " +
                     "  FROM game_questboard gq ";

        using (MySqlConnection connection = new MySqlConnection(con))
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
                        QuestBoardVO quest = new QuestBoardVO();
                        quest.QuestNo = reader.GetInt32(reader.GetOrdinal("QUESTNO"));
                        quest.QuestNm = reader.GetString(reader.GetOrdinal("QUESTNM"));
                        quest.QuestMemo = reader.GetString(reader.GetOrdinal("QUESTMEMO"));
                        quest.SubmitFlag = reader.GetString(reader.GetOrdinal("SUBMITFALG"));
                        quest.CompleteFlag = reader.GetString(reader.GetOrdinal("COMPLETEFLAG")); 
                        QuestList.Add(quest);
                    }
                }
            }
        }

        return QuestList;
    }

    /*
    public List<Dictionary<string, object>> GetCompleteQuestBoardList()
    {
        List<Dictionary<string, object>> QuestList = new List<Dictionary<string, object>>();

        string sql = "SELECT gq.QUESTNO, gq.QUESTNM, gq.QUESTMEMO " +
                     "  FROM game_questboard gq " +
                     " WHERE COMPLETEFLAG ='Y'" +
                        "AND SUBMITFALG ='Y'";

        using (MySqlConnection connection = new MySqlConnection(con))
        {
            connection.Open();
            using (MySqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("QUESTNO", reader["QUESTNO"]);
                        dic.Add("QUESTNM", reader["QUESTNM"]);
                        dic.Add("QUESTMEMO", reader["QUESTMEMO"]);

                        QuestList.Add(dic);
                    }
                }
            }
        }

        return QuestList;

    }
    */
    public void SubmitQuset(int questNo)
    {
        string sql = "UPDATE game_questboard " +
                       " SET SUBMITFALG ='Y'" +
                     " WHERE QUESTNO = @QuestNo ";
        using (MySqlConnection connection = new MySqlConnection(con))
        {
            connection.Open();
            using (MySqlCommand cmd = connection.CreateCommand())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@QuestNo", questNo);
                cmd.ExecuteNonQuery();
            }
        }

    }
    /*
    public void DeleteQuset(int questNo)
    {
        string sql = "UPDATE game_questboard " +
                       " SET DELETEFLAG ='Y'" +
                     " WHERE QUESTNO = @QuestNo ";
        using (MySqlConnection connection = new MySqlConnection(con))
        {
            connection.Open();
            using (MySqlCommand cmd = connection.CreateCommand())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@QuestNo", questNo);
                cmd.ExecuteNonQuery();
            }
        }

    }
    */
}
