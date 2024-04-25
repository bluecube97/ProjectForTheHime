namespace Script.UI.Outing
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ClothingStoreManager : MonoBehaviour
    {
        private static ClothingStoreManager instance; // ESC메뉴의 인스턴스
        public GameObject MakeClothingMenu; // 옷 패널 오브젝트
        public GameObject SellMenu; // 판매 패널 오브젝트
        public GameObject BuyMenu; // 구매 패널 오브젝트

        private bool MakeClothingMenuActive; // 옷 화면 활성화 여부
        private bool SellMenuActive; // 판매 화면 활성화 여부
        private bool BuyMenuActive; // 구매 화면 활성화 여부

        public static ClothingStoreManager Instance
        {
            get
            {
                // 인스턴스가 없다면 새로 생성
                if (instance == null)
                {
                    instance = FindObjectOfType<ClothingStoreManager>();

                    // 씬에 메뉴가 없다면 새로 생성
                    if (instance == null)
                    {
                        var obj = new GameObject();
                        obj.name = "MakeClothingMenu";
                        obj.name = "SellMenu";
                        obj.name = "BuyMenu";

                        instance = obj.AddComponent<ClothingStoreManager>();
                    }
                }

                return instance;
            }
        }
        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
        public void OnClickMakeClothinging()
        {

            ActivateMakeClothingMenu();
        }
        public void OnClickMakeClothingOuting()
        {
            DeactivateMakeClothingMenu();
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





        private void ActivateMakeClothingMenu()
        {
            MakeClothingMenu.SetActive(true);
        }

        private void DeactivateMakeClothingMenu()
        {
            MakeClothingMenu.SetActive(false);
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
