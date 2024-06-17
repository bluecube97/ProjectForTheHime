using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI.Outing.RestaurantScript
{
    public class RestaurantManager : MonoBehaviour
    {
        public GameObject foodListPrefab; //foodlist 이미지 참조
        public GameObject foodList; //foodlist 이미지 참조
        public Transform foodListLayout; //foodlistLayout 이미지 참조

        private List<FoodListVO> FoodList; //List 형식으로 foodlist를 담음
        private FoodListVO foodlistVO; // foodlist를 담음
        private RestaurantDao restaurantDao;
        private RestaurantUIController RestaurantUIController;
        private string FoodPr; //음식 가격을 담음

        private void Start()
        {
            restaurantDao = GetComponent<RestaurantDao>(); // RestaurantDao 컴포넌트를 가져와서 초기화합니다.
            RestaurantUIController = FindObjectOfType<RestaurantUIController>(); // RestaurantManager를 찾아서 초기화합니다.
            FoodList = restaurantDao.GetFoodListFromDB(); //DB에 저장되어 있는 음식을 담음
        }

        //밥먹기 클릭 시 실행되는 메서드
        public void OnclickFoodList()
        {
            foreach (FoodListVO dic in FoodList)
            {
                //음식 목록 인스턴스화
                GameObject foodListInstance = Instantiate(foodListPrefab, foodListLayout);
                
                //각각의 목록에 이름 부여
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

        //버튼 클릭 시
        public void GetclickListValue()
        {
            //클릭한 버튼 값 받아옴
            GameObject clickList = EventSystem.current.currentSelectedGameObject;
            
            //오브젝트의 이름을 담음
            string objectName = clickList.name;
          
            // dic["itemNo"]를 추출
            string indexString = objectName.Replace("foodlist", "");
           
            //FoodList에서 indexString과 동일한 이름이 들어있는 인덱스에 저장되어 있는 값 담음
            FoodListVO fv = FoodList.Find(p => p.FoodNo == indexString);
           
            //추출한 값을 전역변수로 담음
            FoodPr = fv.FoodPr;
            
            Debug.Log("계산 금액 " + FoodPr);
        }

        //결제하는 구문
        public void ProcessPayment()
        {
            //유저 정보에서 보유현금 담음
            string _userCash = restaurantDao.GetUserInfoFromDB();
            
            //계산을 위한 형변환
            int userCash = int.Parse(_userCash);
            int _FoorPr = int.Parse(FoodPr);
            int _NowCash = userCash - _FoorPr;
            string NowCash = _NowCash.ToString();
            Debug.Log("계산 금액 " + FoodPr);

            Debug.Log("DB 유저 현금 " + userCash);
            Debug.Log("계산 후 금액 " + NowCash);
            //계산된 값이 0보다 크면
            if (_NowCash > 0)
            {
                //결제 된 금액을 업데이트 하고
                restaurantDao.UpdateUserCash(NowCash);
                //구매성공 UI를 연다
                RestaurantUIController.OnClickBuyComple();
            }
            else
            {
                Debug.Log("Not enough cash!");
                //구매 실패 UI를 염
                RestaurantUIController.OnClickBuyFail();
            }
        }
    }
}