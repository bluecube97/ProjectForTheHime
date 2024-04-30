using Script.UI.StartLevel.Dao;
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


    private RestaurantDao _rsetaurantDao;



    string foodName = "";
    int foodPrice = 0;
    int Uesrcash = 0;

    public void Awake()
    {
        _rsetaurantDao = GetComponent<RestaurantDao>(); // 현재 게임 오브젝트에 붙어 있는 RestaurantDao 스크립트를 가져옴

    }

    public void GetFoodList(List<Dictionary<string, object>> foodList)
    {

        int i = 0;
        _rsetaurantDao.OnClickEatMeueBtn();
        foreach (var dic in foodList)
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
        foodList.Clear();
    }

        public void GetclickFoodList(List<Dictionary<string, object>> foodList)
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
            Dictionary<string, object> foodInfo = foodList[index - 1]; // 인덱스는 1부터 시작하므로 -1 해줍니다.

            // 가져온 정보를 사용합니다.
            foodName = (string)foodInfo["FOODNM"];
            foodPrice = (int)foodInfo["FOODPRICE"];

        }
    }
