using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        private SmeltDao smeltDao;

        private List<Dictionary<string, object>> SmeltList = new List<Dictionary<string, object>>();
        private List<Dictionary<string, object>> BuyList = new List<Dictionary<string, object>>();

        private List<GameObject> smeltListInstances = new List<GameObject>();
        private List<GameObject> buyListInstances = new List<GameObject>();

        private void Start()
        {
            smeltDao = GetComponent<SmeltDao>();
            BuyList = smeltDao.GetBuyList();
            SmeltList = smeltDao.GetSmeltList();
            SelSmeltList(SmeltList);
            SetBuyList(BuyList);
        }

        //재련 리스트 출력(재료로 구매)
        public void SelSmeltList(List<Dictionary<string, object>> SmeltList)
        {
            smeltList.SetActive(true);
            foreach (GameObject smeltInstance in smeltListInstances)
            {
                Destroy(smeltInstance);
            }
            smeltListInstances.Clear();
            foreach (var dic in SmeltList)
            {
                GameObject smeltListInstance = Instantiate(smeltListPrefab, smeltListLayout);
                smeltListInstance.name = "SmeltList" + dic["itemNo"];
                smeltListInstances.Add(smeltListInstance);

                Text textComponent = smeltListInstance.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = dic["itemNm"] + "\r\n" +
                                         dic["itemDesc"]; /* + "\r\n"+
                                         "소재 :  " + dic["itemValNm"] + "\r\n" +
                                         "필요 갯수 : " + dic["itemValCnt"];*/

                }
            }
            smeltList.SetActive(false);
        }
        public void SetBuyList(List<Dictionary<string, object>> BuyList)
        {
            foreach (var dic in BuyList)
            {
                GameObject buyListInstance = Instantiate(buyListPrefab, buyListLayout);
                buyListInstance.name = "weaponlist" + dic["itemNo"];
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
            string indexString = parentObjectName.Replace("weaponlist", "");
            Dictionary<string, object> selectedItem = BuyList.Find
                (dic => dic["itemNo"].ToString() == indexString);

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
            string indexString = parentObjectName.Replace("weaponlist", "");
            int index = int.Parse(indexString);


        }
        public void ProcessPayment(int weaponPrice)
        {
            string _userCash = smeltDao.GetUserInfoFromDB();
            int userCash = int.Parse(_userCash);
            int NowCash = userCash - weaponPrice;
            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + NowCash);
            if (NowCash >= 0)
            {
                smeltDao.UpdateUserCash(NowCash);
            }
            else
            {
                Debug.Log("Not enough cash!");
            }
        }

    }
}