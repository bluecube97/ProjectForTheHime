using Script.UI.MainLevel.Inventory;
using Script.UI.StartLevel.Dao;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI.Outing.VarietyStoreScript
{
    public class VarietyStoreManager : MonoBehaviour
    {   
        public GameObject BuyListPrefab; // BUYList 이미지 프리팹 참조
        public GameObject buyList; // BUYList 이미지 참조
        public Transform buyListLayout; // BUYList들이 들어갈 레이아웃 참조
        private List<GameObject> buyListInstancese = new();
        
        public GameObject sellListPrefab; //판매 이미지 참조
        public GameObject sellList; // 판매이미지 참조
        public Transform sellListLayout; //판매 이미지 레이아웃 참조
        private readonly List<GameObject> sellListInstances = new();
        
        private List<Dictionary<string,object>> inventoryList = new();
        private List<Dictionary<string,object>> itemList = new();
        
        public GameObject VarietyStoreBuyBackGround; // 설정 패널 오브젝트
        public GameObject VarietyStoreSellBackGround; // 설정 패널 오브젝트
        public GameObject BuySuccess; //구매 성공 이미지
        public GameObject BuyFail; //구매 실패 이미지
        public GameObject CheckBuyMenu; // 구매 선택 이미지
        public GameObject SellSuccess; //구매 성공 이미지
        public GameObject SellFail; //구매 실패 이미지
        public GameObject CheckSellMenu; // 구매 선택 이미지
        
        private VarietyStoreDao varietystoreDao;
        private InventoryDao inventoryDao;
        private Dictionary<string, object> userinfo = new();
        private StartLevelDao _sld; // StartLevelDao를 사용하기 위한 변수

        //구매 시 얻게되는 아이템 아이디를 담음
        private string itemid;
        private int itempr; // 상점 구매시 판매 아이템 가격 담는 통
        private string Sellprice;
        private string pid;

        public void Start()
        {
            inventoryDao = GetComponent<InventoryDao>();
            varietystoreDao = GetComponent<VarietyStoreDao>();
            _sld = GetComponent<StartLevelDao>();

            StartCoroutine(varietystoreDao.GetBuyList(list =>
            {
                itemList = list;
                LoadItemList(itemList);
            }));
            
            // 서버에서 인벤토리 데이터 가져오기
            StartCoroutine(_sld.GetUserEmail(info =>
            {
                userinfo = info;
                pid = userinfo["useremail"].ToString();
                StartCoroutine(inventoryDao.GetInventoryList(pid, list =>
                {
                    inventoryList = list;
                    SetSellList(inventoryList);
                }));
            }));
            
            /*ItemList = varietystoreDao.LoadData();
            invenList = inventoryDao.GetInvenList();*/

        }
        
        //전체 잡화점 아이템 값 출력하는 구문
        public void LoadItemList(List<Dictionary<string, object>> list)
        {
            buyList.SetActive(true);
            
            foreach (GameObject buyListInstance in buyListInstancese)
            {
                Destroy(buyListInstance);
            }
            buyListInstancese.Clear();
            
            foreach (var dic in list)
            {
                GameObject buyListInstance = Instantiate(BuyListPrefab, buyListLayout);
                buyListInstance.name = "itemlist" + dic["itemid"];
                buyListInstancese.Add(buyListInstance);
                Text textComponent = buyListInstance.GetComponentInChildren<Text>();
                if (textComponent == null)
                {
                    return;
                }
                dic.TryGetValue("itemnm", out object itemNm);
                dic.TryGetValue("buyprice", out object buyprice);
                textComponent.text = itemNm +"\r\n"+ buyprice;
                
            }
            buyList.SetActive(false);

        }
        
        //소모품버튼 클릭 시
        public void Openingredients()
        {
            buyList.SetActive(true);

            foreach (GameObject buyListInstance in buyListInstancese)
            {
                Destroy(buyListInstance);
            }

            foreach (var dic in itemList)
            {
                //소모품 타입인 값들만 출력
                if (dic["typeid"].ToString().Equals("2001"))
                {
                    GameObject buyListInstance = Instantiate(BuyListPrefab, buyListLayout);
                    buyListInstance.name = "itemlist" + dic["itemid"];
                    buyListInstancese.Add(buyListInstance);
                    Text textComponent = buyListInstance.GetComponentInChildren<Text>();
                    if (textComponent == null)
                    {
                        return;
                    }
                    dic.TryGetValue("itemnm", out object itemNm);
                    dic.TryGetValue("buyprice", out object buyprice);
                    textComponent.text = itemNm +"\r\n"+ buyprice;
                }
            }

            buyList.SetActive(false);
        }
        
        //선물버튼 클릭 시
        public void Opengift()
        {
            buyList.SetActive(true);

            foreach (GameObject buyListInstance in buyListInstancese)
            {
                Destroy(buyListInstance);
            }

            foreach (var dic in itemList)
            {
                //소모품 타입인 값들만 출력
                if (dic["typeid"].ToString().Equals("3005"))
                {
                    GameObject buyListInstance = Instantiate(BuyListPrefab, buyListLayout);
                    buyListInstance.name = "itemlist" + dic["itemid"];
                    buyListInstancese.Add(buyListInstance);
                    Text textComponent = buyListInstance.GetComponentInChildren<Text>();
                    if (textComponent == null)
                    {
                        return;
                    }
                    dic.TryGetValue("itemnm", out object itemNm);
                    dic.TryGetValue("buyprice", out object buyprice);
                    textComponent.text = itemNm +"\r\n"+ buyprice;
                }
            }

            buyList.SetActive(false);

        }
        
        private void SetSellList(List<Dictionary<string, object>> iList)
        {
            sellList.SetActive(true);

            foreach (GameObject SellInstance in sellListInstances)
            {
                Destroy(SellInstance);
            }

            sellListInstances.Clear();

            foreach (Dictionary<string, object> cls in iList)
            {
                GameObject SellInstance = Instantiate(sellListPrefab, sellListLayout);
                cls.TryGetValue("itemid", out object itemId);
                SellInstance.name = "selllist" + itemId;
                sellListInstances.Add(SellInstance);

                Text textComponent = SellInstance.GetComponentInChildren<Text>();
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

            sellList.SetActive(false);
        }
        public void GetclickSellValue()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid = parentObjectName.Replace("selllist", "");

            Dictionary<string, object> clv = inventoryList.Find(p => p["itemid"].ToString() == itemid);

            clv.TryGetValue("sellprice", out object tempSellPrice);
            Sellprice = tempSellPrice?.ToString();

            Debug.Log(Sellprice);
        }
        
        //선택한 아이템 값 담는 메서드
        public void GetListValue()
        {
            GameObject ListValue = EventSystem.current.currentSelectedGameObject;
            string objectName = ListValue.name;
            string indexString = objectName.Replace("itemlist", "");
           
            Dictionary<string,object> selectedItem = itemList.Find
                (dic =>dic["itemid"].ToString().Equals(indexString));
            if (selectedItem != null)
            {
                if (int.TryParse(selectedItem["buyprice"].ToString(), out int price))
                {
                    Debug.Log("Selected item price: " + price);
                    //구매 할 itemid와 itemid의 price를 담음
                    itempr = price;
                    itemid = indexString;
                }
            }
        }
        
        //구매 시 계산하는 메서드
        public void ProcessPayment()
        {
            StartCoroutine(ProcessPaymentCoroutine());
        }

        private IEnumerator ProcessPaymentCoroutine()
        {
            bool userInfoFetched = false;
            int userCash = 0;

            // 사용자 정보 비동기적으로 가져오기
            StartCoroutine(_sld.GetUser(pid,info =>
            {
                userCash = int.Parse((string)info["cash"]);
                userInfoFetched = true;
            }));

            yield return new WaitUntil(() => userInfoFetched);

            if (userCash >= itempr)
            {
                int _NowCash = userCash - itempr;
                StartCoroutine(inventoryDao.UpdateUserCashs(pid, _NowCash.ToString()));
                //DB에 값을 넣을 때 update와 insert문을 구별 하기 위해
                // 구매 시 얻게 되는 itemid가 있는 지 확인
                Dictionary<string, object> buyitem = inventoryList.Find(p => p["itemid"].ToString().Equals(itemid));
                BuySuccessOn();
                //인벤토리에 구매 아이템이 있다면
                if (buyitem != null)
                {
                    string _cnt = buyitem["itemcnt"].ToString();
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
                StartCoroutine(inventoryDao.GetInventoryList(pid,list =>
                {
                    inventoryList = list;
                    inventoryFetched = true;
                }));

                // Wait until the inventory list is fetched
                yield return new WaitUntil(() => inventoryFetched);
            }
            else
            {
                BuyFailOn();
                Debug.Log("Not enough cash!");
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
                SellSuccessOn();
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
                SetSellList(inventoryList);
            }
            else
            {
                SellFailOn();
            }
        }
        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
        
        public void ToggleMenu(GameObject menu, bool isOpen)
        {
            menu.SetActive(isOpen);
        }

            public void OpenBuy() => ToggleMenu(VarietyStoreBuyBackGround, true);
            public void CloseBuy()=> ToggleMenu(VarietyStoreBuyBackGround, false);
            public void OpenSell()=> ToggleMenu(VarietyStoreSellBackGround, true);
            public void CloseSell()=> ToggleMenu(VarietyStoreSellBackGround, false);
            public void OpenCheckBuy()=> ToggleMenu(CheckBuyMenu, true);
            public void CloseCheckBuy()=> ToggleMenu(CheckBuyMenu, false);
            public void BuySuccessOn()=> ToggleMenu(BuySuccess, true);
            public void BuySuccessOut()=> ToggleMenu(BuySuccess, false);
            public void BuyFailOn()=> ToggleMenu(BuyFail, true);
            public void BuyFailOut()=> ToggleMenu(BuyFail, false);
            public void OpenCheckSell()=> ToggleMenu(CheckSellMenu, true);
            public void CloseCheckSell()=> ToggleMenu(CheckSellMenu, false);
            public void SellSuccessOn()=> ToggleMenu(SellSuccess, true);
            public void SellSuccessOut()=> ToggleMenu(SellSuccess, false);
            public void SellFailOn()=> ToggleMenu(SellFail, true);
            public void SellFailOut()=> ToggleMenu(SellFail, false);


    }
    
}
    
    
