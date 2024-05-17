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

        public GameObject ClotBuyPrefab; // 옷가게 판매 이미지 프리팹 참조
        public GameObject ClotBuy; // 옷가게 판매 이미지 참조
        public Transform ClotBuyLayout; // 판매 리스트들이 들어갈 레이아웃 참조
        private readonly List<GameObject> ClotBuyInstances = new();

        private ClothingDao clothingDao;
        private readonly List<GameObject> ClothingInstances = new();

        private List<ClothingVO> clothingList = new();
        private ClothingUIManager clothingUIManager;
        private List<ClothingVO> cltBuyList = new();
        private List<InventoryVO> invenList = new();
        private InventoryDao inventoryDao;
        
        private ClothingVO slik;
        private string Buyprice;
        private ClothingVO line;
        private string itemid;


        private void Start()
        {
            clothingDao = GetComponent<ClothingDao>();
            inventoryDao = GetComponent<InventoryDao>();
            clothingUIManager = FindObjectOfType<ClothingUIManager>();

            clothingList = clothingDao.GetClothingList();
            cltBuyList = clothingDao.GetClothingBuyList();
            invenList = inventoryDao.GetInvenList();

            SetClothingList(clothingList);
            SetCltBuyList(clothingList);
        }

        private void SetCltBuyList(List<ClothingVO> clothingList)
        {
            foreach (GameObject cloteBuyInstance in ClotBuyInstances)
            {
                Destroy(cloteBuyInstance);
            }

            ClotBuyInstances.Clear();
            foreach (ClothingVO cls in clothingList)
            {
                if (cls.typeid.Equals("3003"))
                {
                 GameObject clotBuyInstance = Instantiate(ClotBuyPrefab, ClotBuyLayout);
                    clotBuyInstance.name = "Clothing" + cls.itemid;
                    ClotBuyInstances.Add(clotBuyInstance);
                    Text textComponent = clotBuyInstance.GetComponentInChildren<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = cls.itemnm + "\r\n" +
                                             cls.itemdesc + "\r\n" +
                                             "가격 : " + cls.buyprice;
                    }
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
                if (clo.typeid.Equals("1002"))
                {
                    GameObject clothingInstance = Instantiate(ClothingPrefab, ClothingLayout);
                    clothingInstance.name = "Clothing" + clo.itemid;
                    ClothingInstances.Add(clothingInstance);

                    Text textComponent = clothingInstance.GetComponentInChildren<Text>();
                    
                    if (textComponent != null)
                    {
                        textComponent.text = clo.itemnm
                                             + "\r\n" + "옷감 요구량" + clo.itemnm
                                             + "\r\n" + "실 요구량" + clo.buyprice;
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
            if (clothingList.Find(p => p.itemid.Equals(indexString)) != null)
            {
                slik = clothingList.Find(p => p.itemnm.Equals("옷감"));
                line = clothingList.Find(p => p.itemnm.Equals("실"));
            }

            ClothingVO clv = new();
        }
        public void GetClotBuyValue()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid = parentObjectName.Replace("Clothing", "");
            
            ClothingVO clv = clothingList.Find(p => p.itemid == itemid);
            Buyprice = clv.buyprice;
            Debug.Log(Buyprice);
        }
        public void BuyClothing()
        {
            // 여러 번의 데이터베이스 액세스를 단일 액세스로 변경
            InventoryVO slik = invenList.Find(p => p.ItemNm.Equals("옷감"));
            InventoryVO line = invenList.Find(p => p.ItemNm.Equals("실"));

            int balSlik = int.Parse(slik.ItemCnt);
            int balLine = int.Parse(line.ItemCnt);

            // 쿼리 결과를 한 번만 사용하도록 변경
            if (balSlik > 0 && balLine > 0)
            {
                string _balSlik = balSlik.ToString();
                string _balLine = balLine.ToString();

                inventoryDao.BuyClothing(_balSlik, _balLine);
                clothingDao.Buyclothing(itemid);
                clothingUIManager.OnClickBuyComple();
            }
            else
            {
                clothingUIManager.OnClickBuyFail();
            }
        }

        public void BuyThing()
        {
            string userCash = clothingDao.GetUserInfoFromDB();
            int _NowCash = int.Parse(userCash) - int.Parse(Buyprice);
            string NowCash = _NowCash.ToString();
            
            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + NowCash);
            if (_NowCash > 0)
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