namespace Script.UI.Outing
{
    using global::System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using UnityEngine.UIElements;

    public class QuestBoardManager : MonoBehaviour
    {
        public GameObject questListPrefab; // QuestList 이미지 프리팹 참조
        public GameObject questList; // QuestList 이미지 참조
        public Transform questListLayout; // QuestList들이 들어갈 레이아웃 참조
        private List<GameObject> questListInstances = new List<GameObject>();

        public GameObject SubmitquestListPrefab; // SubmitQuestList 이미지 프리팹 참조
        public GameObject SubmitquestList; // SubmitQuestList 이미지 참조
        public Transform SubmitquestListLayout; // SubmitQuestList들이 들어갈 레이아웃 참조
        private List<GameObject> SubmitquestListInstances = new List<GameObject>();

        private QusetBoardDao questBoardDao;
        private List<QuestBoardVO> QuestList = new List<QuestBoardVO>();
        private void Start()
        {
            questBoardDao = GetComponent<QusetBoardDao>();
            QuestList = questBoardDao.GetQuestBoardList();

            StartQuestList(QuestList);

        }
        public void StartQuestList(List<QuestBoardVO> QuestList)
        {
            questList.SetActive(true);
            SubmitquestList.SetActive(true);

            // 기존에 생성된 QuestList 오브젝트들을 제거합니다.
            foreach (GameObject questInstance in questListInstances)
            {
                Destroy(questInstance);
            }
            foreach (GameObject submitquestInstance in SubmitquestListInstances)
            {
                Destroy(submitquestInstance);
            }
            questListInstances.Clear();
            SubmitquestListInstances.Clear();
            // 새로운 QuestList 오브젝트를 생성하고 설정합니다.


            foreach (var quest in QuestList)
            {
                if (quest.SubmitFlag.Equals("N"))
                {
                    {
                        GameObject questListInstance = Instantiate(questListPrefab, questListLayout);
                        questListInstance.name = "QuestList" + quest.QuestNo;
                        questListInstances.Add(questListInstance);

                        Text textComponent = questListInstance.GetComponentInChildren<Text>();

                        if (textComponent != null)
                        {
                            textComponent.text = quest.QuestNo + "." +
                                         " : " + quest.QuestNm + "\r\n" +
                                    " 내용 : " + quest.QuestMemo;
                        }
                    }
                }
            }
            questList.SetActive(false);
            SubmitquestList.SetActive(false);
        }
        public void UpdateQuestList(List<QuestBoardVO> QuestList)
        {
            questList.SetActive(true);
            SubmitquestList.SetActive(true);

            // 기존에 생성된 QuestList 오브젝트들을 제거합니다.
            foreach (GameObject questInstance in questListInstances)
            {
                Destroy(questInstance);
            }
            foreach (GameObject submitquestInstance in SubmitquestListInstances)
            {
                Destroy(submitquestInstance);
            }
            questListInstances.Clear();
            SubmitquestListInstances.Clear();
            // 새로운 QuestList 오브젝트를 생성하고 설정합니다.


            foreach (var quest in QuestList)
            {
                if (!quest.SubmitFlag.Equals("N"))
                {
                    {
                        GameObject SubmitquestListInstance = Instantiate(SubmitquestListPrefab, SubmitquestListLayout);
                        SubmitquestListInstance.name = "QuestList" + quest.QuestNo;
                        SubmitquestListInstances.Add(SubmitquestListInstance);

                        Text textComponent = SubmitquestListInstance.GetComponentInChildren<Text>();

                        if (textComponent != null)
                        {
                            textComponent.text = quest.QuestNo + "." +
                                         " : " + quest.QuestNm + "\r\n" +
                                    " 내용 : " + quest.QuestMemo;
                        }
                    }
                }
            }
            questList.SetActive(false);
            SubmitquestList.SetActive(false);
        }

        public void OnClickSubmit()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            string indexString = parentObjectName.Replace("QuestList", "");
            int index = int.Parse(indexString);

            questBoardDao.SubmitQuset(index);
            // 업데이트된 정보를 다시 표시합니다.
            StartQuestList(QuestList);
            SceneManager.LoadScene("QuestBoardScene");

        }
        public void OnClickRefuseSubmit()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            string indexString = parentObjectName.Replace("QuestList", "");
            int index = int.Parse(indexString);
            questBoardDao.RefuseSubmitQuset(index);
            // 업데이트된 정보를 다시 표시합니다.
            StartQuestList(QuestList);
            SceneManager.LoadScene("QuestBoardScene");

        }
        public void QuestButton()
        {
            string ButtonNm = OnClickQuestListButton();
            if (ButtonNm.Equals("PalybleQuest"))
            {
                StartQuestList(QuestList);
            }
            else if (ButtonNm.Equals("SubmitQuest"))
            {
                UpdateQuestList(QuestList);
            }
        }
        public string OnClickQuestListButton()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            string ChiceBotton = clickedButton.name;
            return ChiceBotton;
        }
        public void OnClickReturn()
        {
            {
                SceneManager.LoadScene("OutingScene");
            }
        }
    }
}