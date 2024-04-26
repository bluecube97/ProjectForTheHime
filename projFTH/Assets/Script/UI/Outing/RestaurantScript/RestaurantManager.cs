namespace Script.UI.Outing
{
    using global::System;
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class RestaurantManager : MonoBehaviour
    {
        private static RestaurantManager instance; // ESC메뉴의 인스턴스
        public GameObject EatMenu; // 식사 패널 오브젝트
        public GameObject SellMenu; // 판매 패널 오브젝트
        public GameObject BuyMenu; // 구매 패널 오브젝트

        private bool EatMenuActive; // 식사 화면 활성화 여부
        private bool SellMenuActive; // 판매 화면 활성화 여부
        private bool BuyMenuActive; // 구매 화면 활성화 여부

        public static RestaurantManager Instance
        {
            get
            {
                // 인스턴스가 없다면 새로 생성
                if (instance == null)
                {
                    instance = FindObjectOfType<RestaurantManager>();

                    // 씬에 메뉴가 없다면 새로 생성
                    if (instance == null)
                    {
                        var obj = new GameObject();
                        obj.name = "EatMenu";
                        obj.name = "SellMenu";
                        obj.name = "BuyMenu";

                        instance = obj.AddComponent<RestaurantManager>();
                    }
                }

                return instance;
            }
        }
        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
        public void OnClickEating()
        {

            ActivateEatMenu();
        }
        public void OnClickEatOuting()
            {
                DeactivateEatMenu();
            }

        
        public void OnClickSelling()
        {

            // 판매메뉴가 활성화 되어있지 않다면
          
                ActivateSellMenu();
            }
        public void OnClickSellOuting()

        {
            DeactivateSellMenu();
            }

        
        public void OnClickBuying()
        {

            // 구매메뉴가 활성화 되어있지 않다면
           
           
                ActivateBuyMenu();
            }
        public void OnClickBuyOuting()

        {
            DeactivateBuyMenu();
            }

        



        private void ActivateEatMenu()
        {
            EatMenu.SetActive(true);
        }

        private void DeactivateEatMenu()
        {
            EatMenu.SetActive(false);
        }
        private void ActivateSellMenu()
        {
            SellMenu.SetActive(true);
        }

        private void DeactivateSellMenu()
        {
            SellMenu.SetActive(false);
        }
        private void ActivateBuyMenu()
        {
            BuyMenu.SetActive(true);
        }

        private void DeactivateBuyMenu()
        {
            BuyMenu.SetActive(false);
        }
    }
}

