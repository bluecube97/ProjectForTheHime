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

        private bool playbleQuest = true;
        private bool submitQuest;
        private bool completeQuest;

        private List<GameObject> questListInstances = new List<GameObject>();

        private void Start()
        {

            questBoardDao = GetComponent<QusetBoardDao>();
            UpdateQuestList();

        }

        public void UpdateQuestList()
        {
            questList.SetActive(true);

            List<Dictionary<string, object>> QuestList = new List<Dictionary<string, object>>();

            if (playbleQuest)
            {
                QuestList = questBoardDao.GetQuestBoardList();
            }
            else if (submitQuest)
            {
                QuestList = questBoardDao.GetSubmitQuestBoardList();
            }
            else if (completeQuest)
            {
                QuestList = questBoardDao.GetCompleteQuestBoardList();
            }

            // 기존에 생성된 QuestList 오브젝트들을 제거합니다.
            foreach (GameObject questInstance in questListInstances)
            {
                Destroy(questInstance);
            }
            questListInstances.Clear();

            // 새로운 QuestList 오브젝트를 생성하고 설정합니다.
            int i = 0;
            foreach (var dic in QuestList)
            {
                i++;

                GameObject questListInstance = Instantiate(questListPrefab, questListLayout);
                questListInstance.name = "QuestList" + i;
                questListInstances.Add(questListInstance);

                Text textComponent = questListInstance.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = dic["QUESTNO"] + "." +
                            " : " + dic["QUESTNM"] + "\r\n" +
                            " 내용 : " + dic["QUESTMEMO"];
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

            Dictionary<string, object> QuestInfo = QuestList[index - 1];
            int questNo = (int)QuestInfo["QUESTNO"];

            questBoardDao.SubmitQuset(questNo);

            // 업데이트된 정보를 다시 표시합니다.
            UpdateQuestList();
        }

        public void OnClickPlayQuest()
        {
            playbleQuest = true;
            submitQuest = false;
            completeQuest = false;
            UpdateQuestList();
        }

        public void OnClickSubmitQuest()
        {
            playbleQuest = false;
            submitQuest = true;
            completeQuest = false;
            UpdateQuestList();
        }

        public void OnClickCompleteQuest()
        {
            playbleQuest = false;
            submitQuest = false;
            completeQuest = true;
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