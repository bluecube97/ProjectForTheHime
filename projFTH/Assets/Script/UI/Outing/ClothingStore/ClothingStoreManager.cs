using Script.UI.MainLevel.Inventory;
using System;
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

        private string reqitem;
        private string reqitem_cnt;

        private string Buyprice;
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
            SetCltBuyList(cltBuyList);
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
                {
                    GameObject clothingInstance = Instantiate(ClothingPrefab, ClothingLayout);
                    clothingInstance.name = "Clothing" + clo.r_itemid;
                    ClothingInstances.Add(clothingInstance);

                    Text textComponent = clothingInstance.GetComponentInChildren<Text>();

                    if (textComponent != null)
                    {
                        textComponent.text = clo.r_name
                                             + "\r\n" + clo.r_desc
                                             + "\n" + clo.req_name + " : " + clo.req_itemcnt + " 개";
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
            itemid = parentObjectName.Replace("Clothing", "");
            ClothingVO cl = clothingList.Find(p => p.r_itemid == itemid);

            reqitem = cl.req_item;
            reqitem_cnt = cl.req_itemcnt;
        }

        public void GetClotBuyValue()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid = parentObjectName.Replace("Clothing", "");

            ClothingVO clv = cltBuyList.Find(p => p.itemid == itemid);
            Buyprice = clv.buyprice;
            Debug.Log(Buyprice);
        }

        public void BuyClothing()
        {
            // 여러 번의 데이터베이스 액세스를 단일 액세스로 변경
            InventoryVO giveitem = invenList.Find(p => p.ItemNo.Equals(reqitem));
            InventoryVO checkVal = invenList.Find(p => p.ItemNo.Equals(itemid));

            if (giveitem == null)
            {
                clothingUIManager.OnClickBuyFail();
            }
            else
            {
                string gitemid = giveitem.ItemNo;
                string _gitemcnt = giveitem.ItemCnt;
                int gitemcnt = int.Parse(_gitemcnt);
                int ritemcnt = int.Parse(reqitem_cnt);
                int resuit = gitemcnt - ritemcnt;

                // 쿼리 결과를 한 번만 사용하도록 변경
                if (resuit >= 0)
                {
                    if (checkVal != null)
                    {
                        string _cnt = checkVal.ItemCnt;
                        int cnt = int.Parse(_cnt);
                        int _uitem = cnt + 1;
                        string uitem = _uitem.ToString();
                        inventoryDao.ItemCraftUpdate(uitem, itemid);
                    }
                    else
                    {
                        string _result = resuit.ToString();
                        inventoryDao.ItemCraftPayment(gitemid, _result);
                        string usbl = "1";
                        string slot = "Cloth";
                        inventoryDao.ItemCraftInsert(itemid,usbl,slot);
                        clothingUIManager.OnClickBuyComple();
                        invenList = inventoryDao.GetInvenList();
                    }
                 

                }
                else
                {
                    clothingUIManager.OnClickBuyFail();
                }
            }
        }

        public void BuyThing()
        {
            string userCash = clothingDao.GetUserInfoFromDB();
            int _NowCash = int.Parse(userCash) - int.Parse(Buyprice);
            InventoryVO buyitem = invenList.Find(p => p.ItemNo.Equals(itemid));
            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + _NowCash);
            if (buyitem == null)
            {
                inventoryDao.InsertBuyThing(itemid);
                invenList = inventoryDao.GetInvenList();

            }
            else
            {
                string _item = buyitem.ItemCnt;
                int item = int.Parse(_item);
                string bitem = (item + 1).ToString();
                string NowCash = _NowCash.ToString();
           
                if (_NowCash > 0)
                {
                    clothingDao.UpdateUserCash(NowCash);
                    inventoryDao.UpdateBuyThing(bitem, itemid);
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
}