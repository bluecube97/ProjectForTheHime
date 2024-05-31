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

            string sql = "SELECT gq.QNO, gq.QNM, gq.QMEMO, " +
                         "  gq.Q_OBJ , ti.NAME as QNAME, gq.Q_OCNT, gq.Q_REWARD, ti2.NAME as Q_RNAME, gq.Q_REWARD_CNT, " +
                         "  gq.SFALG, gq.CFLAG" +
                         " FROM TBL_QUEST gq" +
                         " INNER JOIN TBL_ITEM ti " +
                         " ON ti.ITEM_ID = gq.Q_OBJ " +
                         " INNER JOIN TBL_ITEM ti2 " +
                         " ON ti2.ITEM_ID = gq.Q_REWARD ";

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
                            quest.QitemNm = reader.GetString("QNAME");
                            quest.Qitem_cnt = reader.GetString("Q_OCNT");
                            quest.Qreward = reader.GetString("Q_REWARD");
                            quest.QrewardNm = reader.GetString("Q_RNAME");
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

        public void CompletQuset(int questNo)
        {
            string sql = "UPDATE TBL_QUEST " +
                         " SET CFLAG ='Y'" +
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
