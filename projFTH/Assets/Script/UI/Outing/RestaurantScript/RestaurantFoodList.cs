using JetBrains.Annotations;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RestaurantFoodList : MonoBehaviour
{
    public GameObject foodListPrefab; // foodList 이미지 프리팹 참조
    public GameObject foodList; // foodList 이미지 참조
    public Transform foodListLayout; // foodList 들어갈 레이아웃 참조
   
    public List<Dictionary<string, object>> FoodList = new();
    public List<Dictionary<string, object>> UserInfo = new();
    string foodName="";
    int foodPrice=0;
    int Uesrcash = 0;
    string con = "Server=localhost;Database=testdb;Uid=root;Pwd=1234;Charset=utf8mb4";

    public void Start()
    {

        OnClickEatMeueBtn();
        int i = 0;

        foreach (var dic in FoodList)
        {
            i++;
           string a = i.ToString();
            // 이미지 프리팹 인스턴스화
            GameObject foodListInstance = Instantiate(foodListPrefab, foodListLayout);
            foodListInstance.name = "foodlist" + a;
            // 이미지 오브젝트에 딕셔너리 값 설정
            Text textComponent = foodListInstance.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = dic["FOODNM"] + "\r\n" + " " + dic["FOODPRICE"];
                
            }
        }
       

        foodList.SetActive(false);
    }

    public void OnClickEatMeueBtn()
    {

        var sql = "SELECT gr.FOODNM , gr.FOODPRICE " +
                    "FROM game_restaurant gr ";
        using (MySqlConnection connection = new MySqlConnection(con))
        {
            connection.Open();
            using (MySqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> dic = new();
                        foodName = (string)reader["FOODNM"];
                        foodPrice = (int)reader["FOODPRICE"];
                        dic.Add("FOODNM", foodName);
                        dic.Add("FOODPRICE", foodPrice);

                        FoodList.Add(dic);
                    }
                }
            }
        }
    }

    public void GetclickFoodList()
    {
       // 이벤트 시스템에서 현재 선택된 게임 오브젝트를 가져옵니다.
    GameObject clickList = EventSystem.current.currentSelectedGameObject;

        // 클릭된 게임 오브젝트의 이름을 가져옵니다.
        string objectName = clickList.name;

        // "foodlist"를 제거하고 인덱스만 남깁니다.
        string indexString = objectName.Replace("foodlist", "");

        // 가져온 인덱스 문자열을 정수로 변환합니다.
        int index = int.Parse(indexString);

        // 해당 인덱스에 해당하는 음식 정보를 가져옵니다.
        Dictionary<string, object> foodInfo = FoodList[index - 1]; // 인덱스는 1부터 시작하므로 -1 해줍니다.

        // 가져온 정보를 사용합니다.
        string foodName = (string)foodInfo["FOODNM"];
        foodPrice = (int)foodInfo["FOODPRICE"];

    }

    public void GetUserInfo()
    {
        var sql = "  SELECT gu.USERCASH " +
                 "   FROM game_userinfo gu ";
        using (MySqlConnection connection = new MySqlConnection(con))
        {
            connection.Open();
            using (MySqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Uesrcash = reader.GetInt32(0);
                        Debug.Log("DB값은 "+Uesrcash);
                    }
                }
            }
        }
    }

    
    public void PaymentFood()
    {
        GetUserInfo();
 

        int payment = Uesrcash - foodPrice; // 사용자 캐시에서 음식 가격 차감
        Debug.Log("소지금 " +payment);

        if (payment > 0)
        {

            using (MySqlConnection connection = new MySqlConnection(con))
            {
                connection.Open();
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    var sql = " update game_userinfo " +
                                 " set USERCASH = (@payment)" +
                               " where USERNO = 1 ";
                    // DB에 유저 정보 저장
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@payment", payment);
                    cmd.ExecuteNonQuery();
                }
            }

        }
        if(payment < 0)
        {
            Debug.Log("소지금이 부족합니다");
        }

    }

   
}