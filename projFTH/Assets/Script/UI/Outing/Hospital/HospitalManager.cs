using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI.Outing.Hospital
{
    public class HospitalManager : MonoBehaviour
    {
        public GameObject HospitalPrefab;
        public GameObject Hospital;
        public Transform HospitalLayout;
        private Dictionary<string, object> dic = new();

        private HospitalDao hospitalDao;
        private List<GameObject> HospitalInstances = new();
        private List<Dictionary<string, object>> SellList = new();

        private void Start()
        {
            hospitalDao = GetComponent<HospitalDao>();
            SellList = hospitalDao.getSellList();
        }

        public void OnclickSellList()
        {
            foreach (Dictionary<string, object> dic in SellList)
            {
                GameObject hospitalInstances = Instantiate(HospitalPrefab, HospitalLayout);
                hospitalInstances.name = "itemList" + dic["itemNo"];
                Text textComponent = hospitalInstances.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = dic["itemNm"] + "\r\n"
                                                       + "\r\n" + dic["itemDesc"] + "\r\n"
                                                       + "\r\n" + "가격 : " + dic["itemPrice"];
                }
            }

            Hospital.SetActive(false);
        }

        public void GetclickListValue()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            string indexString = parentObjectName.Replace("itemList", "");
            Dictionary<string, object> selectedItem = SellList.Find
                (dic => dic["itemNo"].ToString() == indexString);

            if (selectedItem != null && selectedItem.TryGetValue("itemPrice", out object priceObj) 
                                     && priceObj is string priceStr
                                     && int.TryParse(priceStr, out int price))
            {
                Debug.Log(selectedItem);
                ProcessPayment(price);
            }
        }

        public void ProcessPayment(int price)
        {
            dic = hospitalDao.GetUserInfo();
            string _userCash = (string)dic["userCash"];
            int userCash = int.Parse(_userCash);
            int NowCash = userCash - price;
            string _NowCash = NowCash.ToString();
            Debug.Log("계산 금액 " + price);

            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + NowCash);
            if (NowCash > 0)
            {
                hospitalDao.SetBuyAfter(_NowCash);
            }
            else
            {
                Debug.Log("Not enough cash!");
            }
        }

        public void OnclikHealing()
        {
            dic = hospitalDao.GetUserInfo();
            int userCash = int.Parse((string)dic["userCash"]);
            int userMaxHP = int.Parse((string)dic["userMaxHP"]);
            int userHP = int.Parse((string)dic["userHP"]);
            int _payCash = userCash - ((userMaxHP - userHP) * 10);
            string payCash = _payCash.ToString();
            string _userMaxHP = userMaxHP.ToString();
            Debug.Log("usercash"+userCash);
            Debug.Log("userMaxHP"+userMaxHP);
            Debug.Log("userHP"+userHP);

            hospitalDao.SetAfterHeal(payCash, _userMaxHP);
        }
    }
}