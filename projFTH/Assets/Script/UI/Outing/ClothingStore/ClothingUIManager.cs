namespace Script.UI.Outing
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ClothingUIManager : MonoBehaviour
    {
        private static ClothingUIManager instance; // 메뉴의 인스턴스
        public GameObject MakeClothingMenu; // 옷 패널 오브젝트
        public GameObject SellMenu; // 판매 패널 오브젝트
        public GameObject BuyMenu; // 구매 패널 오브젝트
        public GameObject ChiceUi;
        public GameObject BuyChiceUi;
        public GameObject BuyComple;
        public GameObject BuyFail;
        private void Awake()
        {
            // 인스턴스가 없을 경우 현재 GameObject에 ClothingUIManager 추가합니다.
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        public static ClothingUIManager Instance => instance;

        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
        public void OnClickMakeClothinging()
        {

            ActivateMenu(MakeClothingMenu);
        }
        public void OnClickMakeClothingOuting()
        {
            DeactivateMenu(MakeClothingMenu);
        }


        public void OnClickSelling()
        {

            // 판매메뉴가 활성화 되어있지 않다면

            ActivateMenu(SellMenu);
        }
        public void OnClickSellOuting()

        {
            DeactivateMenu(SellMenu);
        }
        public void OnClickBuying()
        {

            ActivateMenu(BuyMenu);
        }
        public void OnClickBuyOuting()

        {
            DeactivateMenu(BuyMenu);
        }
        public void OnClickChiceUi()
        {
            ActivateMenu(ChiceUi);
        }
        public void OnClickChiceUiOut()
        {
            DeactivateMenu(ChiceUi);
        }
        public void OnClickBuyChiceUi()
        {
            ActivateMenu(BuyChiceUi);
        }
        public void OnClickBuyChiceUiOut()
        {
            DeactivateMenu(BuyChiceUi);
        }
        public void OnClickBuyComple()
        {
            ActivateMenu(BuyComple);
        }

        public void OnClickBuyCompleOut()
        {
            DeactivateMenu(BuyComple);
            SceneManager.LoadScene("ClothingStoreScene");

        }
        public void OnClickBuyFail()
        {
            ActivateMenu(BuyFail);
        }

        public void OnClickBuyFailOut()
        {
            DeactivateMenu(BuyFail);
        }
        private void ActivateMenu(GameObject menu)
        {
            menu.SetActive(true);
        }

        private void DeactivateMenu(GameObject menu)
        {
            menu.SetActive(false);
        }
    }
}
