using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VarietyStoreManager : MonoBehaviour
{   
    

    private string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

    
    public GameObject BuyListPrefab; // BUYList 이미지 프리팹 참조
    public GameObject buyList; // BUYList 이미지 참조
    public Transform buyListLayout; // BUYList들이 들어갈 레이아웃 참조
    public List<Dictionary<string, object>> BuyList = new(); // BUYList를 담는 딕셔너리 리스트


    public void Start()
    {
            LoadData();

         foreach (var dic in BuyList)
            {
                // 이미지 프리팹 인스턴스화
                GameObject buyListInstance = Instantiate(BuyListPrefab, buyListLayout);

                // 이미지 오브젝트에 딕셔너리 값 설정
                Text textComponent = buyListInstance.GetComponentInChildren<Text>();
                if (textComponent!= null)
                {
                    textComponent.text =  dic["ITEMNO"] + " : " + dic["ITEMNAME"];
                }
            }
            buyList.SetActive(false);        
    }

 public void LoadData()
    {
        using (var connection = new MySqlConnection(con))
        {   
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = " SELECT ITEMNO, ITEMNAME " +  
                                     " FROM varietystorebuylist ";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> dic = new();
                         dic.Add("ITEMNO", reader["ITEMNO"]);
                        dic.Add("ITEMNAME", reader["ITEMNAME"]);

                        BuyList.Add(dic);
                    }
                }
            }
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

    
} 