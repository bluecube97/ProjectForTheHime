using Script.UI.MainLevel.Inventory;
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
        
        private List<InventoryVO> invenList = new();
        private ItemListVO ItemListvo; // 상점 구매 아이템 정보를 담는 통
        private List<ItemListVO> ItemList; // 아이템 리스트
        
        //구매 시 얻게되는 아이템 아이디를 담음
        private string itemid;
        private int itempr; // 상점 구매시 판매 아이템 가격 담는 통
       
        public GameObject VarietyStoreBuyBackGround; // 설정 패널 오브젝트
        public GameObject VarietyStoreSellBackGround; // 설정 패널 오브젝트
        public GameObject BuySuccess; //구매 성공 이미지
        public GameObject BuyFail; //구매 실패 이미지
        public GameObject CheckBuyMenu; // 구매 선택 이미지
        
        private VarietyStoreDao varietystoreDao;
        private InventoryDao inventoryDao;

        public void Start()
        {
            inventoryDao = GetComponent<InventoryDao>();
            varietystoreDao = GetComponent<VarietyStoreDao>();
            ItemList = varietystoreDao.LoadData();
            invenList = inventoryDao.GetInvenList();

        }
        
        //전체 잡화점 아이템 값 출력하는 구문
        public void LoadItemList()
        {
            buyList.SetActive(true);
            
            foreach (GameObject buyListInstance in buyListInstancese)
            {
                Destroy(buyListInstance);
            }
            buyListInstancese.Clear();
            
            foreach (var dic in ItemList)
            {
                GameObject buyListInstance = Instantiate(BuyListPrefab, buyListLayout);
                buyListInstance.name = "itemlist" + dic.ITEMNO;
                buyListInstancese.Add(buyListInstance);
                Text textComponent = buyListInstance.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    textComponent.text = dic.ITEMNAME +"\r\n"+ dic.ITEMPR;
                }
            }
            buyList.SetActive(false);

        }
        
        //선택한 아이템 값 담는 메서드
        public void GetListValue()
        {
            GameObject ListValue = EventSystem.current.currentSelectedGameObject;
            string objectName = ListValue.name;
            string indexString = objectName.Replace("itemlist", "");
           
            ItemListVO selectedItem = ItemList.Find
                (dic =>dic.ITEMNO  == indexString);
            if (selectedItem != null)
            {
                if (int.TryParse(selectedItem.ITEMPR, out int price))
                {
                    Debug.Log("Selected item price: " + price);
                    //구매 할 itemid와 itemid의 price를 담음
                    itempr = price;
                    itemid = indexString;
                }
            }
        }

        
        
        //소모품버튼 클릭 시
        public void Openingredients()
        {
            buyList.SetActive(true);

            foreach (GameObject buyListInstance in buyListInstancese)
            {
                Destroy(buyListInstance);
            }

            foreach (var dic in ItemList)
            {
                //소모품 타입인 값들만 출력
                if (dic.TYPEID.Equals("2001"))
                {
                    GameObject buyListInstance = Instantiate(BuyListPrefab, buyListLayout);
                    buyListInstance.name = "itemlist" + dic.ITEMNO;
                    buyListInstancese.Add(buyListInstance);
                    Text textComponent = buyListInstance.GetComponentInChildren<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = dic.ITEMNAME + "\r\n"+dic.ITEMPR;
                    }
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

            foreach (var dic in ItemList)
            {
                //선물 타입인 것만 출력
                if (dic.TYPEID.Equals("3005"))
                {
                    GameObject buyListInstance = Instantiate(BuyListPrefab, buyListLayout);
                    buyListInstance.name = "itemlist" + dic.ITEMNO;
                    buyListInstancese.Add(buyListInstance);
                    Text textComponent = buyListInstance.GetComponentInChildren<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = dic.ITEMNAME +"\r\n"+ dic.ITEMPR;
                    }
                }
            }

            buyList.SetActive(false);

        }
        
        //구매 시 계산하는 메서드
        public void ProcessPayment( )
        {
            string _userCash = varietystoreDao.GetUserInfo();
            int _NowCash = int.Parse(_userCash) - itempr;
            
            //DB에 값을 넣을 때 update와 insert문을 구별 하기 위해
            // 구매 시 얻게 되는 itemid가 있는 지 확인
            InventoryVO buyitem = invenList.Find(p => p.ItemNo.Equals(itemid));

            Debug.Log("DB 유저 현금 " + _userCash);
            Debug.Log("계산 후 금액 " + _NowCash);
            
            //인벤토리에 구매 아이템이 없다면
            if (buyitem == null)
            {
                //인서트 구문을 사용해 인벤토리DB에 넣어줌
                inventoryDao.InsertBuyThing(itemid);
                //값 갱신
                invenList = inventoryDao.GetInvenList();
            }
            else
            {
                //인벤토리에 구매아이템이 있다면 
                // 인벤토리에 구매아이템의 갯수를 담고
                //계산을 위해 형변환
                string _item = buyitem.ItemCnt;
                int item = int.Parse(_item);
                string bitem = (item + 1).ToString();
                string NowCash = _NowCash.ToString();
                
                //계산 후 금액이 0보다 크면
                if (_NowCash > 0)
                {
                    //DB에 값을 update함
                    inventoryDao.UpdateBuyThing(bitem, itemid);
                    varietystoreDao.UpdateUserCash(NowCash);
                    BuySuccessOn();
                    invenList = inventoryDao.GetInvenList();


                }
                else
                {
                    Debug.Log("Not enough cash!");
                    BuyFailOn();

                }
            }
           
        }
            
        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
        
        public void OpenBuy()
        {
            ActivateMenu(VarietyStoreBuyBackGround);
        }

        public void CloseBuy()
        {
            DeactivateMenu(VarietyStoreBuyBackGround); 
        }
        
        public void OpenSell()
        {
            ActivateMenu(VarietyStoreSellBackGround);
        }

        public void CloseSell()
        {
            DeactivateMenu(VarietyStoreSellBackGround); 
        }
        
        public void OpenCheckBuy()
        {
            ActivateMenu(CheckBuyMenu);
        }
        public void CloseCheckBuy()
        {
            DeactivateMenu(CheckBuyMenu); 
        }

        public void BuySuccessOn()
        {
            ActivateMenu(BuySuccess);
        }

        public void BuySuccessOut()
        {
            DeactivateMenu(BuySuccess);
        }
        public void BuyFailOn()
        {
            ActivateMenu(BuyFail);
        }

        public void BuyFailOut()
        {
            DeactivateMenu(BuyFail);
        }
        private void ActivateMenu(GameObject menu)
        {
            menu.SetActive(true);
        }

        private void DeactivateMenu(GameObject menu)
        {
            menu.SetActive(false);
        }

    }
    
}
    
    
