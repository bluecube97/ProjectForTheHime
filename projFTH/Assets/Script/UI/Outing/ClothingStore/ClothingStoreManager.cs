using Script.UI.Outing;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;

public class ClothingStoreManager : MonoBehaviour
{
    public GameObject ClothingPrefab; // �� ���� �̹��� ������ ����
    public GameObject Clothing; // �� ���� �̹��� ����
    public Transform ClothingLayout; // ���� ����Ʈ���� �� ���̾ƿ� ����
    private List<GameObject> ClothingInstances = new List<GameObject>();

    public GameObject ClotBuyPrefab; // �ʰ��� �Ǹ� �̹��� ������ ����
    public GameObject ClotBuy; // �ʰ��� �Ǹ� �̹��� ����
    public Transform ClotBuyLayout; // �Ǹ� ����Ʈ���� �� ���̾ƿ� ����
    private List<GameObject> ClotBuyInstances = new List<GameObject>();

    private ClothingDao clothingDao;
    private InventoryDao inventoryDao;
    private ClothingUIController clothingUIController;

    private List<ClothingVO> clothingList = new List<ClothingVO>();
    private List<ClothingVO> cltBuyList = new List<ClothingVO>();
    private List<InventoryVO> invenList = new List<InventoryVO>();

    private int slikCnt = 0;
    private int lineCnt = 0;
    private int index = 0;
    private int Buyprice = 0;

    void Start()
    {
        clothingDao = GetComponent<ClothingDao>();
        inventoryDao = GetComponent<InventoryDao>();
        clothingUIController = FindObjectOfType<ClothingUIController>();

        clothingList = clothingDao.GetClothingList();
        cltBuyList = clothingDao.GetClothingBuyList();
        invenList = inventoryDao.GetInvenList();

        SetClothingList(clothingList);
        SetCltBuyList(cltBuyList);
    }

    private void SetCltBuyList(List<ClothingVO> cltBuyList)
    {
        foreach (GameObject cloteBuyInstance in ClotBuyInstances)
        {
            Destroy(cloteBuyInstance);
        }
        ClotBuyInstances.Clear();
        foreach (var cls in cltBuyList)
        {
            GameObject clotBuyInstance = Instantiate(ClotBuyPrefab, ClotBuyLayout);
            clotBuyInstance.name = "Clothing" + cls.seq;
            ClotBuyInstances.Add(clotBuyInstance);
            Text textComponent = clotBuyInstance.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = cls.clsNm + "\r\n" +
                                     cls.clsDes + "\r\n" +
                                     "���� : " + cls.clspri;
            }
        }
        ClotBuy.SetActive(false);
    }
    private void SetClothingList(List<ClothingVO> clothingList)
    {
        foreach (GameObject clothingInstances in ClothingInstances)
        {
            Destroy(clothingInstances);
        }
        ClothingInstances.Clear();

        foreach (var clo in clothingList)
        {
            if (clo.buyFlag.Equals("N"))
            {

                GameObject clothingInstance = Instantiate(ClothingPrefab, ClothingLayout);
                clothingInstance.name = "Clothing" + clo.cloNo;
                ClothingInstances.Add(clothingInstance);

                Text textComponent = clothingInstance.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    textComponent.text = clo.cloNm
                                        + "\r\n" + "�ʰ� �䱸��" + clo.slikCnt
                                        + "\r\n" + "�� �䱸��" + clo.linCnt;
                }
            }
        }
        Clothing.SetActive(false);
    }
    public void GetClothingValue()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        GameObject parentObject = clickedButton.transform.parent.gameObject;
        string parentObjectName = parentObject.name;
        string indexString = parentObjectName.Replace("Clothing", "");
        index = int.Parse(indexString) - 1;

        ClothingVO clv = new();

        clv = clothingList[index];
        slikCnt = clv.slikCnt;
        lineCnt = clv.linCnt;
        Debug.Log(slikCnt);
    }
    public void GetClotBuyValue()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        GameObject parentObject = clickedButton.transform.parent.gameObject;
        string parentObjectName = parentObject.name;
        string indexString = parentObjectName.Replace("Clothing", "");
        index = int.Parse(indexString) - 1;

        ClothingVO clv = new();
        clv = cltBuyList[index];
        Buyprice = clv.clspri;
        Debug.Log(Buyprice);
    }
        public void BuyClothing()
    {
        // ���� ���� �����ͺ��̽� �׼����� ���� �׼����� ����
        var slik = invenList.Find(p => p.ItemNm.Equals("�ʰ�"));
        var line = invenList.Find(p => p.ItemNm.Equals("��"));

        int balSlik = slik.ItemCnt - slikCnt;
        int balLine = line.ItemCnt - lineCnt;

        // ���� ����� �� ���� ����ϵ��� ����
        if (balSlik > 0 && balLine > 0)
        {
            inventoryDao.BuyClothing(balSlik, balLine);
            clothingDao.Buyclothing(index);
            clothingUIController.OnClickBuyComple();
        }
        else
        {
            clothingUIController.OnClickBuyFail();
        }

       
    }
    public void BuyThing()
    {
        int userCash = clothingDao.GetUserInfoFromDB();
        int NowCash = userCash - Buyprice;
        Debug.Log("DB ���� ���� " + userCash);
        Debug.Log("��� �� �ݾ� " + NowCash);
        if (NowCash > 0)
        {
            clothingDao.UpdateUserCash(NowCash);
            clothingUIController.OnClickBuyComple();
        }
        else
        {
            Debug.Log("Not enough cash!");
            clothingUIController.OnClickBuyFail();

        }
    }
}
