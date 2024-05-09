using System.Collections.Generic;
using System.Data.SqlTypes;
using Script.UI.Outing;
using Script.UI.StartLevel.Dao;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;

public class HospitalManager : MonoBehaviour
{
    public GameObject HospitalPrefab; 
    public GameObject Hospital; 
    public Transform HospitalLayout; 
    private List<GameObject> HospitalInstances = new List<GameObject>();
    private List<Dictionary<string,object>> SellList = new List<Dictionary<string,object>>();   

    private HospitalDao hospitalDao;
    private Dictionary<string,object> dic = new Dictionary<string,object>();
    private void Start()
    {
        hospitalDao = GetComponent<HospitalDao>(); 
        SellList = hospitalDao.getSellList();

    }
    public void OnclickSellList()
    {
        foreach (var dic in SellList)
        {
            GameObject hospitalInstances = Instantiate(HospitalPrefab, HospitalLayout);
            hospitalInstances.name = "itemList" + dic["itemNo"];
            Text textComponent = hospitalInstances.GetComponentInChildren<Text>();

            if (textComponent != null)
            {
                textComponent.text = dic["itemNm"] + "\r\n" 
                           +"\r\n" + dic["itemDesc"] + "\r\n" 
               +"\r\n" + "가격 : " + dic["itemPrice"];
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
        int index = int.Parse(indexString);
        var dic = SellList[index - 1];
        int price = (int)dic["itemPrice"];
        ProcessPayment(price);
    }
    public void ProcessPayment(int price)
    {
        dic = hospitalDao.GetUserInfo();
        int userCash = (int)dic["userCash"];
        int NowCash = userCash - price;
        Debug.Log("계산 금액 " + price);

        Debug.Log("DB 유저 현금 " + userCash);
        Debug.Log("계산 후 금액 " + NowCash);
        if (NowCash > 0)
        {
            hospitalDao.SetBuyAfter(NowCash);
        }
        else
        {
            Debug.Log("Not enough cash!");
        }
    }
    public void OnclikHealing()
    {
        dic = hospitalDao.GetUserInfo();
        int userCash = (int)dic["userCash"];
        int userMaxHP = (int)dic["userMaxHP"];
        int userHP = (int)dic["userHP"];
        int payCash = userCash - ((userMaxHP - userHP)*10);

        hospitalDao.SetAfterHeal(payCash, userMaxHP);
    }
}