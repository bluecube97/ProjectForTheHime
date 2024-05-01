namespace Script.UI.Outing
{
    using global::System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class QuestBoardManager : MonoBehaviour
    {
        public GameObject questListPrefab; // QuestList 이미지 프리팹 참조
        public GameObject questList; // QuestList 이미지 참조
        public Transform questListLayout; // QuestList들이 들어갈 레이아웃 참조

        private QusetBoardDao questBoardDao;

       

        private List<GameObject> questListInstances = new List<GameObject>();

        private void Start()
        {

            questBoardDao = GetComponent<QusetBoardDao>();
            UpdateQuestList();

        }

        public void UpdateQuestList()
        {
            questList.SetActive(true);

            List<QuestBoardVO> QuestList = questBoardDao.GetQuestBoardList();




            // 기존에 생성된 QuestList 오브젝트들을 제거합니다.
            foreach (GameObject questInstance in questListInstances)
            {
                Destroy(questInstance);
            }
            questListInstances.Clear();

            // 새로운 QuestList 오브젝트를 생성하고 설정합니다.
            int i = 0;
            foreach (var quest in QuestList)
            {
                i++;

                GameObject questListInstance = Instantiate(questListPrefab, questListLayout);
                questListInstance.name = "QuestList" + i;
                questListInstances.Add(questListInstance);

                Text textComponent = questListInstance.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = quest.QuestNo + "." +
                            " : " + quest.QuestNm + "\r\n" +
                            " 내용 : " + quest.QuestMemo;
                }
            }
            questList.SetActive(false);


        }

        public void OnClickSubmit()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            Debug.Log("클릭된 버튼의 부모 오브젝트 이름: " + parentObjectName);

            var QuestList = questBoardDao.GetQuestBoardList();
            string indexString = parentObjectName.Replace("QuestList", "");
            int index = int.Parse(indexString);

            QuestBoardVO quest = QuestList[index - 1];
            int questNo = quest.QuestNo;

            questBoardDao.SubmitQuset(questNo);

            // 업데이트된 정보를 다시 표시합니다.
            UpdateQuestList();
        }

        public void OnClickPlayQuest()
        {
           
            UpdateQuestList();
        }

        public void OnClickSubmitQuest()
        {
        
            UpdateQuestList();
        }

        public void OnClickCompleteQuest()
        {
            
            UpdateQuestList();
        }

        public void OnClickReturn()
        {
            {
                SceneManager.LoadScene("OutingScene");
            }
        }
    }
}