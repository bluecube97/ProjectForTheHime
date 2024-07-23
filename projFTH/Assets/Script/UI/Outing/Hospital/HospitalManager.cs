using Script.UI.MainLevel.Inventory;
using Script.UI.StartLevel.Dao;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI.Outing.Hospital
{
    public class HospitalManager : MonoBehaviour
    {
        // 게임 오브젝트를 담은 통
        private HospitalGo _hpgo;
        // DB값을 담는 통
        private HospitalVo _hpvo;
        // 인벤토리 값을 담음
        private InventoryDao inventoryDao;
        private HospitalDao hospitalDao;
        private StartLevelDao _sld; // StartLevelDao를 사용하기 위한 변수

        private string pid;

        private void Start()
        {
            // 각종 인스턴스 초기화
            _hpgo = new HospitalGo();
            _hpvo = new HospitalVo();
            inventoryDao = GetComponent<InventoryDao>();
            hospitalDao = GetComponent<HospitalDao>();
            _sld = GetComponent<StartLevelDao>();

            // 서버에서 BuyList 데이터 가져오기
            StartCoroutine(hospitalDao.GetBuyLists(list =>
            {
                _hpvo.BuyList = list;
            }));

            // 서버에서 유저 데이터 가져오기
            StartCoroutine(_sld.GetUserEmail(info =>
            {
                _hpvo.Userinfo = info;
                pid = _hpvo.Userinfo["useremail"].ToString();
                // 서버에서 인벤토리 데이터 가져오기
                StartCoroutine(inventoryDao.GetInventoryList(pid, list =>
                {
                    _hpvo.inventoryList = list;
                    SetSellList(list);
                }));
            }));

            // 활성화 되어있는 메뉴들 비활성화
            _hpgo.BuyMenu.SetActive(false);
            _hpgo.CureMenu.SetActive(false);
            _hpgo.ChoiceMenu.SetActive(false);
            _hpgo.SellChoiceMenu.SetActive(false);
            _hpgo.SellMenu.SetActive(false);
            _hpgo.SellComplete.SetActive(false);
            _hpgo.SellFail.SetActive(false);
        }

        // 구매하기 버튼 클릭 시
        public void OnclickBuyList()
        {
            // 구매 목록 활성화
            _hpgo.BuyMenu.SetActive(true);

            // 구매 목록에 들어갈 값 세팅
            foreach (Dictionary<string, object> dic in _hpvo.BuyList)
            {
                // 구매 목록 인스턴스화
                _hpgo.BuyListInstances = Instantiate(_hpgo.BuyListPrefab, _hpgo.BuyListLayout.transform);

                // 각각의 목록에 이름 부여
                _hpgo.BuyListInstances.name = "itemList" + dic["itemid"];

                Text textComponent = _hpgo.BuyListInstances.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = dic["itemnm"] + "\r\n"
                                       + "\r\n" + dic["itemdesc"] + "\r\n"
                                       + "\r\n" + "가격 : " + dic["buyprice"];
                }
            }

            _hpgo.BuyList.SetActive(false);
        }

        // 판매 목록을 설정하는 메서드
        private void SetSellList(List<Dictionary<string, object>> sellList)
        {
            _hpgo.SellList.SetActive(true);

            foreach (GameObject sellListInstance in _hpgo.sellListInstances)
            {
                Destroy(sellListInstance);
            }

            _hpgo.sellListInstances.Clear();

            foreach (Dictionary<string, object> cls in sellList)
            {
                GameObject sellListInstance = Instantiate(_hpgo.SellListPrefab, _hpgo.SellLayout.transform);
                cls.TryGetValue("itemid", out object itemId);
                sellListInstance.name = "itemList" + itemId;
                _hpgo.sellListInstances.Add(sellListInstance);

                Text textComponent = sellListInstance.GetComponentInChildren<Text>();
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

            _hpgo.SellList.SetActive(false);
        }

        // 구매 버튼 클릭 시 호출되는 메서드
        public void GetclickListValue()
        {
            // 클릭한 버튼 값 받아옴
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            // 클릭한 버튼의 부모 게임오브젝트 담음
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            // 부모 오브젝트의 이름을 담음
            string parentObjectName = parentObject.name;
            // 부모 오브젝트 이름에서 dic["itemNo"]를 추출
            string indexString = parentObjectName.Replace("itemList", "");
            // 구매목록에서 추출한 이름과 동일한 이름을 가진 값을 찾아 담음 
            Dictionary<string, object> selectedItem = _hpvo.BuyList.Find(dic => dic["itemid"].ToString() == indexString);

            // 담은 값에서 itemPrice 의 값을 찾아 형 변환 후 반환
            if (selectedItem != null && selectedItem.TryGetValue("buyprice", out object priceObj)
                                     && priceObj is string priceStr
                                     && int.TryParse(priceStr, out int price))
            {
                Debug.Log(price);
                Debug.Log(indexString);
                // 반환된 값을 vo에 담음
                _hpvo.price = price;
                _hpvo.itemid = indexString;
            }
        }

        // 판매 버튼 클릭 시 호출되는 메서드
        public void GetSellValue()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            _hpvo.itemid = parentObjectName.Replace("itemList", "");
            Debug.Log("판매할 아이템 아이디 " + _hpvo.itemid);
            Dictionary<string, object> clv = _hpvo.inventoryList.Find(p => p["itemid"].ToString() == _hpvo.itemid);
            Debug.Log("판매할 아이템 아이디 값" + clv);

            clv.TryGetValue("sellprice", out object tempSellPrice);
            _hpvo.sellprice = tempSellPrice?.ToString();

            Debug.Log(_hpvo.price);
        }

        // 구매 시 결제하는 메서드
        public void ProcessPayment()
        {
            StartCoroutine(ProcessPaymentCoroutine());
        }

        private IEnumerator ProcessPaymentCoroutine()
        {
            // 서버에서 유저 데이터 가져오기
            bool userInfoFetched = false;
            StartCoroutine(_sld.GetUser(pid, list =>
            {
                _hpvo.Userinfo = list;
                userInfoFetched = true;
            }));
            yield return new WaitUntil(() => userInfoFetched);

            // 유저 정보에서 보유현금 담음
            string _userCash = (string)_hpvo.Userinfo["cash"];
            // 계산을 위한 형변환
            int userCash = int.Parse(_userCash);
            // GetclickListValue에서 담은 값과 유저현금을 계산
            int NowCash = userCash - _hpvo.price;
            // DB업데이트를 위한 형변환
            string _NowCash = NowCash.ToString();
            Debug.Log("계산 금액 " + _hpvo.price);

            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + NowCash);

            // 계산된 값이 0이상이면
            if (userCash > _hpvo.price)
            {
                // 인벤토리DB에 구매 할 아이템 보유 여부 확인, 있다면 값을 담음
                StartCoroutine(inventoryDao.UpdateUserCashs(pid, _NowCash));

                Dictionary<string, object> buyitem =
                    _hpvo.inventoryList.Find(p => p["itemid"].ToString().Equals(_hpvo.itemid));

                if (buyitem != null)
                {
                    string _cnt = buyitem["itemcnt"].ToString();
                    int cnt = int.Parse(_cnt);
                    int _bitem = cnt + 1;
                    string bitem = _bitem.ToString();

                    StartCoroutine(inventoryDao.UpdateBuyThings(bitem, _hpvo.itemid, pid));

                }
                // 구매한 물품이 인벤토리에 없다면
                else
                {
                    string cnt = "1";
                    StartCoroutine(inventoryDao.InsertBuyThings(_hpvo.itemid, cnt, pid));
                }

                // 인벤토리 목록 업데이트
                bool inventoryFetched = false;
                StartCoroutine(inventoryDao.GetInventoryList(pid, list =>
                {
                    _hpvo.inventoryList = list;
                    inventoryFetched = true;
                }));

                yield return new WaitUntil(() => inventoryFetched);
            }
            else
            {
                Debug.Log("Not enough cash!");
            }
        }

        // 치료하기 버튼 클릭 시
        public void OnclikHealing()
        {
            StartCoroutine(OnclikHealingCoroutine());
        }

        private IEnumerator OnclikHealingCoroutine()
        {
            StartCoroutine(_sld.GetUser(pid, list =>
            {
                _hpvo.Userinfo = list;
            }));
            Debug.Log("씨방 왜 안됨? " + _hpvo.Userinfo["cash"]);
            // 계산을 위한 형변환
            Debug.Log("파싱전 " + _hpvo.Userinfo["cash"]);

            int userCash = int.Parse((string)_hpvo.Userinfo["cash"]);
            int userMaxHP = int.Parse((string)_hpvo.Userinfo["maxhp"]);
            int userHP = int.Parse((string)_hpvo.Userinfo["chp"]);
            Debug.Log("usercash" + userCash);
            Debug.Log("userMaxHP" + userMaxHP);
            Debug.Log("userHP" + userHP);

            // 치료에 따른 지불값 설정 후 계산
            int _payCash = userCash - ((userMaxHP - userHP) * 10);
            if (_payCash > 0)
            {
                // DB에 값을 넣기 위한 형변환
                string payCash = _payCash.ToString();
                string _userMaxHP = userMaxHP.ToString();
                Debug.Log("usercash" + userCash);
                Debug.Log("userMaxHP" + userMaxHP);
                Debug.Log("userHP" + userHP);
                StartCoroutine(hospitalDao.SetAfterHeals(pid, payCash, _userMaxHP));

            }
            else
            {
                Debug.Log("돈이 부족하시네요");
            }

            // 서버에서 유저 데이터 가져오기
            bool userInfoFetched = false;
            StartCoroutine(_sld.GetUser(pid, list =>
            {
                _hpvo.Userinfo = list;
                userInfoFetched = true;
            }));
            yield return new WaitUntil(() => userInfoFetched);
        }

        // 돌아가기 버튼 클릭 시
        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }

        // 판매하는 메서드
        public void SellThing()
        {
            // 인벤토리 목록과 유저 정보를 동기적으로 가져옴
            StartCoroutine(SellThingCoroutine());
        }

        private IEnumerator SellThingCoroutine()
        {
            // 유저 정보 가져오기
            bool userInfoFetched = false;
            int cash = 0;
            StartCoroutine(_sld.GetUser(pid, userinfo =>
            {
                cash = int.Parse((string)userinfo["cash"]);
                userInfoFetched = true;
            }));

            yield return new WaitUntil(() => userInfoFetched);

            int price = int.Parse(_hpvo.sellprice);
            Dictionary<string, object> checkVal =
                _hpvo.inventoryList.Find(dic => dic["itemid"].ToString().Equals(_hpvo.itemid));
            if (checkVal != null)
            {
                OnClickSellComplete();
                int payment = cash + price;
                string result = payment.ToString();

                string _cnt = checkVal["itemcnt"].ToString();
                int cnt = int.Parse(_cnt);
                int _bitem = cnt - 1;
                string bitem = _bitem.ToString();

                // 유저 현금 업데이트
                yield return StartCoroutine(inventoryDao.UpdateUserCashs(pid, result));

                // 판매된 아이템 업데이트
                yield return StartCoroutine(inventoryDao.UpdateSellThings(bitem, _hpvo.itemid, pid));

                // 판매 후 업데이트된 인벤토리 목록 가져오기
                bool updatedInventoryFetched = false;
                StartCoroutine(inventoryDao.GetInventoryList(pid, updatedList =>
                {
                    _hpvo.inventoryList = updatedList;
                    updatedInventoryFetched = true;
                }));

                yield return new WaitUntil(() => updatedInventoryFetched);

                // 판매 목록 UI 업데이트
                SetSellList(_hpvo.inventoryList);
            }
            else
            {
                OnClickSellFail();
            }
        }

        // 메뉴를 on, off하는 메서드
        public void ToggleMenu(GameObject menu, bool isActive)
        {
            menu.SetActive(isActive);
        }

        // 메서드 호출 시 toggleMenu에 전달
        public void OnClickCure() => ToggleMenu(_hpgo.CureMenu, true);
        public void OnClickCureOut() => ToggleMenu(_hpgo.CureMenu, false);
        public void OnClickChoice() => ToggleMenu(_hpgo.ChoiceMenu, true);
        public void OnClickChoiceOuting() => ToggleMenu(_hpgo.ChoiceMenu, false);
        public void OnClickSellChoice() => ToggleMenu(_hpgo.SellChoiceMenu, true);
        public void OnClickSellChoiceOuting() => ToggleMenu(_hpgo.SellChoiceMenu, false);
        public void OnClickBuying() => ToggleMenu(_hpgo.BuyMenu, true);
        public void OnClickBuyOuting() => ToggleMenu(_hpgo.BuyMenu, false);
        public void OnClickSelling() => ToggleMenu(_hpgo.SellMenu, true);
        public void OnClickSellOuting() => ToggleMenu(_hpgo.SellMenu, false);
        public void OnClickSellComplete() => ToggleMenu(_hpgo.SellComplete, true);
        public void OnClickSellCompleteOut() => ToggleMenu(_hpgo.SellComplete, false);
        public void OnClickSellFail() => ToggleMenu(_hpgo.SellFail, true);
        public void OnClickSellFailOut() => ToggleMenu(_hpgo.SellFail, false);
    }
}
