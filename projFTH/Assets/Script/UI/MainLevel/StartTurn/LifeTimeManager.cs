using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI.MainLevel.StartTurn
{
    public class LifeTimeManager : MonoBehaviour
    {
        public string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

        public GameObject todoListPrefab; // TODOList 이미지 프리팹 참조
        public GameObject todoList; // TODOList 이미지 참조
        public Transform todoListLayout; // TODOList들이 들어갈 레이아웃 참조
        public List<Dictionary<string, string>> TodoList = new(); // TODO리스트를 담는 딕셔너리 리스트

        public void Start()
        {
            LoadData();

            foreach (var dic in TodoList)
            {
                // 이미지 프리팹 인스턴스화
                GameObject todoListInstance = Instantiate(todoListPrefab, todoListLayout);

                // 이미지 오브젝트에 딕셔너리 값 설정
                Text textComponent = todoListInstance.GetComponentInChildren<Text>();
                if (textComponent!= null)
                {
                    textComponent.text = "이름: " + dic["USERNAME"] + " 성별: " + dic["USERSEX"];
                }
            }
            todoList.SetActive(false);
        }

        public void OnClickComplete()
        {
            Debug.Log("Complete");
        }

        public void OnClickReturn()
        {
            SceneManager.LoadScene("StartTurnScene");
        }

        private void LoadData()
        {
            using (var connection = new MySqlConnection(con))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM tbl_test";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, string> dic = new();
                            dic.Add("USERNAME", reader["USERNAME"].ToString());
                            dic.Add("USERSEX", reader["USERSEX"].ToString());

                            TodoList.Add(dic);
                        }
                    }
                }
            }
        }
    }
}