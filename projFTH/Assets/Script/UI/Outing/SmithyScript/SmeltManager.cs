using Script.UI.MainLevel.Inventory;
using Script.UI.StartLevel.Dao;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI.Outing.SmithyScript
{
    public class SmeltManager : MonoBehaviour
    {
        public GameObject smeltListPrefab; //재련 이미지 참조
        public GameObject smeltList; // 재련 이미지 참조 
        public Transform smeltListLayout; //재련 이미치 레이아웃 참조

        public GameObject buyListPrefab; //구매 이미지 참조
        public GameObject buyList; // 구매이미지 참조
        public Transform buyListLayout; //구매 이미지 레이아웃 참조

        public GameObject sellListPrefab; //판매 이미지 참조
        public GameObject sellList; // 판매이미지 참조
        public Transform sellListLayout; //판매 이미지 레이아웃 참조

        //구매에 대한 정보 담음
        private List<Dictionary<string, object>> BuyList = new();

        private readonly List<GameObject> buyListInstances = new();

        //구매 시 구매 가격을 담음
        private int Buyprice;

        //인벤토리 정보를 담음
        private List<InventoryVO> invenList = new();
        private InventoryDao inventoryDao;

        private List<Dictionary<string, object>> inventoryList = new();
        private Dictionary<string, object> userinfo = new();
        private StartLevelDao _sld; // StartLevelDao를 사용하기 위한 변수

        //구매나 제련 시 얻게되는 아이템 아이디를 담음
        private string itemid;

        //나중에 세션등으로 받을 유저 아이디값
        private string pid;

        //재련 시 요구 아이템 아이디를 담음
        private string reqitem;

        //재련 시 요구 아이템의 갯수를 담음
        private string reqitem_cnt;

        private readonly List<GameObject> sellListInstances = new();

        //판매 시 판매가격을 담음
        private string Sellprice;


        private SmeltDao smeltDao;

        //재련에 대한 정보 담음
        private List<Dictionary<string, object>> SmeltList = new();

        private readonly List<GameObject> smeltListInstances = new();

        private SmithyController smithyui;

        private void Start()
        {
            // Dao 컴포넌트들 초기화
            smeltDao = GetComponent<SmeltDao>();
            inventoryDao = GetComponent<InventoryDao>();
            smithyui = GetComponent<SmithyController>();
            _sld = GetComponent<StartLevelDao>();

            // 서버에서 BuyList 데이터 가져오기
            StartCoroutine(smeltDao.GetBuyLists(list =>
            {
                BuyList = list;
                // BuyList 세팅 후 BuyList 화면에 출력
                SetBuyList(BuyList);
            }));

            // 서버에서 SmeltList 데이터 가져오기
            StartCoroutine(smeltDao.GetSmeltLists(list =>
            {
                SmeltList = list;
                // SmeltList 세팅 후 SmeltList 화면에 출력
                SetSmeltList(SmeltList);
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
        }

        // SmeltList 화면에 출력하는 함수
        public void SetSmeltList(List<Dictionary<string, object>> sList)
        {
            smeltList.SetActive(true);

            // 기존에 생성된 SmeltList 객체들 삭제
            foreach (GameObject smeltInstance in smeltListInstances)
            {
                Destroy(smeltInstance);
            }

            smeltListInstances.Clear();

            // SmeltList 데이터로 새로운 객체들 생성 및 텍스트 세팅
            foreach (Dictionary<string, object> dic in sList)
            {
                // 인벤토리에서 해당 아이템의 보유 개수 확인
                Dictionary<string, object> have = inventoryList.Find(p => p["itemid"].Equals(dic["req_item"]));
                string havecnt = have == null ? "0" : have["itemcnt"].ToString();

                // 프리팹으로부터 인스턴스 생성 후 리스트에 추가
                GameObject smeltListInstance = Instantiate(smeltListPrefab, smeltListLayout);
                smeltListInstance.name = "list" + dic["itemid"];
                smeltListInstances.Add(smeltListInstance);

                // 텍스트 컴포넌트에 정보 표시
                Text textComponent = smeltListInstance.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    textComponent.text = dic["itemnm"] + "\r\n" +
                                         dic["itemdesc"] + "\r\n" +
                                         "소재 :  " + dic["req_name"] + "\r\n" +
                                         "필요 갯수 : " + dic["req_itemcnt"] + "\r\n" +
                                         "소지 갯수 : " + havecnt;
                }
            }

            smeltList.SetActive(false);
        }

        //구매 목록 출력
        public void SetBuyList(List<Dictionary<string, object>> bList)
        {
            // 기존에 생성된 BuyList 객체들 삭제
            foreach (GameObject instance in buyListInstances)
            {
                Destroy(instance);
            }

            buyListInstances.Clear();

            // BuyList 데이터로 새로운 객체들 생성 및 텍스트 세팅
            foreach (Dictionary<string, object> dic in bList)
            {
                // 프리팹으로부터 인스턴스 생성 후 리스트에 추가
                GameObject buyListInstance = Instantiate(buyListPrefab, buyListLayout);
                buyListInstance.name = "weaponlist" + dic["itemid"];
                buyListInstances.Add(buyListInstance);

                // 텍스트 컴포넌트에 정보 표시
                Text textComponent = buyListInstance.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    textComponent.text = dic["itemnm"] + "\r\n" +
                                         dic["itemdesc"] + "\r\n" +
                                         "가격 : " + dic["buyprice"];
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

        //구매하기 버튼 클릭 시 
        public void GetclickBuyList()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid = parentObjectName.Replace("weaponlist", "");

            //구매 목록 중 itemid와  동일한 item을 찾아 그 값을 담음
            Dictionary<string, object> selectedItem = BuyList.Find
                (dic => dic["itemid"].ToString() == itemid);

            //itemid 가 있다면 itemSellPr의 값을 추출해 int형으로 price에 담음
            if (selectedItem != null && selectedItem.TryGetValue("buyprice", out object priceObj)
                                     && priceObj is string priceStr
                                     && int.TryParse(priceStr, out Buyprice))
            {

            }
        }

        //구매 리스트 선택 시 구매 리스트 정보 받아오기(현금 구매)
        public void GetclickWeaponList()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid = parentObjectName.Replace("list", "");
            //SmeltList에 itmeid이 있는 지 찾아 값을 담음
            Dictionary<string, object> sw = SmeltList.Find
                (dic => dic["itemid"].ToString() == itemid);
            //요구 itemid를 담음
            reqitem = sw["req_item"].ToString();
            //요구 itemid의 갯수를 담음
            reqitem_cnt = sw["req_itemcnt"].ToString();
            Debug.Log(reqitem);
            Debug.Log(reqitem_cnt);
        }

        //물품 판매 시 판매물품 가격 받아오는 구문
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


            if (userCash >= Buyprice)
            {
                int newCash = userCash - Buyprice;
                StartCoroutine(inventoryDao.UpdateUserCashs(pid, newCash.ToString()));

                Dictionary<string, object> buyItem =
                    inventoryList.Find(p => p["itemid"].ToString().Equals(itemid));
                smithyui.OnClickBuyComple(); // 구매 성공 UI 표시

                if (buyItem == null)
                {
                    string cnt = "1";
                    // 인벤토리에 구매 아이템이 없으면 추가
                    StartCoroutine(inventoryDao.InsertBuyThings(itemid, cnt, pid));
                }
                else
                {
                    // 인벤토리에 구매 아이템이 있으면 업데이트
                    int itemCount = int.Parse(buyItem["itemcnt"].ToString()) + 1;
                    StartCoroutine(inventoryDao.UpdateBuyThings(itemCount.ToString(), itemid, pid));
                }

                // 사용자 현금 업데이트

                // 인벤토리 목록 업데이트
                StartCoroutine(inventoryDao.GetInventoryList(pid, list =>
                {
                    inventoryList = list;
                }));
            }
            else
            {
                Debug.Log("돈이 부족합니다!");
                smithyui.OnClickBuyFail(); // 구매 실패 UI 표시
            }
        }

        // 재련 시 결제 처리 메서드 시작
        public void ProcessPayItem()
        {
            StartCoroutine(ProcessPayItemCoroutine());
        }

        private IEnumerator ProcessPayItemCoroutine()
        {
            bool inventoryFetched = false;

            // 인벤토리 목록 비동기적으로 가져오기
            StartCoroutine(inventoryDao.GetInventoryList(pid, list =>
            {
                inventoryList = list;
                inventoryFetched = true;
            }));

            yield return new WaitUntil(() => inventoryFetched);

            Dictionary<string, object> checkVal = inventoryList.Find(p => p["itemid"].Equals(itemid));
            Dictionary<string, object> giveItem = inventoryList.Find(p => p["itemid"].Equals(reqitem));

            if (giveItem == null)
            {
                Debug.Log("호갱님 가진 것이 없으시네요");
                yield break;
            }

            int gitemCnt = int.Parse(giveItem["itemcnt"].ToString());
            int reqItemCnt = int.Parse(reqitem_cnt);
            int result = gitemCnt - reqItemCnt;

            if (result >= 0)
            {
                // 재련에 필요한 아이템 갯수 업데이트
                StartCoroutine(inventoryDao.ItemCraftPayments(pid, giveItem["itemid"].ToString(), result.ToString()));

                if (checkVal != null)
                {
                    // 재련 성공 시 아이템 갯수 업데이트
                    int cnt = int.Parse(checkVal["itemcnt"].ToString()) + 1;
                    StartCoroutine(inventoryDao.ItemCraftUpdates(pid, cnt.ToString(), itemid));
                }
                else
                {
                    // 인벤토리에 아이템 추가
                    StartCoroutine(inventoryDao.ItemCraftInserts(pid, itemid, "1"));
                }

                // 인벤토리 목록 업데이트
                StartCoroutine(inventoryDao.GetInventoryList(pid, list =>
                {
                    inventoryList = list;
                    smithyui.OnClickSmithyComplete(); // 재련 성공 UI 표시
                }));
            }
            else
            {
                Debug.Log("호갱님 재료가 부족해요");
                smithyui.OnClickSmithyFail(); // 재련 실패 UI 표시
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

                smithyui.OnClickSellComplete();

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
                smithyui.OnClickSellFail();
            }
        }
    }
}