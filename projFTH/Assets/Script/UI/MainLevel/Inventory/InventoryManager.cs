using Script.UI.Outing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public GameObject InventoryMenu;
    public GameObject inventoryPrefab; // QuestList �̹��� ������ ����
    public GameObject inventory; // QuestList �̹��� ����

    public Transform inventorytLayout; // QuestList���� �� ���̾ƿ� ����
    private List<GameObject> inventoryInstances = new List<GameObject>();

    private InventoryDao inven;
    private List<InventoryVO> InvenList = new List<InventoryVO>();

    private void Awake()
    {
        // �ν��Ͻ��� ���� ��� ���� GameObject�� RestaurantManager�� �߰��մϴ�.
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

        // ������ ������ QuestList ������Ʈ���� �����մϴ�.
        foreach (GameObject invenInstance in inventoryInstances)
        {
            Destroy(invenInstance);
        }
        inventoryInstances.Clear();
        // ���ο� QuestList ������Ʈ�� �����ϰ� �����մϴ�.
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

