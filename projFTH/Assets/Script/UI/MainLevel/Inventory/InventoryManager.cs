using Script.UI.System;
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
            inven = GetComponent<InventoryDao>();
            InvenList = inven.GetInvenList();
            StartInven(InvenList);
        }
        public void StartInven(List<InventoryVO> InvenList)
        {
            inventory.SetActive(true);

            // 기존에 생성된 QuestList 오브젝트들을 제거합니다.
            foreach (GameObject invenInstance in inventoryInstances)
            {
                Destroy(invenInstance);
            }
            inventoryInstances.Clear();
            // 새로운 QuestList 오브젝트를 생성하고 설정합니다.
            foreach (var inven in InvenList)
            {
                {
                    GameObject invenInstance = Instantiate(inventoryPrefab, inventorytLayout);
                    invenInstance.name =  "Inven" + inven.ItemNo;
                    inventoryInstances.Add(invenInstance);

                    Text textComponent = invenInstance.GetComponentInChildren<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = inven.ItemNm + " X " +
                                             inven.ItemCnt + "\r\n" +
                                             inven.ItemDese;
                    }
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

