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
        public GameObject questListPrefab; // QuestList image prefab reference
        public GameObject questList; // QuestList image reference
        public Transform questListLayout; // Layout for QuestList
        private List<GameObject> questListInstances = new List<GameObject>();

        public GameObject submitQuestListPrefab; // SubmitQuestList image prefab reference
        public GameObject submitQuestList; // SubmitQuestList image reference
        public Transform submitQuestListLayout; // Layout for SubmitQuestList
        private List<GameObject> submitQuestListInstances = new List<GameObject>();
        
        public GameObject completeQuestListPrefab; // completeQuestList image prefab reference
        public GameObject completeQuestList; // completeQuestList image reference
        public Transform completeQuestListLayout; // Layout for completeQuestList
        private List<GameObject> completeQuestListInstances = new List<GameObject>();

        private InventoryDao inventoryDao;
        private QusetBoardDao questBoardDao;
        private List<InventoryVO> invenList;
        private List<QuestBoardVO> questListData;

        private void Start()
        {
            questBoardDao = GetComponent<QusetBoardDao>();
            inventoryDao = GetComponent<InventoryDao>();

            questListData = questBoardDao.GetQuestBoardList();
            invenList = inventoryDao.GetInvenList();

            StartQuestList(questListData);
        }

        public void StartQuestList(List<QuestBoardVO> questListData)
        {
            questList.SetActive(true);
            submitQuestList.SetActive(true);
            completeQuestList.SetActive(true);

            ClearExistingQuestList();

            foreach (var quest in questListData)
            {
                if (quest.SubmitFlag.Equals("N") && quest.CompleteFlag.Equals("N"))
                {
                    GameObject questListInstance = Instantiate(questListPrefab, questListLayout);
                    questListInstance.name = "QuestList" + quest.QuestNo;
                    questListInstances.Add(questListInstance);

                    InventoryVO giveitem = invenList.Find(p => p.ItemNo.Equals(quest.Qitem));
                    string havecnt = giveitem == null ? "0" : giveitem.ItemCnt;

                    Text textComponent = questListInstance.GetComponentInChildren<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = $"{quest.QuestNo}. : {quest.QuestNm}\r\n 내용 : {quest.QuestMemo}\r\n 요구 아이템 : {quest.QitemNm} ( {havecnt} / {quest.Qitem_cnt} )";
                    }
                }
            }
            questList.SetActive(false);
            submitQuestList.SetActive(false);
            completeQuestList.SetActive(false);

        }

        public void SubmitQuestList(List<QuestBoardVO> questListData)
        {
            questList.SetActive(true);
            submitQuestList.SetActive(true);
            completeQuestList.SetActive(true);

            ClearExistingQuestList();

            foreach (var quest in questListData)
            {
                if (quest.SubmitFlag.Equals("Y") && quest.CompleteFlag.Equals("N"))
                {
                    GameObject submitQuestListInstance = Instantiate(submitQuestListPrefab, submitQuestListLayout);
                    submitQuestListInstance.name = "QuestList" + quest.QuestNo;
                    submitQuestListInstances.Add(submitQuestListInstance);

                    InventoryVO giveitem = invenList.Find(p => p.ItemNo.Equals(quest.Qitem));
                    string havecnt = giveitem == null ? "0" : giveitem.ItemCnt;

                    Text textComponent = submitQuestListInstance.GetComponentInChildren<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = $"{quest.QuestNo}. : {quest.QuestNm}\r\n 내용 : {quest.QuestMemo} 요구 아이템 : {quest.QitemNm} ( {havecnt} / {quest.Qitem_cnt} )";
                    }
                }
            }
            questList.SetActive(false);
            submitQuestList.SetActive(false);
            completeQuestList.SetActive(false);

        }

        public void CompleteQuestList(List<QuestBoardVO> questListData)
        {
            questList.SetActive(true);
            submitQuestList.SetActive(true);
            completeQuestList.SetActive(true);

            ClearExistingQuestList();

            foreach (var quest in questListData)
            {
                if (quest.CompleteFlag.Equals("Y"))
                {
                    GameObject completeQuestListInstance = Instantiate(completeQuestListPrefab, completeQuestListLayout);
                    completeQuestListInstance.name = "QuestList" + quest.QuestNo;
                    completeQuestListInstances.Add(completeQuestListInstance);

                    Text textComponent = completeQuestListInstance.GetComponentInChildren<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = $"{quest.QuestNo}. : {quest.QuestNm}\r\n 내용 : {quest.QuestMemo} ";
                    }
                }
            }
            questList.SetActive(false);
            submitQuestList.SetActive(false);
            completeQuestList.SetActive(false);

        }

        private void ClearExistingQuestList()
        {
            foreach (GameObject questInstance in questListInstances)
            {
                Destroy(questInstance);
            }
            foreach (GameObject submitQuestInstance in submitQuestListInstances)
            {
                Destroy(submitQuestInstance);
            }
            foreach (GameObject completeQuestInstance in completeQuestListInstances)
            {
                Destroy(completeQuestInstance);
            }
            questListInstances.Clear();
            submitQuestListInstances.Clear();
            completeQuestListInstances.Clear();

        }

        public void OnClickSubmit()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            int index = GetQuestIndexFromObjectName(parentObject.name);

            questBoardDao.SubmitQuest(index);
            questListData = questBoardDao.GetQuestBoardList();
            StartQuestList(questListData);
        }

        public void OnClickRefuseSubmit()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            int index = GetQuestIndexFromObjectName(parentObject.name);

            questBoardDao.RefuseSubmitQuest(index);
            questListData = questBoardDao.GetQuestBoardList();
            StartQuestList(questListData);
        }

        public void OnClickComplete()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            int index = GetQuestIndexFromObjectName(parentObject.name);

            QuestBoardVO qv = questListData.Find(vo => vo.QuestNo.Equals(index));
            string requiredItemCount = qv.Qitem_cnt;
            string requiredItem = qv.Qitem;
            InventoryVO iv = invenList.Find(vo => vo.ItemNo.Equals(requiredItem));

            if (iv != null)
            {
                int remainingCount = int.Parse(iv.ItemCnt) - int.Parse(requiredItemCount);
                if (remainingCount >= 0)
                {
                    inventoryDao.ItemCraftPayment(qv.Qitem, remainingCount.ToString());
                    questBoardDao.CompleteQuest(index);

                    InventoryVO rewardItem = invenList.Find(p => p.ItemNo.Equals(qv.Qreward));
                    if (rewardItem != null)
                    {
                        int updatedCount = int.Parse(qv.Qreward_cnt) + int.Parse(rewardItem.ItemCnt);
                        inventoryDao.ItemCraftUpdate(updatedCount.ToString(), qv.Qreward);
                    }
                    else
                    {
                        inventoryDao.ItemCraftInsert(qv.Qreward, qv.Qitem_cnt);
                    }
                    Start();
                }
                else
                {
                    Debug.Log("Insufficient items.");
                }
            }
            else
            {
                Debug.Log("Required item not found in inventory.");
            }
        }

        public void QuestButton()
        {
            string buttonName = OnClickQuestListButton();
            if (buttonName.Equals("PalybleQuest"))
            {
                StartQuestList(questListData);
            }
            else if (buttonName.Equals("SubmitQuest"))
            {
                SubmitQuestList(questListData);
            }
            else if (buttonName.Equals("CompleteQuest"))
            {
                CompleteQuestList(questListData);
            }
        }

        public string OnClickQuestListButton()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            return clickedButton.name;
        }

        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }

        private int GetQuestIndexFromObjectName(string objectName)
        {
            string indexString = objectName.Replace("QuestList", "");
            return int.Parse(indexString);
        }
    }
}
