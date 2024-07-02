using Script.UI.MainLevel.Inventory;
using Script.UI.StartLevel.Dao;
using System.Collections;
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

        public GameObject ClotBuyPrefab; // 옷가게 구매 이미지 프리팹 참조
        public GameObject ClotBuy; // 옷가게 구매 이미지 참조
        public Transform ClotBuyLayout; // 구매 리스트들이 들어갈 레이아웃 참조

        public GameObject ClotSellPrefab; // 옷가게 판매 이미지 프리팹 참조
        public GameObject ClotSell; // 옷가게 판매 이미지 참조
        public Transform ClotSellLayout; // 판매 리스트들이 들어갈 레이아웃 참조

        private readonly List<GameObject> ClotBuyInstances = new();
        private readonly List<GameObject> ClothingInstances = new();
        private readonly List<GameObject> ClotSellInstances = new();

        private List<ClothingVO> _clothingList = new();

        //구매가격을 담는 전역 변수
        private string Buyprice;

        //DAO호출을 함
        private ClothingDao clothingDao;
        private StartLevelDao _sld; // StartLevelDao를 사용하기 위한 변수

        //옷 만들기에 들어가는 아이템을 담음
        private List<Dictionary<string, object>> clothingList = new();

        //UI매니저 호출을 함
        private ClothingUIManager clothingUIManager;

        //구매하기에 들어가는 아이템을 담음
        private List<Dictionary<string, object>> cltBuyList = new();

        //판매하기에 들어가는 아이템을 담음
        //인벤토리 값을 담음
        private List<InventoryVO> invenList = new();

        private InventoryDao inventoryDao;
        private List<Dictionary<string, object>> inventoryList = new();
        private Dictionary<string, object> userinfo = new();

        //제작하거나 구매하는 아이템코드를 담음
        private string itemid;

        //나중에 세션등으로 받을 유저 아이디값

        //옷 제작 시 필요한 아이템코드와 갯수를 담음
        private string reqitem;

        private string reqitem_cnt;

        //판매가격을 담는 전역 변수
        private string Sellprice;
        private string pid;


        private void Start()
        {
            clothingDao = GetComponent<ClothingDao>();
            inventoryDao = GetComponent<InventoryDao>();
            clothingUIManager = FindObjectOfType<ClothingUIManager>();
            _sld = GetComponent<StartLevelDao>();

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
            
            StartCoroutine(_sld.GetUserEmail(info =>
            {
                userinfo = info;
                pid = userinfo["useremail"].ToString();
                StartCoroutine(inventoryDao.GetInventoryList(pid, list =>
                {
                    inventoryList = list;
                    SetCltSellList(inventoryList);
                }));
            }));
           
          
            //cltBuyList = clothingDao.GetClothingBuyList();
            //invenList = inventoryDao.GetInvenList();

            //SetClothingList(clothingList);
            //SetCltBuyList(cltBuyList);
        }

        private void SetCltSellList(List<Dictionary<string, object>> clothingList)
        {
            ClotSell.SetActive(true);

            foreach (GameObject clotSellInstance in ClotSellInstances)
            {
                Destroy(clotSellInstance);
            }

            ClotSellInstances.Clear();

            foreach (Dictionary<string, object> cls in clothingList)
            {
                GameObject clotSellInstance = Instantiate(ClotSellPrefab, ClotSellLayout);
                cls.TryGetValue("itemid", out object itemId);
                clotSellInstance.name = "Clothing" + itemId;
                ClotSellInstances.Add(clotSellInstance);

                Text textComponent = clotSellInstance.GetComponentInChildren<Text>();
                if (textComponent == null)
                {
                    return;
                }

                cls.TryGetValue("itemnm", out object itemNm);
                cls.TryGetValue("itemdesc", out object itemDesc);
                cls.TryGetValue("itemcnt", out object itemcnt);
                cls.TryGetValue("sellprice", out object sellprice);
                textComponent.text = itemNm + "\r\n" +
                                     itemDesc + "\r\n" +
                                     "보유 갯수 : " + itemcnt + "\r\n" +
                                     "판매 가격 : " + sellprice;
            }

            ClotSell.SetActive(false);
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

                if (textComponent == null)
                {
                    return;
                }

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

            if (textComponent == null)
            {
                return;
            }

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

            cl.TryGetValue("req_itemid", out object tempReqItem);
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

        //물품 판매 시 판매물품 가격 받아오는 구문
        public void GetClotSellValue()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid = parentObjectName.Replace("Clothing", "");

            Dictionary<string, object> clv = inventoryList.Find(p => p["itemid"].ToString() == itemid);

            clv.TryGetValue("sellprice", out object tempSellPrice);
            Sellprice = tempSellPrice?.ToString();

            Debug.Log(Sellprice);
        }

        //옷 구매하는 구문
        public void BuyClothing()
        {
            
            StartCoroutine(inventoryDao.GetInventoryList(pid, list =>
            {
                inventoryList = list;

                // 여러 번의 데이터베이스 액세스를 단일 액세스로 변경

                //인벤토리에 요구아이템이 있는지 찾음
                Dictionary<string, object> giveitem = inventoryList.Find(p => p["itemid"].ToString().Equals(reqitem));
                //제작 아이템이 인벤토리에 있는지 확인
                Dictionary<string, object> checkVal = inventoryList.Find(p => p["itemid"].ToString().Equals(itemid));

                //요구 아이템이 없으면
                if (giveitem == null)
                {
                    //구매실패 UI를 여는 구문
                    clothingUIManager.OnClickBuyFail();
                }
                else
                {
                    //보유한 요구 아이템의 이이템코드 받아옴
                    string gitemid = giveitem["itemid"].ToString();
                    Debug.Log("요구아이템 아이디 " + gitemid);
                    //보유한 요구 아이템의 이이템갯수 받아옴
                    string _gitemcnt = giveitem["itemcnt"].ToString();
                    Debug.Log("요구아이템 개수 " + _gitemcnt);

                    //갯수를 계산하기 위한 형변화
                    int gitemcnt = int.Parse(_gitemcnt);
                    int ritemcnt = int.Parse(reqitem_cnt);

                    int result = gitemcnt - ritemcnt;
                    //계산 후 DB에 값을 넣기위해 형변환
                    string _result = result.ToString();

                    Debug.Log("DB 업뎃되는 계산 후 잔액 " + _result);
                    // 쿼리 결과를 한 번만 사용하도록 변경
                    if (result >= 0)
                    {
                        //결제처리
                        StartCoroutine(inventoryDao.ItemCraftPayments(pid, gitemid, _result));
                        //제작성공 UI open
                        clothingUIManager.OnClickMakeComplete();

                        //인벤토리에 제작 아이템이 있으면
                        if (checkVal != null)
                        {
                            //인벤토리에 있는 제작아이템의 갯수를 받음
                            string _cnt = checkVal["itemcnt"].ToString();
                            Debug.Log("인벤토리 값 " + _cnt);

                            //계산을 위한 형변환
                            int cnt = int.Parse(_cnt);
                            int _uitem = cnt + 1;
                            //DB에 값을 올리기 위한 형변화
                            string uitem = _uitem.ToString();
                            Debug.Log("계산 결과 값 " + uitem);

                            //값 업데이트
                            StartCoroutine(inventoryDao.ItemCraftUpdates(pid, itemid, uitem));
                        }
                        //인벤토리에 제작 아이템이 없다면
                        else
                        {
                            string cnt = "1";
                            //DB에 insert구문으로 값을 넣어줌
                            StartCoroutine(inventoryDao.ItemCraftInserts(pid, itemid, cnt));
                        }
                        // Fetch the inventory list
                        StartCoroutine(inventoryDao.GetInventoryList(pid, list =>
                        {
                            inventoryList = list;
                        }));
                    }
                    else
                    {
                        clothingUIManager.OnClickMakeFail();
                    }
                }
            }));
        }

        public void BuyThing()
        {
            // Get the inventory list and user info synchronously
            StartCoroutine(BuyThingCoroutine());
        }

        private IEnumerator BuyThingCoroutine()
        {
           
            // Fetch the user info
            bool userInfoFetched = false;
            int cash = 0;
            StartCoroutine(_sld.GetUser(pid, info =>
            {
                cash = int.Parse((string)info["cash"]);
                userInfoFetched = true;
            }));

            // Wait until the user info is fetched
            yield return new WaitUntil(() => userInfoFetched);

            int price = int.Parse(Buyprice);


            if (cash >= price)
            {
                int payment = cash - price;
                string result = payment.ToString();

                Dictionary<string, object>
                    checkVal = inventoryList.Find(dic => dic["itemid"].ToString().Equals(itemid));
                StartCoroutine(inventoryDao.UpdateUserCashs(pid, result));
                clothingUIManager.OnClickBuyComplete();

                if (checkVal != null)
                {
                    string _cnt = checkVal["itemcnt"].ToString();
                    int cnt = int.Parse(_cnt);
                    int _bitem = cnt + 1;
                    string bitem = _bitem.ToString();

                    StartCoroutine(inventoryDao.UpdateBuyThings(bitem, itemid, pid));
                }
                else
                {
                    string cnt = "1";
                    StartCoroutine(inventoryDao.InsertBuyThings(itemid, cnt, pid));
                }
                // Fetch the inventory list
                bool inventoryFetched = false;
                StartCoroutine(inventoryDao.GetInventoryList(pid, list =>
                {
                    inventoryList = list;
                    inventoryFetched = true;
                }));

                // Wait until the inventory list is fetched
                yield return new WaitUntil(() => inventoryFetched);

            }
            else
            {
                clothingUIManager.OnClickBuyFail();
            }
        }

        public void SellThing()
        {
            // Get the inventory list and user info synchronously
            StartCoroutine(SellThingCoroutine());
        }

        private IEnumerator SellThingCoroutine()
        {
            // Fetch the user info
            bool userInfoFetched = false;
            int cash = 0;
            StartCoroutine(_sld.GetUser(pid,info =>
            {
                cash = int.Parse((string)info["cash"]);
                userInfoFetched = true;
            }));

            // Wait until the user info is fetched
            yield return new WaitUntil(() => userInfoFetched);

            int price = int.Parse(Sellprice);
            Dictionary<string, object> checkVal = inventoryList.Find(dic => dic["itemid"].ToString().Equals(itemid));
            if (checkVal != null)
            {
                int payment = cash + price;
                string result = payment.ToString();

                string _cnt = checkVal["itemcnt"].ToString();
                int cnt = int.Parse(_cnt);
                int _bitem = cnt - 1;
                string bitem = _bitem.ToString();

                // Update user cash
                yield return StartCoroutine(inventoryDao.UpdateUserCashs(pid, result));

                // Update sell things
                yield return StartCoroutine(inventoryDao.UpdateSellThings(bitem, itemid, pid));

                clothingUIManager.OnClickSellComplete();

                // Fetch the updated inventory list after selling the item
                bool updatedInventoryFetched = false;
                StartCoroutine(inventoryDao.GetInventoryList(pid, updatedList =>
                {
                    inventoryList = updatedList;
                    updatedInventoryFetched = true;
                }));

                // Wait until the updated inventory list is fetched
                yield return new WaitUntil(() => updatedInventoryFetched);

                // Update the sell list UI
                SetCltSellList(inventoryList);
            }
            else
            {
                clothingUIManager.OnClickSellFail();
            }
        }
    }
}