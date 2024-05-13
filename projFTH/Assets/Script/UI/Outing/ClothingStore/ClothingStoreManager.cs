using Script.UI.Outing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClothingStoreManager : MonoBehaviour
{
    public GameObject ClothingPrefab; // 옷 제작 이미지 프리팹 참조
    public GameObject Clothing; // 옷 제작 이미지 참조
    public Transform ClothingLayout; // 제작 리스트들이 들어갈 레이아웃 참조

    public GameObject ClotBuyPrefab; // 옷가게 판매 이미지 프리팹 참조
    public GameObject ClotBuy; // 옷가게 판매 이미지 참조
    public Transform ClotBuyLayout; // 판매 리스트들이 들어갈 레이아웃 참조
    private int Buyprice;
    private readonly List<GameObject> ClotBuyInstances = new();

    private ClothingDao clothingDao;
    private readonly List<GameObject> ClothingInstances = new();

    private List<ClothingVO> clothingList = new();
    private ClothingUIManager clothingUIManager;
    private List<ClothingVO> cltBuyList = new();
    private int index;
    private List<InventoryVO> invenList = new();
    private InventoryDao inventoryDao;
    private int lineCnt;

    private int slikCnt;

    private void Start()
    {
        clothingDao = GetComponent<ClothingDao>();
        inventoryDao = GetComponent<InventoryDao>();
        clothingUIManager = FindObjectOfType<ClothingUIManager>();

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
        foreach (ClothingVO cls in cltBuyList)
        {
            GameObject clotBuyInstance = Instantiate(ClotBuyPrefab, ClotBuyLayout);
            clotBuyInstance.name = "Clothing" + cls.seq;
            ClotBuyInstances.Add(clotBuyInstance);
            Text textComponent = clotBuyInstance.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = cls.clsNm + "\r\n" +
                                     cls.clsDes + "\r\n" +
                                     "가격 : " + cls.clspri;
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

        foreach (ClothingVO clo in clothingList)
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
                                         + "\r\n" + "옷감 요구량" + clo.slikCnt
                                         + "\r\n" + "실 요구량" + clo.linCnt;
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
        // 여러 번의 데이터베이스 액세스를 단일 액세스로 변경
        InventoryVO slik = invenList.Find(p => p.ItemNm.Equals("옷감"));
        InventoryVO line = invenList.Find(p => p.ItemNm.Equals("실"));

        int balSlik = slik.ItemCnt - slikCnt;
        int balLine = line.ItemCnt - lineCnt;

        // 쿼리 결과를 한 번만 사용하도록 변경
        if (balSlik > 0 && balLine > 0)
        {
            inventoryDao.BuyClothing(balSlik, balLine);
            clothingDao.Buyclothing(index);
            clothingUIManager.OnClickBuyComple();
        }
        else
        {
            clothingUIManager.OnClickBuyFail();
        }
    }

    public void BuyThing()
    {
        int userCash = clothingDao.GetUserInfoFromDB();
        int NowCash = userCash - Buyprice;
        Debug.Log("DB 유저 현금 " + userCash);
        Debug.Log("계산 후 금액 " + NowCash);
        if (NowCash > 0)
        {
            clothingDao.UpdateUserCash(NowCash);
            clothingUIManager.OnClickBuyComple();
        }
        else
        {
            Debug.Log("Not enough cash!");
            clothingUIManager.OnClickBuyFail();
        }
    }
}