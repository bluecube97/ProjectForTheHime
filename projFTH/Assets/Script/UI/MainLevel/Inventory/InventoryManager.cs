using Script.UI.StartLevel.Dao;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.MainLevel.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        private static InventoryManager instance;
        public GameObject InventoryMenu;
        public GameObject inventoryPrefab; // QuestList 이미지 프리팹 참조
        public GameObject inventory; // QuestList 이미지 참조

        public Transform inventorytLayout; // QuestList들이 들어갈 레이아웃 참조
        private List<GameObject> inventoryInstances = new List<GameObject>();

        private InventoryDao inven;
        private List<InventoryVO> InvenList = new List<InventoryVO>();
        private List<Dictionary<string,object>> inventoryList = new List<Dictionary<string,object>>();

        private Dictionary<string, object> userinfo = new();
        private StartLevelDao _sld; // StartLevelDao를 사용하기 위한 변수
        private string pid;

        
        private void Awake()
        {
            // 인스턴스가 없을 경우 현재 GameObject에 RestaurantManager를 추가합니다.
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject );
            }
        }
        public static InventoryManager Instance => instance;



        private void Start()
        {            
            _sld = GetComponent<StartLevelDao>();
            inven = GetComponent<InventoryDao>();
            StartCoroutine(_sld.GetUserEmail(info =>
            {
                userinfo = info;
                pid = userinfo["useremail"].ToString();
                StartCoroutine(inven.GetInventoryList(pid, list =>
                {
                    inventoryList = list;
                    StartInven(inventoryList);
                }));
            }));
        }
        public void StartInven(List<Dictionary<string,object>> inventoryList)
        {
            inventory.SetActive(true);

            // 기존에 생성된 QuestList 오브젝트들을 제거합니다.
            foreach (GameObject invenInstance in inventoryInstances)
            {
                Destroy(invenInstance);
            }
            inventoryInstances.Clear();
            // 새로운 QuestList 오브젝트를 생성하고 설정합니다.
            foreach (var inven in inventoryList)
            {
                {
                    GameObject invenInstance = Instantiate(inventoryPrefab, inventorytLayout);
                    invenInstance.name =  "Inven" + inven["itemid"];
                    inventoryInstances.Add(invenInstance);

                    Text textComponent = invenInstance.GetComponentInChildren<Text>();
                    if (textComponent == null)
                    {
                        return;
                    }
                    inven.TryGetValue("itemnm", out object itemnm);
                    inven.TryGetValue("itemcnt", out object itemcnt);
                    inven.TryGetValue("itemdesc", out object itemdesc);
                    textComponent.text = itemnm + " X " +
                                         itemcnt + "\r\n" +
                                         itemdesc;
                }
            }
            inventory.SetActive(false);
        }
   
        public void OnClickInventory()
        {
            ActivateMenu(InventoryMenu);
        }
        public void OnClickInverntoryOut()
        {
            DeactivateMenu(InventoryMenu);
        }
        private void ActivateMenu(GameObject InventoryMenu)
        {
            InventoryMenu.SetActive(true);
        }
        private void DeactivateMenu(GameObject InventoryMenu)
        {
            InventoryMenu.SetActive(false);
        }
    
    }
}

