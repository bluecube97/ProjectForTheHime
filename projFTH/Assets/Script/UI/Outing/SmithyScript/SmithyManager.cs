
    using global::System;
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SmithyManager : MonoBehaviour
    {
        private static SmithyManager instance; // ESC메뉴의 인스턴스
        public GameObject SmeltMenu; // 설정 패널 오브젝트
        public GameObject SellMenu; // 설정 패널 오브젝트
        public GameObject BuyMenu; // 설정 패널 오브젝트

        private bool SmeltMenuActive; // 설정 화면 활성화 여부
        private bool SellMenuActive; // 설정 화면 활성화 여부
        private bool BuyMenuActive; // 설정 화면 활성화 여부

        public static SmithyManager Instance
        {
            get
            {
                // 인스턴스가 없다면 새로 생성
                if (instance == null)
                {
                    instance = FindObjectOfType<SmithyManager>();

                    // 씬에 ESC메뉴가 없다면 새로 생성
                    if (instance == null)
                    {
                        var obj = new GameObject();
                        obj.name = "SmeltMenu";
                        obj.name = "SellMenu";
                        obj.name = "BuyMenu";

                        instance = obj.AddComponent<SmithyManager>();
                    }
                }

                return instance;
            }
        }
        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
        public void OnClickSmelting()
        {

            ActivateSmeltMenu();
        }
        public void OnClickSmeltOuting()
        {
            DeactivateSmeltMenu();
        }

        
        public void OnClickSelling()
        {
            // ESC메뉴가 활성화 되어있지 않다면
          
            ActivateSellMenu();
        }
        public void OnClickSellOuting()
        {
            DeactivateSellMenu();
        }

        
        public void OnClickBuying()
        {
            // ESC메뉴가 활성화 되어있지 않다면
            ActivateBuyMenu();
        }

        public void OnClickBuyOuting()
        {
            DeactivateBuyMenu();
        }

        



        private void ActivateSmeltMenu()
        {
            SmeltMenu.SetActive(true);
        }

        private void DeactivateSmeltMenu()
        {
            SmeltMenu.SetActive(false);
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


