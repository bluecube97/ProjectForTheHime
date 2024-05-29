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

            string sql = "SELECT gq.QNO, gq.QNM, gq.Q_OBJ ,Q_OCNT,Q_REWARD,Q_REWARD_CNT,gq.QMEMO, gq.SFALG, gq.CFLAG " +
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
                            quest.QuestNo = reader.GetInt32("QNO");
                            quest.QuestNm = reader.GetString("QNM");
                            quest.QuestMemo = reader.GetString("QMEMO");
                            quest.SubmitFlag = reader.GetString("SFALG");
                            quest.CompleteFlag = reader.GetString("CFLAG"); 
                            quest.Qitem = reader.GetString("Q_OBJ");
                            quest.Qitem_cnt = reader.GetString("Q_OCNT");
                            quest.Qreward = reader.GetString("Q_REWARD");
                            quest.Qreward_cnt = reader.GetString("Q_REWARD_CNT");

                            QuestList.Add(quest);
                        }
                    }
                }
            }

            return QuestList;
        }
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
    
       
    }
}
