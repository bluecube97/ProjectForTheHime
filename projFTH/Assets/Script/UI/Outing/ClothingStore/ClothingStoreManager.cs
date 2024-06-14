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

        //구매가격을 담는 전역 변수
        private string Buyprice;

        //DAO호출을 함
        private ClothingDao clothingDao;

        private List<ClothingVO> _clothingList = new();

        //옷 만들기에 들어가는 아이템을 담음
        private List<Dictionary<string, object>> clothingList = new();

        //UI매니저 호출을 함
        private ClothingUIManager clothingUIManager;

        //구매하기에 들어가는 아이템을 담음
        private List<Dictionary<string, object>> cltBuyList = new();

        //인벤토리 값을 담음
        private List<InventoryVO> invenList = new();

        private InventoryDao inventoryDao;

        //제작하거나 구매하는 아이템코드를 담음
        private string itemid;

        //옷 제작 시 필요한 아이템코드와 갯수를 담음
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

        //옷가게에서 구매 목록을 세팅하는 메서드
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

        //물품 제작 시 구매물품 itemid와 요구itemid와 요구아이템에 대한 갯수를 받아오는 구문
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

        //물품 구매 시 구매물품 itemid를 받아오는 구문
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

        //옷 구매하는 구문
        public void BuyClothing()
        {
            // 여러 번의 데이터베이스 액세스를 단일 액세스로 변경

            //인벤토리에 요구아이템이 있는지 찾음
            InventoryVO giveitem = invenList.Find(p => p.ItemNo.Equals(reqitem));
            //제작 아이템이 인벤토리에 있는지 확인
            InventoryVO checkVal = invenList.Find(p => p.ItemNo.Equals(itemid));

            //요구 아이템이 없으면
            if (giveitem == null)
            {
                //구매실패 UI를 여는 구문
                clothingUIManager.OnClickBuyFail();
            }
            else
            {
                //보유한 요구 아이템의 이이템코드 받아옴
                string gitemid = giveitem.ItemNo;
                //보유한 요구 아이템의 이이템갯수 받아옴
                string _gitemcnt = giveitem.ItemCnt;

                //갯수를 계산하기 위한 형변화
                int gitemcnt = int.Parse(_gitemcnt);
                int ritemcnt = int.Parse(reqitem_cnt);

                int result = gitemcnt - ritemcnt;
                //계산 후 DB에 값을 넣기위해 형변환
                string _result = result.ToString();

                // 쿼리 결과를 한 번만 사용하도록 변경
                if (result >= 0)
                {
                    //인벤토리에 제작 아이템이 있으면
                    if (checkVal != null)
                    {
                        //인벤토리에 있는 제작아이템의 갯수를 받음
                        string _cnt = checkVal.ItemCnt;

                        //계산을 위한 형변환
                        int cnt = int.Parse(_cnt);
                        int _uitem = cnt + 1;
                        //DB에 값을 올리기 위한 형변화
                        string uitem = _uitem.ToString();

                        //값 업데이트
                        inventoryDao.ItemCraftPayment(gitemid, _result);
                        inventoryDao.ItemCraftUpdate(uitem, itemid);
                        //제작 성공 UI을 염
                        clothingUIManager.OnClickBuyComplete();

                        //인벤토리 갱신
                        invenList = inventoryDao.GetInvenList();
                    }
                    //인벤토리에 제작 아이템이 없다면
                    else
                    {
                        string cnt = "1";
                        inventoryDao.ItemCraftInsert(itemid, cnt);
                        clothingUIManager.OnClickBuyComplete();
                        //값 지불 
                        inventoryDao.ItemCraftPayment(gitemid, _result);
                        //DB에 insert구문으로 값을 넣어줌
                        inventoryDao.ItemCraftInsert(itemid, cnt);
                        //제작 성공 UI을 염
                        clothingUIManager.OnClickBuyComplete();
                        //인벤토리 갱신
                        invenList = inventoryDao.GetInvenList();
                    }
                }
                else
                {
                    clothingUIManager.OnClickBuyFail();
                }
            }
        }

        //물품 구매하는 메서드
        public void BuyThing()
        {
            string userCash = "";

            //DB에서 유저의 보유 현금 받아옴
            StartCoroutine(clothingDao.GetUserInfoFromDB(cash =>
            {
                userCash = cash;
            }));
            //물품 구매 현금 계산
            int nowCash = int.Parse(userCash) - int.Parse(Buyprice);

            string NowCash = nowCash.ToString();

            //인벤토리에 구매할 물품 보유 여부 확인, 있다면 구매할 물품에 대한 정보 담음
            InventoryVO buyitem = invenList.Find(p => p.ItemNo.Equals(itemid));
            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + nowCash);


            if (nowCash > 0)
            {
                //구매 후 현금으로 업데이트하고 
                clothingDao.UpdateUserCash(NowCash);
                //구매 성공 UI를 열음
                clothingUIManager.OnClickBuyComplete();

                //구매한 물품이 인벤토리에 없다면
                if (buyitem == null)
                {
                    //DB에서 insert로 값을 넣어줌
                    inventoryDao.InsertBuyThing(itemid);
                    //inventory 갱신
                    invenList = inventoryDao.GetInvenList();
                }

                //구매한 물품이 인벤토리에 있다면
                else
                {
                    //인벤토리에 있는 아이템의 갯수을 받아
                    string _item = buyitem.ItemCnt;
                    int item = int.Parse(_item);

                    //갯수를 추가하고 DB에 없데이트를 위해 형변환함
                    string bitem = (item + 1).ToString();

                    //DB에 update구문으로 넣어줌
                    inventoryDao.UpdateBuyThing(bitem, itemid);
                    //inventory 갱신
                    invenList = inventoryDao.GetInvenList();
                }
            }
            //구매가 실패하면
            else
            {
                Debug.Log("Not enough cash!");
                clothingUIManager.OnClickBuyFail();
            }

        }
    }
}
