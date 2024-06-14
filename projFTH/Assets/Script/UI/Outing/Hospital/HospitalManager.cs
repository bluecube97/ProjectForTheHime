using Script.UI.MainLevel.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Script.UI.Outing.Hospital
{
    public class HospitalManager : MonoBehaviour
    {
        //게임 오브젝트를 담은 통 
        private HospitalGo _hpgo;
        //DB값을 담는 통 
        private HospitalVo _hpvo;
        //인벤토리 값을 담음

        private InventoryDao inventoryDao;

        private HospitalDao hospitalDao;

        private void Start()
        {
            
            _hpgo = new HospitalGo();
            _hpvo = new HospitalVo();
            
            inventoryDao = GetComponent<InventoryDao>();
            hospitalDao = GetComponent<HospitalDao>();
            //구매목록을 담음
            _hpvo.BuyList = hospitalDao.getBuyList();
            //유저정보를 담음(현재 hp,maxHp, cash)
            _hpvo.Userinfo = hospitalDao.GetUserInfo();
            //인벤토리를 담음
            _hpvo.invenList = inventoryDao.GetInvenList();
            
            //활성화 되어있는 메뉴들 비활성화
            _hpgo.BuyMenu.SetActive(false);
            _hpgo.CureMenu.SetActive(false);
            _hpgo.ChoiceMenu.SetActive(false);
            _hpgo.SellMenu.SetActive(false);

        }

        //구매하기 버튼 클릭 시
        public void OnclickBuyList()
        {
            //구매매 목록 활성화
            _hpgo.BuyMenu.SetActive(true);
            
            //구매목록에 들어갈 값 세팅
            foreach (Dictionary<string, object> dic in _hpvo.BuyList)
            {
                //구매목록 인스턴스화
                _hpgo.BuyListInstances = Instantiate(_hpgo.BuyListPrefab, _hpgo.BuyListLayout.transform);
                
                //각각의 목록에 이름 부여
                _hpgo.BuyListInstances.name = "itemList" + dic["itemNo"];
                
                Text textComponent = _hpgo.BuyListInstances.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = dic["itemNm"] + "\r\n"
                                                       + "\r\n" + dic["itemDesc"] + "\r\n"
                                                       + "\r\n" + "가격 : " + dic["itemPrice"];
                }
            }

            _hpgo.BuyList.SetActive(false);
        }
        
        //구매 버튼 클릭 시 
        public void GetclickListValue()
        {
            //클릭한 버튼 값 받아옴
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            //클릭한 버튼의 부모 게임오브젝트 담음
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            //부모 오브젝트의 이름을 담음
            string parentObjectName = parentObject.name;
            //부모 오브젝트 이름에서 dic["itemNo"]를 추출
            string indexString = parentObjectName.Replace("itemList", "");
            //구매목록에서 추출한 이름과 동일한 이름을 가진 값을 찾아 담음 
            Dictionary<string, object> selectedItem = _hpvo.BuyList.Find
                (dic => dic["itemNo"].ToString() == indexString);

            //담은 값에서 itemPrice 의 값을 찾아 형 변환 후 반환
            if (selectedItem != null && selectedItem.TryGetValue("itemPrice", out object priceObj) 
                                     && priceObj is string priceStr
                                     && int.TryParse(priceStr, out int price))
            {
                Debug.Log(price);
                Debug.Log(indexString);
                //반환된 값을 vo에 담음
                _hpvo.price = price;
                _hpvo.itemid = indexString;
            }
        }
        
        //구매 시 결제하는 구문
        public void ProcessPayment()
        {
            //유저 정보에서 보유현금 담음
            string _userCash = (string)_hpvo.Userinfo["userCash"];
            //계산을 위한  형변환
            int userCash = int.Parse(_userCash);
            //GetclickListValue에서 담은 값과 유저현금을 계산
            int NowCash = userCash - _hpvo.price;
            //DB업데이트를 위한 형변환
            string _NowCash = NowCash.ToString();
            Debug.Log("계산 금액 " + _hpvo.price);

            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + NowCash);
            //계산된 값이 0이상이면
            if (NowCash > 0)
            {
                //인벤토리DB에 구매 할 아이템 보유 여부 확인, 있다면 값을 담음
                InventoryVO buyitem = _hpvo.invenList.Find(p => p.ItemNo.Equals(_hpvo.itemid));
                hospitalDao.SetBuyAfter(_NowCash);
                if (buyitem == null)
                {
                    //DB에서 insert로 값을 넣어줌
                    inventoryDao.InsertBuyThing(_hpvo.itemid);
                    //inventory 갱신
                    _hpvo.invenList = inventoryDao.GetInvenList();
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
                    inventoryDao.UpdateBuyThing(bitem, _hpvo.itemid);
                    //inventory 갱신
                    _hpvo.invenList = inventoryDao.GetInvenList();
                }
            }
            else
            {
                Debug.Log("Not enough cash!");
            }
        }

        //치료하기 버튼 클릭 시
        public void OnclikHealing()
        {
            //계산을 위한 형변환
            int userCash = int.Parse((string)_hpvo.Userinfo["userCash"]);
            int userMaxHP = int.Parse((string)_hpvo.Userinfo["userMaxHP"]);
            int userHP = int.Parse((string)_hpvo.Userinfo["userHP"]);
            //치료에 따른 지불값 설정 후 계산
            int _payCash = userCash - ((userMaxHP - userHP) * 10);
            if (_payCash > 0)
            {
                //DB에 값을 넣기 위한 형변환
                string payCash = _payCash.ToString();
                string _userMaxHP = userMaxHP.ToString();
                Debug.Log("usercash"+userCash);
                Debug.Log("userMaxHP"+userMaxHP);
                Debug.Log("userHP"+userHP);

                hospitalDao.SetAfterHeal(payCash, _userMaxHP);
            }
            else
            {
                Debug.Log("돈이 부족하시네요");
            }
           
        }
        
        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
        
        //메뉴를 on,off하는 구문
        public void ToggleMenu(GameObject menu, bool isActive)
        {
            menu.SetActive(isActive);
            
        }
        
        //메서드 호출 시 toggleMenu에 날라감
        public void OnClickCure() => ToggleMenu(_hpgo.CureMenu, true);
        public void OnClickCureOut() => ToggleMenu(_hpgo.CureMenu, false);
        public void OnClickChoice() => ToggleMenu(_hpgo.ChoiceMenu, true);
        public void OnClickChoiceOuting() => ToggleMenu(_hpgo.ChoiceMenu, false);
        public void OnClickBuying() => ToggleMenu(_hpgo.BuyMenu, true);
        public void OnClickBuyOuting() => ToggleMenu(_hpgo.BuyMenu, false);
        public void OnClickSelling() => ToggleMenu(_hpgo.SellMenu, true);
        public void OnClickSellOuting() => ToggleMenu(_hpgo.SellMenu, false);
    }
    }
