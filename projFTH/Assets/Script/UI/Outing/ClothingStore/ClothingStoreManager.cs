using Script.UI.MainLevel.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI.Outing.ClothingStore
{
    public class ClothingStoreManager : MonoBehaviour
    {
        public GameObject DebugText;

        public GameObject ClothingPrefab; // 옷 제작 이미지 프리팹 참조
        public GameObject Clothing; // 옷 제작 이미지 참조
        public Transform ClothingLayout; // 제작 리스트들이 들어갈 레이아웃 참조

        public GameObject ClotBuyPrefab; // 옷가게 판매 이미지 프리팹 참조
        public GameObject ClotBuy; // 옷가게 판매 이미지 참조
        public Transform ClotBuyLayout; // 판매 리스트들이 들어갈 레이아웃 참조
        private readonly List<GameObject> ClotBuyInstances = new();
        private readonly List<GameObject> ClothingInstances = new();

        private string Buyprice;

        private ClothingDao clothingDao;

        private List<ClothingVO> _clothingList = new();
        private List<Dictionary<string, object>> clothingList = new();
        private ClothingUIManager clothingUIManager;
        private List<Dictionary<string, object>> cltBuyList = new();
        private List<InventoryVO> invenList = new();
        private InventoryDao inventoryDao;
        private string itemid;

        private string reqitem;
        private string reqitem_cnt;


        private void Start()
        {
            clothingDao = GetComponent<ClothingDao>();
            inventoryDao = GetComponent<InventoryDao>();
            clothingUIManager = FindObjectOfType<ClothingUIManager>();

            //clothingList = clothingDao._GetClothingList();
            StartCoroutine(clothingDao.GetClothingList(list =>
            {
                clothingList = list;
                UpdateClothingUI(list);
            }));
            StartCoroutine(clothingDao.GetClothingBuyList(list =>
            {
                cltBuyList = list;
                SetCltBuyList(list);
            }));
            //cltBuyList = clothingDao.GetClothingBuyList();
            //invenList = inventoryDao.GetInvenList();

            //SetClothingList(clothingList);
            //SetCltBuyList(cltBuyList);
        }

        private void SetCltBuyList(List<Dictionary<string, object>> clothingList)
        {
            foreach (GameObject cloteBuyInstance in ClotBuyInstances)
            {
                Destroy(cloteBuyInstance);
            }

            ClotBuyInstances.Clear();
            foreach (Dictionary<string, object> cls in clothingList)
            {
                GameObject clotBuyInstance = Instantiate(ClotBuyPrefab, ClotBuyLayout);
                cls.TryGetValue("itemid", out object itemId);
                clotBuyInstance.name = "Clothing" + itemId;
                ClotBuyInstances.Add(clotBuyInstance);

                Text textComponent = clotBuyInstance.GetComponentInChildren<Text>();

                if (textComponent == null) return;
                cls.TryGetValue("itemnm", out object itemNm);
                cls.TryGetValue("itemdesc", out object itemDesc);
                cls.TryGetValue("buyprice", out object buyPrice);
                textComponent.text = itemNm + "\r\n" +
                                     itemDesc + "\r\n" +
                                     "가격 : " + buyPrice;
            }

            ClotBuy.SetActive(false);
        }

        private void UpdateClothingUI(List<Dictionary<string, object>> clothingList)
        {
            ClearClothingInstances();

            foreach (Dictionary<string, object> clothingData in clothingList)
            {
                Text text = DebugText.GetComponentInChildren<Text>();
                text.text = clothingData["r_itemid"].ToString();
                CreateClothingInstance(clothingData);
            }

            Clothing.SetActive(false);
        }

        private void ClearClothingInstances()
        {
            foreach (GameObject clothingInstance in ClothingInstances)
            {
                Destroy(clothingInstance);
            }

            ClothingInstances.Clear();
        }

        private void CreateClothingInstance(Dictionary<string, object> clothingData)
        {
            GameObject clothingInstance = Instantiate(ClothingPrefab, ClothingLayout);
            clothingData.TryGetValue("r_itemid", out object r_itemid);
            clothingInstance.name = "Clothing" + r_itemid;
            ClothingInstances.Add(clothingInstance);

            Text textComponent = clothingInstance.GetComponentInChildren<Text>();

            if (textComponent == null) return;
            clothingData.TryGetValue("r_name", out object r_name);
            clothingData.TryGetValue("r_desc", out object r_desc);
            clothingData.TryGetValue("req_name", out object req_name);
            clothingData.TryGetValue("req_itemcnt", out object req_itemcnt);
            textComponent.text = $"{r_name}\r\n{r_desc}\n{req_name} : {req_itemcnt} 개";
        }

        public void GetClothingValue()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid = parentObjectName.Replace("Clothing", "");
            Dictionary<string, object> cl = clothingList.Find(p => p["r_itemid"].ToString() == itemid);

            cl.TryGetValue("req_item", out object tempReqItem);
            reqitem = tempReqItem?.ToString();
            cl.TryGetValue("req_itemcnt", out object tempReqItemCnt);
            reqitem_cnt = tempReqItemCnt?.ToString();
        }

        public void GetClotBuyValue()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid = parentObjectName.Replace("Clothing", "");

            Dictionary<string, object> clv = cltBuyList.Find(p => p["itemid"].ToString() == itemid);

            clv.TryGetValue("buyprice", out object tempBuyPrice);
            Buyprice = tempBuyPrice?.ToString();
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
                        string cnt = "1";
                        inventoryDao.ItemCraftInsert(itemid, cnt);
                        clothingUIManager.OnClickBuyComplete();
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
            string userCash = "";
            StartCoroutine(clothingDao.GetUserInfoFromDB(cash =>
            {
                userCash = cash;
            }));
            int nowCash = int.Parse(userCash) - int.Parse(Buyprice);
            InventoryVO buyitem = invenList.Find(p => p.ItemNo.Equals(itemid));
            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + nowCash);
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
                string NowCash = nowCash.ToString();

                if (nowCash > 0)
                {
                    clothingDao.UpdateUserCash(NowCash);
                    inventoryDao.UpdateBuyThing(bitem, itemid);
                    clothingUIManager.OnClickBuyComplete();
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