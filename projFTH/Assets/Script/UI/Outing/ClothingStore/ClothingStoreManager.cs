using Script.UI.MainLevel.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI.Outing.ClothingStore
{
    public class ClothingStoreManager : MonoBehaviour
    {
        public GameObject ClothingPrefab; // 옷 제작 이미지 프리팹 참조
        public GameObject Clothing; // 옷 제작 이미지 참조
        public Transform ClothingLayout; // 제작 리스트들이 들어갈 레이아웃 참조
        private List<GameObject> ClothingInstances = new List<GameObject>();

        public GameObject ClotBuyPrefab; // 옷가게 판매 이미지 프리팹 참조
        public GameObject ClotBuy; // 옷가게 판매 이미지 참조
        public Transform ClotBuyLayout; // 판매 리스트들이 들어갈 레이아웃 참조
        private List<GameObject> ClotBuyInstances = new List<GameObject>();

        private ClothingDao clothingDao;
        private InventoryDao inventoryDao;
        private ClothingUIManager clothingUIManager;

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
            var slik = invenList.Find(p => p.ItemNm.Equals("옷감"));
            var line = invenList.Find(p => p.ItemNm.Equals("실"));

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
}
