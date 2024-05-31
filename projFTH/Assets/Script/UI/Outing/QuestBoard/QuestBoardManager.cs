using Script.UI.MainLevel.Inventory;

namespace Script.UI.Outing.QuestBoard

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
        private List<GameObject> questListInstances = new List<GameObject>();

        public GameObject SubmitquestListPrefab; // SubmitQuestList 이미지 프리팹 참조
        public GameObject SubmitquestList; // SubmitQuestList 이미지 참조
        public Transform SubmitquestListLayout; // SubmitQuestList들이 들어갈 레이아웃 참조
        private List<GameObject> SubmitquestListInstances = new List<GameObject>();

        private InventoryDao inventoryDao;
        private QusetBoardDao questBoardDao;
        private List<InventoryVO> invenList;
        private List<QuestBoardVO> QuestList;
        private void Start()
        {
            questBoardDao = GetComponent<QusetBoardDao>();
            inventoryDao = GetComponent<InventoryDao>();

            QuestList = questBoardDao.GetQuestBoardList();
            invenList = inventoryDao.GetInvenList();

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
                        InventoryVO giveitem = invenList.Find(p => p.ItemNo.Equals(quest.Qitem));
                        string havecnt = giveitem == null ? "0" : giveitem.ItemCnt;
                        Text textComponent = questListInstance.GetComponentInChildren<Text>();
                        if (textComponent != null)
                        {
                            textComponent.text = quest.QuestNo + "." +
                                         " : " + quest.QuestNm + "\r\n" +
                                    " 내용 : " + quest.QuestMemo+ "\r\n" +
                                         "요구 아이템 : "+ quest.QitemNm + "( "+ havecnt+ " / "+quest.Qitem_cnt+" )";
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
                        InventoryVO giveitem = invenList.Find(p => p.ItemNo.Equals(quest.Qitem));
                        string havecnt = giveitem == null ? "0" : giveitem.ItemCnt;
                        if (textComponent != null)
                        {
                            textComponent.text = quest.QuestNo + "." +
                                                 " : " + quest.QuestNm + "\r\n" +
                                                 " 내용 : " + quest.QuestMemo+ 
                                                 "요구 아이템 : "+ quest.QitemNm + "( "+ havecnt+ " / "+quest.Qitem_cnt+" )";
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
            QuestList = questBoardDao.GetQuestBoardList();

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
            QuestList = questBoardDao.GetQuestBoardList();

        }

        public void OnClickComple()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            string indexString = parentObjectName.Replace("QuestList", "");
            int index = int.Parse(indexString);
            QuestBoardVO qv = QuestList.Find(vo =>vo.QuestNo.Equals(index) );
            string _qtem = qv.Qitem_cnt;
            string reqtem = qv.Qitem;
            InventoryVO iv = invenList.Find(vo => vo.ItemNo.Equals(reqtem));
            InventoryVO checkVal = invenList.Find(p => p.ItemNo.Equals(qv.Qreward));

            if (iv == null)
            {
                Debug.Log("응 없어");
                
            }
            else
            {
                int _result = int.Parse(iv.ItemCnt)-int.Parse(_qtem);
                if (_result > 0)
                {
                    string result = _result.ToString();
                    inventoryDao.ItemCraftPayment(qv.Qitem, result);
                    questBoardDao.CompletQuset(index);
                    if (checkVal != null)
                    {
                        int upcnt = int.Parse(qv.Qreward_cnt) + int.Parse(iv.ItemCnt);
                        string _upcnt = upcnt.ToString();
                        inventoryDao.ItemCraftUpdate(_upcnt,qv.Qreward);
                    }
                    else
                    {
                        inventoryDao.ItemCraftInsert(qv.Qreward,qv.Qitem_cnt);
                    }
                    
                }
            }
           
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