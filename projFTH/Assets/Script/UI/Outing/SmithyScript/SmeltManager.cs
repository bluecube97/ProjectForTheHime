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
        public GameObject smeltListPrefab;
        public GameObject smeltList;
        public Transform smeltListLayout;

        public GameObject buyListPrefab;
        public GameObject buyList;
        public Transform buyListLayout;

        private List<InventoryVO> invenList = new();

        
        private SmeltDao smeltDao;
        private InventoryDao inventoryDao;

        private List<Dictionary<string, object>> SmeltList = new List<Dictionary<string, object>>();
        private List<Dictionary<string, object>> BuyList = new List<Dictionary<string, object>>();

        private List<GameObject> smeltListInstances = new List<GameObject>();
        private List<GameObject> buyListInstances = new List<GameObject>();

        private string itemid;
        private string reqitem;
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
                InventoryVO have =  invenList.Find(p => p.ItemNo.Equals(dic["req_item"]));
                string havecnt;
                if (have == null)
                {
                    havecnt = "0";
                }
                else
                {
                    havecnt = have.ItemCnt;
                }
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
        public void SetBuyList(List<Dictionary<string, object>> bList)
        {
            foreach (var dic in bList)
            {
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
        public void OnClickSubmit()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            itemid = parentObjectName.Replace("weaponlist", "");
            Dictionary<string, object> selectedItem = BuyList.Find
                (dic => dic["itemId"].ToString() == itemid);

            if (selectedItem != null && selectedItem.TryGetValue("itemSellPr", out object priceObj)
                                     && priceObj is string priceStr
                                     && int.TryParse(priceStr, out int price))
            {
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
            Dictionary<string, object> sw = SmeltList.Find
                (dic => dic["itemId"].ToString() == itemid);
            reqitem = sw["req_item"].ToString();
            reqitem_cnt = sw["req_itemcnt"].ToString();
            Debug.Log(reqitem);
            Debug.Log(reqitem_cnt);
        }
        public void ProcessPayment(int weaponPrice)
        {
            string _userCash = smeltDao.GetUserInfoFromDB();
            int _NowCash = int.Parse(_userCash) - weaponPrice;
            InventoryVO buyitem = invenList.Find(p => p.ItemNo.Equals(itemid));

            Debug.Log("DB 유저 현금 " + _userCash);
            Debug.Log("계산 후 금액 " + _NowCash);
            if (buyitem == null)
            {
                inventoryDao.InsertBuyThing(itemid);
                invenList = inventoryDao.GetInvenList();
            }
            else
            {
                string _item = buyitem.ItemCnt;
                int item = int.Parse(_item);
                string bitem = (item + 1).ToString();
                string NowCash = _NowCash.ToString();
                if (_NowCash > 0)
                {
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
                    Debug.Log("호갱님 가진것이 없으시네요");

                }
            }
        }
    }
}