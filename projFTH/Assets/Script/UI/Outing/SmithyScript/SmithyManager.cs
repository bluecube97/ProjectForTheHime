
    using global::System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SmithyManager : MonoBehaviour
    {
        private static SmithyManager instance; 
        public GameObject SmeltMenu;
        public GameObject SellMenu;
        public GameObject BuyMenu;

        private bool SmeltMenuActive; 
        private bool SellMenuActive;
        private bool BuyMenuActive; 

        public static SmithyManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SmithyManager>();

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
            ActivateSellMenu();
        }
       
        public void OnClickSellOuting()
        {
            DeactivateSellMenu();
        }

        public void OnClickBuying()
        {
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


