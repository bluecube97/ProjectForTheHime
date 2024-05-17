using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI.Outing.RestaurantScript
{
    public class RestaurantManager : MonoBehaviour
    {
        public GameObject foodListPrefab;
        public GameObject foodList;
        public Transform foodListLayout;

        private PointerEventData eventData;
        private List<FoodListVO> FoodList;
        private FoodListVO foodlistVO;
        private RestaurantDao restaurantDao;
        private RestaurantUIController RestaurantUIController;
        private string FoodPr;

        private void Start()
        {
            restaurantDao = GetComponent<RestaurantDao>(); // RestaurantDao 컴포넌트를 가져와서 초기화합니다.
            RestaurantUIController = FindObjectOfType<RestaurantUIController>(); // RestaurantManager를 찾아서 초기화합니다.
            FoodList = restaurantDao.GetFoodListFromDB();
        }

        public void OnclickFoodList()
        {
            foreach (FoodListVO dic in FoodList)
            {
                GameObject foodListInstance = Instantiate(foodListPrefab, foodListLayout);
                foodListInstance.name = "foodlist" + dic.FoodNo;
                Text textComponent = foodListInstance.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = dic.FoodNm + "\r\n" +
                                         " " + dic.FoodPr;
                }
            }

            foodList.SetActive(false);
        }

        public void GetclickListValue()
        {
            GameObject clickList = EventSystem.current.currentSelectedGameObject;
            string objectName = clickList.name;
            string indexString = objectName.Replace("foodlist", "");
            FoodListVO fv = FoodList.Find(p => p.FoodNo == indexString);
            FoodPr = fv.FoodPr;
            Debug.Log("계산 금액 " + FoodPr);
        }

        public void ProcessPayment()
        {
            string _userCash = restaurantDao.GetUserInfoFromDB();
            int userCash = int.Parse(_userCash);
            int _FoorPr = int.Parse(FoodPr);
            int _NowCash = userCash - _FoorPr;
            string NowCash = _NowCash.ToString();
            Debug.Log("계산 금액 " + FoodPr);

            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + NowCash);
            if (_NowCash > 0)
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
}