using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI.Outing.Hospital
{
    public class HospitalManager : MonoBehaviour
    {
        private HospitalGo _hpgo;
        private HospitalVo _hpvo;

        private HospitalDao hospitalDao;

        private void Start()
        {
            _hpgo = new HospitalGo();
            _hpvo = new HospitalVo();

            hospitalDao = GetComponent<HospitalDao>();
            _hpvo.SellList = hospitalDao.getSellList();
            _hpvo.Userinfo = hospitalDao.GetUserInfo();
            _hpgo.BuyMenu.SetActive(false);
            _hpgo.CureMenu.SetActive(false);
            _hpgo.BuyMenu.SetActive(false);
            _hpgo.SellMenu.SetActive(false);

        }

        public void OnclickBuyList()
        {
            _hpgo.BuyMenu.SetActive(true);
            foreach (Dictionary<string, object> dic in _hpvo.SellList)
            {
                _hpgo.hospitalInstances = Instantiate(_hpgo.BuyListPrefab, _hpgo.BuyListLayout.transform);
                _hpgo.hospitalInstances.name = "itemList" + dic["itemNo"];
                Text textComponent = _hpgo.hospitalInstances.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = dic["itemNm"] + "\r\n"
                                                       + "\r\n" + dic["itemDesc"] + "\r\n"
                                                       + "\r\n" + "가격 : " + dic["itemPrice"];
                }
            }

            _hpgo.BuyList.SetActive(false);
        }

        public void GetclickListValue()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            string indexString = parentObjectName.Replace("itemList", "");
            Dictionary<string, object> selectedItem = _hpvo.SellList.Find
                (dic => dic["itemNo"].ToString() == indexString);

            if (selectedItem != null && selectedItem.TryGetValue("itemPrice", out object priceObj) 
                                     && priceObj is string priceStr
                                     && int.TryParse(priceStr, out int price))
            {
                Debug.Log(selectedItem);
                _hpvo.price = price;
            }
        }

        public void ProcessPayment()
        {
            string _userCash = (string)_hpvo.Userinfo["userCash"];
            int userCash = int.Parse(_userCash);
            int NowCash = userCash - _hpvo.price;
            string _NowCash = NowCash.ToString();
            Debug.Log("계산 금액 " + _hpvo.price);

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
            int userCash = int.Parse((string)_hpvo.Userinfo["userCash"]);
            int userMaxHP = int.Parse((string)_hpvo.Userinfo["userMaxHP"]);
            int userHP = int.Parse((string)_hpvo.Userinfo["userHP"]);
            int _payCash = userCash - ((userMaxHP - userHP) * 10);
            string payCash = _payCash.ToString();
            string _userMaxHP = userMaxHP.ToString();
            Debug.Log("usercash"+userCash);
            Debug.Log("userMaxHP"+userMaxHP);
            Debug.Log("userHP"+userHP);

            hospitalDao.SetAfterHeal(payCash, _userMaxHP);
        }
        public void ToggleMenu(GameObject menu, bool isActive)
        {
            menu.SetActive(isActive);
        }

        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
        public void OnClickCure() => ToggleMenu(_hpgo.CureMenu, true);
        public void OnClickCureOut() => ToggleMenu(_hpgo.CureMenu, false);
        public void OnClickBuying() => ToggleMenu(_hpgo.BuyMenu, true);
        public void OnClickBuyOuting() => ToggleMenu(_hpgo.BuyMenu, false);
        public void OnClickSelling() => ToggleMenu(_hpgo.SellMenu, true);
        public void OnClickSellOuting() => ToggleMenu(_hpgo.SellMenu, false);
    }
    }
