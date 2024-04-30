using Script.UI.Outing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



    public class RestaurantManager : MonoBehaviour
{
    public GameObject foodListPrefab;
    public GameObject foodList; 
    public Transform foodListLayout;

    private PointerEventData eventData;

    private RestaurantDao restaurantDao;
    private RestaurantUIController RestaurantUIController;

    string foodName = "";
    int foodPrice = 0;
    private void Start()
    {
        restaurantDao = GetComponent<RestaurantDao>(); // RestaurantDao 컴포넌트를 가져와서 초기화합니다.
        RestaurantUIController = FindObjectOfType<RestaurantUIController>(); // RestaurantManager를 찾아서 초기화합니다.

        var FoodList = restaurantDao.GetFoodListFromDB();

        int i = 0;

        foreach (var dic in FoodList)
        {
            i++;
            string i_ = i.ToString();
            GameObject foodListInstance = Instantiate(foodListPrefab, foodListLayout);
            foodListInstance.name = "foodlist" + i_;
            Text textComponent = foodListInstance.GetComponentInChildren<Text>();

            if (textComponent != null)
            {
                textComponent.text = dic["FOODNM"] + "\r\n" +
                               " " + dic["FOODPRICE"];

            }
        }
        foodList.SetActive(false);
    }

    public void GetclickFoodList()
    {
        GameObject clickList = EventSystem.current.currentSelectedGameObject;
        var FoodList = restaurantDao.GetFoodListFromDB();

        string objectName = clickList.name;

        string indexString = objectName.Replace("foodlist", "");

        int index = int.Parse(indexString);

        Dictionary<string, object> foodInfo = FoodList[index - 1];
        foodName = (string)foodInfo["FOODNM"];
        foodPrice = (int)foodInfo["FOODPRICE"];
       
        
        Debug.Log("이름 " + foodName);
        Debug.Log("가격 " + foodPrice);

    }
        public void ProcessPayment()
    {
        int userCash = restaurantDao.GetUserInfoFromDB();
        int NowCash = userCash - foodPrice;
        Debug.Log("DB 유저 현금 " +userCash);
        Debug.Log("계산 후 금액 " + NowCash);
        if (NowCash >= 0)
        {
            restaurantDao.UpdateUserCash(NowCash);
            RestaurantUIController.OnClickBuyComple();
        }
        else
        {
            Debug.Log("Not enough cash!");
            RestaurantUIController.OnClickBuyFail();

        }
    }
}
