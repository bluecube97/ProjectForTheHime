using Script.UI.MainLevel.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

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

        //인벤토리 정보를 담음
        private List<InventoryVO> invenList = new();

        
        private SmeltDao smeltDao;
        private InventoryDao inventoryDao;

        //재련에 대한 정보 담음
        private List<Dictionary<string, object>> SmeltList = new List<Dictionary<string, object>>();
        //구매에 대한 정보 담음
        private List<Dictionary<string, object>> BuyList = new List<Dictionary<string, object>>();

        private List<GameObject> smeltListInstances = new List<GameObject>();
        private List<GameObject> buyListInstances = new List<GameObject>();
        
        //구매나 제련 시 얻게되는 아이템 아이디를 담음
        private string itemid;
        //재련 시 요구 아이템 아이디를 담음
        private string reqitem;
        //재련 시 요구 아이템의 갯수를 담음
        private string reqitem_cnt;

        private void Start()
        {
            smeltDao = GetComponent<SmeltDao>();
            inventoryDao = GetComponent<InventoryDao>();

            BuyList = smeltDao.GetBuyList();
            SmeltList = smeltDao.GetSmeltList();
            invenList = inventoryDao.GetInvenList();

            
            SelSmeltList(SmeltList);
            SetBuyList(BuyList);
        }

        //재련 리스트 출력(재료로 구매)
        public void SelSmeltList(List<Dictionary<string, object>> sList)
        {
            smeltList.SetActive(true);
            foreach (GameObject smeltInstance in smeltListInstances)
            {
                Destroy(smeltInstance);
            }
            smeltListInstances.Clear();
            foreach (var dic in sList)
            {
                //인벤토리에 제작 시 요구 아이템 보유 여부를 찾음
                InventoryVO have =  invenList.Find(p => p.ItemNo.Equals(dic["req_item"]));
                string havecnt; //소지갯수를 담음
                if (have == null)
                {
                    havecnt = "0";
                }
                else
                {
                    havecnt = have.ItemCnt;
                }
                //재련리스트 인스턴스화
                GameObject smeltListInstance = Instantiate(smeltListPrefab, smeltListLayout);
                smeltListInstance.name = "list" + dic["itemId"];
                smeltListInstances.Add(smeltListInstance);

                Text textComponent = smeltListInstance.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = dic["itemNm"] + "\r\n" +
                                         dic["itemDesc"] + "\r\n"+
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
            foreach (var dic in bList)
            {
                //구매 목록 인스턴스화
                GameObject buyListInstance = Instantiate(buyListPrefab, buyListLayout);
                buyListInstance.name = "weaponlist" + dic["itemId"];
                buyListInstances.Add(buyListInstance);
                Text textComponent = buyListInstance.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = dic["itemNm"] + "\r\n" +
                                         dic["itemDesc"] + "\r\n" +
                                         "가격 : " + dic["itemSellPr"];
                }
            }
            buyList.SetActive(false);
        }
        
        //구매하기 버튼 클릭 시 
        public void OnClickSubmit()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid = parentObjectName.Replace("weaponlist", "");
            
            //구매 목록 중 itemid와  동일한 item을 찾아 그 값을 담음
            Dictionary<string, object> selectedItem = BuyList.Find
                (dic => dic["itemId"].ToString() == itemid);

            //itemid 가 있다면 itemSellPr의 값을 추출해 int형으로 price에 담음
            if (selectedItem != null && selectedItem.TryGetValue("itemSellPr", out object priceObj)
                                     && priceObj is string priceStr
                                     && int.TryParse(priceStr, out int price))
            {
                //값 지불하는 메서드
                ProcessPayment(price);
            }
        }
        //구매 리스트 선택 시 구매 리스트 정보 받아오기(현금 구매)
        public void GetclickWeaponList()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid= parentObjectName.Replace("list", "");
            //SmeltList에 itmeid이 있는 지 찾아 값을 담음
            Dictionary<string, object> sw = SmeltList.Find
                (dic => dic["itemId"].ToString() == itemid);
            //요구 itemid를 담음
            reqitem = sw["req_item"].ToString();
            //요구 itemid의 갯수를 담음
            reqitem_cnt = sw["req_itemcnt"].ToString();
            Debug.Log(reqitem);
            Debug.Log(reqitem_cnt);
        }
        
        //구매 시 값을 지불하는 메서드
        public void ProcessPayment(int weaponPrice)
        {
            string _userCash = smeltDao.GetUserInfoFromDB();
            int _NowCash = int.Parse(_userCash) - weaponPrice;
            
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
                    smeltDao.UpdateUserCash(NowCash);
                    invenList = inventoryDao.GetInvenList();

                }
                else
                {
                    Debug.Log("Not enough cash!");
                }
            }
        }
        
        //재련 시 값을 지불하는 메서드
        public void ProcessPayItem()
        {
            //인벤토리에 구매 물품 보유 여부 확인
            InventoryVO checkVal = invenList.Find(p => p.ItemNo.Equals(itemid));
            
            //구매 시 소모 아이템 보유 여부, 갯수 확인
            InventoryVO giveitem = invenList.Find(p => p.ItemNo.Equals(reqitem));
            //아이템이 없으면
            if (giveitem == null)
            {
                Debug.Log("호갱님 가진것이 없으시네요");
            }
            else
            {
                //있다면 int로 변환 하고 계산 
                string gitemid = giveitem.ItemNo;
                string _gitemcnt = giveitem.ItemCnt;
                int gitemcnt = int.Parse(_gitemcnt);
                int ritemcnt = int.Parse(reqitem_cnt);
                int result = gitemcnt - ritemcnt;
                Debug.Log("계산 후 남은 갯수 " + result);
                //계산 시 로직
                if (result >= 0)
                {
                    //DB에 선언된 값이 varchar이기 때문에 String으로 변환
                    string _result = result.ToString();
                    //구매 후 물품 갯수 업데이트 
                    inventoryDao.ItemCraftPayment(gitemid, _result);
                    //구매한 물품 넣어주는 구문, DB에 insert와 update구문을 나누는 작업
                    //인벤에 템이 없다면 insert를 있다면 update 사용
                    if (checkVal != null)
                    {
                        string _cnt = checkVal.ItemCnt;
                        int cnt = int.Parse(_cnt);
                        int _uitem = cnt + 1;
                        string uitem = _uitem.ToString();
                        inventoryDao.ItemCraftUpdate(uitem, itemid);
                        invenList = inventoryDao.GetInvenList();

                    }
                    else
                    {
                        string cnt = "1";
                        Dictionary<string, object> sw = SmeltList.Find
                            (dic => dic["itemId"].ToString() == itemid);
                        inventoryDao.ItemCraftInsert(itemid,cnt);
                        invenList = inventoryDao.GetInvenList();

                    }
                }
                else
                {
                    Debug.Log("호갱님 재료가 부족해요");

                }
            }
        }
    }
}