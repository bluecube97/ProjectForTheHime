using Script.UI.MainLevel.Inventory;
using Script.UI.StartLevel.Dao;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI.Outing.RestaurantScript
{
    public class RestaurantManager : MonoBehaviour
    {
        public GameObject foodListPrefab; //foodlist 이미지 참조
        public GameObject foodList; //foodlist 이미지 참조
        public Transform foodListLayout; //foodlistLayout 이미지 참조

        public GameObject sellListPrefab; //판매 이미지 참조
        public GameObject sellList; // 판매이미지 참조
        public Transform sellListLayout; //판매 이미지 레이아웃 참조
        private readonly List<GameObject> sellListInstances = new();

        private List<Dictionary<string, object>> FoodList; //List 형식으로 foodlist를 담음
        private List<Dictionary<string, object>> inventoryList; //List 형식으로 foodlist를 담음

        private FoodListVO foodlistVO; // foodlist를 담음
        private RestaurantDao restaurantDao;
        private InventoryDao inventoryDao;

        private RestaurantUIController RestaurantUIController;
        private string FoodPr; //음식 가격을 담음
        //나중에 세션등으로 받을 유저 아이디값
        private Dictionary<string, object> userinfo = new();
        private StartLevelDao _sld; // StartLevelDao를 사용하기 위한 변수
        private string pid;
        private string itemid;
        private string Sellprice;

        private void Start()
        {            
            _sld = GetComponent<StartLevelDao>();
            restaurantDao = GetComponent<RestaurantDao>(); // RestaurantDao 컴포넌트를 가져와서 초기화합니다.
            inventoryDao = GetComponent<InventoryDao>(); // RestaurantDao 컴포넌트를 가져와서 초기화합니다.
            RestaurantUIController = FindObjectOfType<RestaurantUIController>(); // RestaurantManager를 찾아서 초기화합니다.
           
            StartCoroutine(restaurantDao.GetFoodList(list =>
            {
                FoodList = list;
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

        //밥먹기 클릭 시 실행되는 메서드
        public void OnclickFoodList()
        {
            foreach (var dic in FoodList)
            {
                //음식 목록 인스턴스화
                GameObject foodListInstance = Instantiate(foodListPrefab, foodListLayout);

                //각각의 목록에 이름 부여
                foodListInstance.name = "foodlist" + dic["itemid"];
                Text textComponent = foodListInstance.GetComponentInChildren<Text>();
                if (textComponent == null)
                {
                    return;
                }

                dic.TryGetValue("itemnm", out object foodnm);
                dic.TryGetValue("sellprice", out object foodpr);
                textComponent.text = foodnm + "\r\n" +
                                     " " + foodpr;

            }

            foodList.SetActive(false);
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
        //버튼 클릭 시
        public void GetclickListValue()
        {
            //클릭한 버튼 값 받아옴
            GameObject clickList = EventSystem.current.currentSelectedGameObject;

            //오브젝트의 이름을 담음
            string objectName = clickList.name;

            // dic["itemNo"]를 추출
            string indexString = objectName.Replace("foodlist", "");

            //FoodList에서 indexString과 동일한 이름이 들어있는 인덱스에 저장되어 있는 값 담음
            Dictionary<string, object> fv =
                FoodList.Find(p => p["itemid"].ToString().Equals(indexString));

            //추출한 값을 전역변수로 담음
            FoodPr = fv["sellprice"].ToString();

            Debug.Log("계산 금액 " + FoodPr);
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

        //결제하는 구문
        public void ProcessPayment()
        {
            StartCoroutine(ProcessPaymentCoroutine());
        }

        private IEnumerator ProcessPaymentCoroutine()
        {

            //유저 정보 들고옴
            bool userInfoFetched = false;
            StartCoroutine(_sld.GetUser(pid,info =>
            {
                userinfo = info;
                userInfoFetched = true;
            }));
            yield return new WaitUntil(() => userInfoFetched);


            //계산을 위한 형변환
            int userCash = int.Parse(userinfo["cash"].ToString());
            int _FoorPr = int.Parse(FoodPr);
            int _NowCash = userCash - _FoorPr;
            string NowCash = _NowCash.ToString();
            Debug.Log("계산 금액 " + FoodPr);

            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + NowCash);
            //계산된 값이 0보다 크면
            if (userCash > _FoorPr)
            {
                //결제 된 금액을 업데이트 하고
                StartCoroutine(inventoryDao.UpdateUserCashs(pid, NowCash));

                //구매성공 UI를 연다
                RestaurantUIController.OnClickBuyComplete();
            }
            else
            {
                Debug.Log("Not enough cash!");
                //구매 실패 UI를 염
                RestaurantUIController.OnClickBuyFail();
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
                RestaurantUIController.OnClickSellComplete();   
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
                StartCoroutine(inventoryDao.GetInventoryList(pid,updatedList =>
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
                RestaurantUIController.OnClickSellFail();
            }
            
        }
    }
}