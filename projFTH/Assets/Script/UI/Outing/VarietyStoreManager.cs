using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI.Outing
{
    public class VarietyStoreManager : MonoBehaviour
    {   
        public GameObject BuyListPrefab; // BUYList 이미지 프리팹 참조
        public GameObject buyList; // BUYList 이미지 참조
        public Transform buyListLayout; // BUYList들이 들어갈 레이아웃 참조
        private List<GameObject> buyListInstancese = new();
        private List<ItemListVO> ItemList;
        private int itempr = 0;
        public GameObject BuySuccess;
        public GameObject BuyFail;
        public GameObject CheckBuyMenu;
        private VarietyStoreDao varietystoreDao;
        private ItemListVO ItemListvo;

        public void Start()
        {
            varietystoreDao = GetComponent<VarietyStoreDao>();
            ItemList = varietystoreDao.LoadData();
            LoadItemList();
        }
        
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
                    itempr = price;
                }
            }
        }


        public void ProcessPayment( )
        {
            string _userCash = varietystoreDao.GetUserInfo();
            int userCash = int.Parse(_userCash);
            int _NowCash = userCash - itempr;
            string NowCash = _NowCash.ToString();
            Debug.Log("계산 금액 " + itempr);

            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + NowCash);
            if (_NowCash > 0)
            {
                varietystoreDao.UpdateUserCash(NowCash);
                BuySuccessOn();
            }
            else
            {
                Debug.Log("Not enough cash!");
                BuyFailOn();

            }
        }
        
        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
        
        public void OpenBuy()
        {
            ActivateBuyMenu(); // 구매 메뉴를 엶
        }

        public void CloseBuy()
        {
            DeactivateBuyMenu   (); // 구매 메뉴를 엶
        }

        public GameObject VarietyStoreBuyBackGround; // 설정 패널 오브젝트

        private void ActivateBuyMenu()    //작동시 활성화
        {
            VarietyStoreBuyBackGround.SetActive(true);
        }

        private void DeactivateBuyMenu()   //작동시 비활성화
        {
            VarietyStoreBuyBackGround.SetActive(false);
        }


        public void OpenSell()
        {
            ActivateSellMenu(); // 판매 메뉴를 엶
        }

        public void CloseSell()
        {
            DeactivateSellMenu(); // 판매 메뉴를 엶
        }

        public GameObject VarietyStoreSellBackGround; // 설정 패널 오브젝트

   
        private void ActivateSellMenu()    //작동시 활성화
        {
            VarietyStoreSellBackGround.SetActive(true);
        }

        private void DeactivateSellMenu()   //작동시 비활성화
        {
            VarietyStoreSellBackGround.SetActive(false);
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

        public void Openingredients()
        {
            buyList.SetActive(true);

            foreach (GameObject buyListInstance in buyListInstancese)
            {
                Destroy(buyListInstance);
            }

            foreach (var dic in ItemList)
            {
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
        
        public void Opengift()
        {
            buyList.SetActive(true);

            foreach (GameObject buyListInstance in buyListInstancese)
            {
                Destroy(buyListInstance);
            }

            foreach (var dic in ItemList)
            {
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
    }
}
    
    
