using Script.UI.Outing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public GameObject InventoryMenu;
    public GameObject inventoryPrefab; // QuestList 이미지 프리팹 참조
    public GameObject inventory; // QuestList 이미지 참조
    public GameObject detailPopup; // TODOList의 세부 정보 팝업창

    public Transform inventorytLayout; // QuestList들이 들어갈 레이아웃 참조
    private List<GameObject> inventoryInstances = new List<GameObject>();

    private InventoryDao inven;
    private List<Dictionary<string, object>> InvenList = new List<Dictionary<string, object>>();

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

    public int ItemNm { get; set; } // TODO의 번호


    private void Start()
    {
        inven = GetComponent<InventoryDao>();
        InvenList = inven.GetInvenList();
        StartInven(InvenList);
    }
    private void Update()
    {
        var popupPosition = Input.mousePosition;
        popupPosition.x += 100;
        popupPosition.y += 100;
        detailPopup.transform.position = popupPosition;
    }
    public void StartInven(List<Dictionary<string, object>> InvenList)
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
                invenInstance.name =  (string)inven["ItemName"];
                inventoryInstances.Add(invenInstance);

                Text textComponent = invenInstance.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    textComponent.text = inven["ItemName"] + 
                                         "\r\n" +
                                         "X"+
                                         inven["Quantity"];
                }
            }
        }
        inventory.SetActive(false);
    }
    public void OnPointerEnter()
    {
        // 현재 마우스가 가리키고 있는 게임 오브젝트 가져옴
        var nowGameObject = EventSystem.current.currentSelectedGameObject;
        // 팝업 활성화
        detailPopup.SetActive(true);
        // 해당 게임 오브젝트의 TodoNo 가져오기
        var ItemNm = nowGameObject.name; // = Convert.ToInt32(nowGameObject.name);
                        // 팝업에 세부 정보 설정
        var popupText = detailPopup.GetComponentInChildren<Text>();
        // TodoList에서 해당 Todo 항목의 정보를 가져옴
        foreach (var dic in InvenList)
        {
            if (dic["ItemName"].Equals(ItemNm))
            {

                // 팝업에 Todo 항목의 세부 정보를 출력
                popupText.text = (string)dic["ItemDescription"];
                break;
            }
        }
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
    public void OnPointerExit()
    {
        // 팝업 비활성화
        detailPopup.SetActive(false);
    }
}

