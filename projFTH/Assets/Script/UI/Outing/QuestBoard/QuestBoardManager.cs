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
        
        //인벤토리를 담음
        private List<InventoryVO> invenList;
        private List<Dictionary<string,object>> inventoryList;

        //퀘스트 정보를 담음
        private List<QuestBoardVO> questListData;
        private List<Dictionary<string,object>> questdata;

        private string sflag;
        private string cflag;


        private void Start()
        {
            questBoardDao = GetComponent<QusetBoardDao>();
            inventoryDao = GetComponent<InventoryDao>();

            /*questListData = questBoardDao.GetQuestBoardList();
            invenList = inventoryDao.GetInvenList();*/
            StartCoroutine(questBoardDao.GetQuestBoardLists(list =>
            {
                questdata = list;
                // SmeltList 세팅 후 SmeltList 화면에 출력
                StartQuestList(questdata);
            }));

            // 서버에서 인벤토리 데이터 가져오기
            StartCoroutine(inventoryDao.GetInventoryList(list =>
            {
                inventoryList = list;
                // SmeltList 세팅 (인벤토리 데이터 필요)
            }));
        }
        
        //퀘스트보드 진입 시 값을 받아옴
        public void StartQuestList(List<Dictionary<string, object>> questdata)
        {
            //퀘스트 오브젝트 활성화
            questList.SetActive(true);
            //수락한 퀘스르 오브젝트 활성화
            submitQuestList.SetActive(true);
            //완료한 퀘스르 오브젝트 활성화
            completeQuestList.SetActive(true);

            //퀘스트 오브젝트 클리어
            ClearExistingQuestList();

            foreach (var quest in questdata)
            {
                if (quest["submit"].Equals("N") && quest["complete"].Equals("N"))
                {
                    GameObject questListInstance = Instantiate(questListPrefab, questListLayout);
                    questListInstance.name = "QuestList" + quest["questno"];
                    questListInstances.Add(questListInstance);

                    //인벤토리에 있는 퀘스트 요구아이템 정보를 담음
                    Dictionary<string,object> giveitem = inventoryList.Find(p => p["itemid"].ToString().Equals(quest["qitemid"]));
                    
                    //요구 아이템이 없다면 0을 있다면 요구아이템의 갯수를 담음
                    string havecnt = giveitem == null ? "0" : giveitem["itemcnt"].ToString();

                    Text textComponent = questListInstance.GetComponentInChildren<Text>();
                    if (textComponent == null)
                    {
                        return;
                    }
                    quest.TryGetValue("questno", out object questno);
                    quest.TryGetValue("questnm", out object questnm);
                    quest.TryGetValue("questmemo", out object questmemo);
                    quest.TryGetValue("qitemnm", out object qitemnm);
                    quest.TryGetValue("qitemcnt", out object qitemcnt); 
                    quest.TryGetValue("ritemnm", out object ritemnm);
                    quest.TryGetValue("ritemcnt", out object ritemcnt); 
                    textComponent.text = $"{questno}. " +
                                         $": {questnm}\r\n " +
                                         $"내용 : {questmemo}" +
                                         $"\r\n 요구 아이템 : {qitemnm} ( {havecnt} / {qitemcnt} )" +
                                         $"\r\n 보상 아이템 : {ritemnm} {ritemcnt}개";
                }
            }
            
            //퀘스트 오브젝트 비활성화
            questList.SetActive(false);
            //수락한 퀘스르 오브젝트 비활성화
            submitQuestList.SetActive(false);
            //완료한 퀘스르 오브젝트 비활성화
            completeQuestList.SetActive(false);
        }

        //수락한 퀘스트 목록을 띄우는 구문
        public void SubmitQuestList(List<Dictionary<string,object>> questdata)
        {
            //퀘스트 오브젝트 활성화
            questList.SetActive(true);
            //수락한 퀘스르 오브젝트 활성화
            submitQuestList.SetActive(true);
            //완료한 퀘스르 오브젝트 활성화
            completeQuestList.SetActive(true);

            //퀘스트 오브젝트 클리어
            ClearExistingQuestList();


            foreach (var quest in questdata)
            {
                if (quest["submit"].Equals("Y") && quest["complete"].Equals("N"))
                {
                    GameObject submitQuestListInstance = Instantiate(submitQuestListPrefab, submitQuestListLayout);
                    submitQuestListInstance.name = "QuestList" + quest["questno"];
                    submitQuestListInstances.Add(submitQuestListInstance);

                    //인벤토리에 있는 퀘스트 요구아이템 정보를 담음
                    Dictionary<string,object> giveitem = inventoryList.Find(p => p["itemid"].ToString().Equals(quest["qitemid"]));
                    
                    //요구 아이템이 없다면 0을 있다면 요구아이템의 갯수를 담음
                    string havecnt = giveitem == null ? "0" : giveitem["itemcnt"].ToString();

                    Text textComponent = submitQuestListInstance.GetComponentInChildren<Text>();
                    if (textComponent == null)
                    {
                        return;
                    }
                    quest.TryGetValue("questno", out object questno);
                    quest.TryGetValue("questnm", out object questnm);
                    quest.TryGetValue("questmemo", out object questmemo);
                    quest.TryGetValue("qitemnm", out object qitemnm);
                    quest.TryGetValue("qitemcnt", out object qitemcnt); 
                    quest.TryGetValue("ritemnm", out object ritemnm);
                    quest.TryGetValue("ritemcnt", out object ritemcnt); 
                    textComponent.text = $"{questno}. " +
                                         $": {questnm}\r\n " +
                                         $"내용 : {questmemo}" +
                                         $"\r\n 요구 아이템 : {qitemnm} ({qitemcnt} / {havecnt})" +
                                         $"\r\n 보상 아이템 : {ritemnm} {ritemcnt}개";

                }
            }
            //퀘스트 오브젝트 비활성화
            questList.SetActive(false);
            //수락한 퀘스르 오브젝트 비활성화
            submitQuestList.SetActive(false);
            //완료한 퀘스르 오브젝트 비활성화
            completeQuestList.SetActive(false);

        }

        //완료된 퀘스트 목록을 띄우는 메서드
        public void CompleteQuestList(List<Dictionary<string,object>> questdata)
        {
            //퀘스트 오브젝트 활성화
            questList.SetActive(true);
            //수락한 퀘스르 오브젝트 활성화
            submitQuestList.SetActive(true);
            //완료한 퀘스르 오브젝트 활성화
            completeQuestList.SetActive(true);

            //퀘스트 오브젝트 클리어
            ClearExistingQuestList();

            foreach (var quest in questdata)
            {
                if (quest["complete"].Equals("Y"))
                {
                    GameObject completeQuestListInstance = Instantiate(completeQuestListPrefab, completeQuestListLayout);
                    completeQuestListInstance.name = "QuestList" + quest["questno"];
                    completeQuestListInstances.Add(completeQuestListInstance);

                    Text textComponent = completeQuestListInstance.GetComponentInChildren<Text>();
                    if (textComponent == null)
                    {
                        return;
                    }
                    quest.TryGetValue("questno", out object questno);
                    quest.TryGetValue("questnm", out object questnm);
                    quest.TryGetValue("questmemo", out object questmemo);
                    textComponent.text = $"{questno}. " +
                                         $": {questnm}\r\n " +
                                         $"내용 : {questmemo} ";
                    
                }
            }
            //퀘스트 오브젝트 비활성화
            questList.SetActive(false);
            //수락한 퀘스르 오브젝트 비활성화
            submitQuestList.SetActive(false);
            //완료한 퀘스르 오브젝트 비활성화
            completeQuestList.SetActive(false);

        }

        //퀘스트 오브젝트를 정리하는 메서트드
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

        // 퀘스트 수락 버튼 클릭 시 
        public void OnClickSubmit()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            int index = GetQuestIndexFromObjectName(parentObject.name);
            
            sflag = "Y";
            cflag = "N";
            //수락 플래그를 N으로 업데이트
            StartCoroutine(questBoardDao.UpdateFlag(sflag,cflag,index));

            StartCoroutine(questBoardDao.GetQuestBoardLists(list =>
            {
                questdata = list;
                // SmeltList 세팅 후 SmeltList 화면에 출력
                StartQuestList(questdata);
            }));
        }

        //수락한 퀘스트 중 거절 버튼 클릭시
        public void OnClickRefuseSubmit()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            int index = GetQuestIndexFromObjectName(parentObject.name);

            sflag = "N";
            cflag = "N";
            //수락 플래그를 N으로 업데이트
            StartCoroutine(questBoardDao.UpdateFlag(sflag,cflag,index));

            StartCoroutine(questBoardDao.GetQuestBoardLists(list =>
            {
                questdata = list;
                // SmeltList 세팅 후 SmeltList 화면에 출력
                StartQuestList(questdata);
            }));
        }

        public void setCompleteFlag(int questno)
        {
            sflag = "N";
            cflag = "N";
            //수락 플래그를 N으로 업데이트
            StartCoroutine(questBoardDao.UpdateFlag(sflag,cflag,questno));

            StartCoroutine(questBoardDao.GetQuestBoardLists(list =>
            {
                questdata = list;
                // SmeltList 세팅 후 SmeltList 화면에 출력
                StartQuestList(questdata);
            }));
        }

        //완료버튼 클릭 시
        public void OnClickComplete()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            int index = GetQuestIndexFromObjectName(parentObject.name);

            // 완료 할 퀘스트 목록의 값을 qv에 담음
            Dictionary<string,object> qv = questdata.Find(p => 
                p["questno"].Equals(index));
            
            //목록에서 요구 아이템과 아이템 갯수를 담음
            string requiredItemCount = qv["qitemcnt"].ToString();
            string requiredItem = qv["qitemid"].ToString();
            
            //인벤토리에 요구 아이템이 있는지 찾고 값을 담음
            Dictionary<string,object>  iv = 
                inventoryList.Find(p => p["itemid"].Equals(requiredItem));

            //요구 아이템이 있다면
            if (iv != null)
            {
                //보유 아이템 갯수와 요구아이템 갯수 계산
                int remainingCount = int.Parse(iv["itemcnt"].ToString()) - int.Parse(requiredItemCount);
                //계산 된 아이템갯수가  0보다 크면
                if (remainingCount >= 0)
                {
                    //계산된 아이템 갯수로 업테이트하고
                    StartCoroutine(inventoryDao.ItemCraftPayments(qv["qitemid"].ToString(), remainingCount.ToString()));

                    //퀘스르 완료처리를 함
                    setCompleteFlag(index);

                    //퀘스트 보상아이템이 인벤토리에 있는지 확인하고 있으면 그 값을 담음
                    Dictionary<string,object> rewardItem = 
                        inventoryList.Find(p => p["itemid"].ToString().Equals(qv["ritemid"]));
                    
                    //보상아이템이 인벤토리에 있다면
                    if (rewardItem != null)
                    {
                        //그 갯수를 계산하고
                        int updatedCount = int.Parse(qv["ritemcnt"].ToString()) + int.Parse(rewardItem["itemcnt"].ToString());
                        //업데이트함
                        StartCoroutine(inventoryDao.ItemCraftUpdates(qv["ritemid"].ToString(), updatedCount.ToString()));

                    }
                    //없다면
                    else
                    {
                        //insert해줌
                        StartCoroutine(inventoryDao.ItemCraftInserts(qv["ritemid"].ToString(), qv["ritemcnt"].ToString()));

                    }
                    //퀘스트 목록 전체 갱신
                    Start();
                }
                //목표 갯수가 모자라면
                else
                {
                    Debug.Log("Insufficient items.");
                }
            }
            //요구아이템이 아예없다면
            else
            {
                Debug.Log("Required item not found in inventory.");
            }
        }

        //버튼 클릭 시 버튼이름으로 값 갱신
        public void QuestButton()
        {
            //버튼이름을 담음
            string buttonName = OnClickQuestListButton();
            
            //버튼이름이 진행가능한퀘스트 라면
            if (buttonName.Equals("PalybleQuest"))
            {
                //진행가능한 퀘스르 목록을 불러옴
                StartQuestList(questdata);
            }
            //버튼이름이 수락한퀘스트라면
            else if (buttonName.Equals("SubmitQuest"))
            {
                //수락한 퀘스트 목록을 불러옴
                SubmitQuestList(questdata);
            }
            //버튼이름이 완료된 퀘스트라면
            else if (buttonName.Equals("CompleteQuest"))
            {
                //완료된 퀘스트 목록을 불러옴
                CompleteQuestList(questdata);
            }
        }

        //버튼 클릭 시 버튼 오브젝트 이름을 담아 리턴함
        public string OnClickQuestListButton()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            return clickedButton.name;
        }

        //뒤로가기 버튼 클릭 시 
        public void OnClickReturn()
        {
            //외출하기 씬으로 보냄
            SceneManager.LoadScene("OutingScene");
        }

        //퀘스트 목록 번호를 반환하는 구문
        private int GetQuestIndexFromObjectName(string objectName)
        {
            string indexString = objectName.Replace("QuestList", "");
            return int.Parse(indexString);
        }
    }
}
