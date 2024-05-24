using MySql.Data.MySqlClient;
using Script.UI.System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.UI.Outing.QuestBoard
{
    public class QusetBoardDao : MonoBehaviour
    {
        private ConnDB _connDB;

        private void Awake()
        {
            _connDB = new ConnDB();
        }

        public List<QuestBoardVO> GetQuestBoardList()
        {
            List<QuestBoardVO> QuestList = new();

            string sql = "SELECT gq.QNO, gq.QNM, gq.QMEMO, gq.SFALG, gq.CFLAG " +
                         "  FROM TBL_QUEST gq ";

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
                            QuestBoardVO quest = new QuestBoardVO();
                            quest.QuestNo = reader.GetInt32(reader.GetOrdinal("QNO"));
                            quest.QuestNm = reader.GetString(reader.GetOrdinal("QNM"));
                            quest.QuestMemo = reader.GetString(reader.GetOrdinal("QMEMO"));
                            quest.SubmitFlag = reader.GetString(reader.GetOrdinal("SFALG"));
                            quest.CompleteFlag = reader.GetString(reader.GetOrdinal("CFLAG")); 
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
            string sql = "UPDATE TBL_QUEST " +
                         " SET SFALG ='Y'" +
                         " WHERE QNO = @QuestNo ";
            using (MySqlConnection connection = new(ConnDB.Con))
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
        public void RefuseSubmitQuset(int questNo)
        {
            string sql = "UPDATE TBL_QUEST " +
                         " SET SFALG ='N'" +
                         " WHERE QNO = @QuestNo ";
            using (MySqlConnection connection = new(ConnDB.Con))
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
}
