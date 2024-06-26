namespace Script.UI.Outing.ClothingStore
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ClothingUIManager : MonoBehaviour
    {
        private static ClothingUIManager instance; // 메뉴의 인스턴스

        public GameObject MakeClothingMenu; // 옷 패널 오브젝트
        public GameObject SellMenu; // 판매 패널 오브젝트
        public GameObject BuyMenu; // 구매 패널 오브젝트
        public GameObject ChoiceUi; //제작여부 확인 오브젝트
        public GameObject BuyChoiceUi; //구매여부 확인 오브젝트
        public GameObject SellChoiceUi; //구매여부 확인 오브젝트
        public GameObject BuyComplete; //구매 성공 시 오브젝트
        public GameObject BuyFail; //구매 실패 시 오브젝트
        public GameObject MakeComplete; //제작 성공 시 오브젝트
        public GameObject MakeFail; //제작 실패 시 오브젝트
        public GameObject SellComplete; //판매 성공 시 오브젝트
        public GameObject SellFail; //판매 실패 시 오브젝트
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

        public void ToggleMenu(GameObject menu, bool isActive)
        {
            menu.SetActive(isActive);
        }

        public void OnClickMakeClothinging() => ToggleMenu(MakeClothingMenu, true);
        public void OnClickMakeClothingOuting() => ToggleMenu(MakeClothingMenu, false);

        public void OnClickSelling() => ToggleMenu(SellMenu, true);
        public void OnClickSellOuting() => ToggleMenu(SellMenu, false);

        public void OnClickBuying() => ToggleMenu(BuyMenu, true);
        public void OnClickBuyOuting() => ToggleMenu(BuyMenu, false);

        public void OnClickChoiceUi() => ToggleMenu(ChoiceUi, true);
        public void OnClickChoiceUiOut() => ToggleMenu(ChoiceUi, false);

        public void OnClickBuyChoiceUi() => ToggleMenu(BuyChoiceUi, true);
        public void OnClickBuyChoiceUiOut() => ToggleMenu(BuyChoiceUi, false);

        public void OnClickSellChoiceUi() => ToggleMenu(SellChoiceUi, true);
        public void OnClickSellChoiceUiOut() => ToggleMenu(SellChoiceUi, false);
        
        public void OnClickBuyComplete() => ToggleMenu(BuyComplete, true);
        public void OnClickBuyCompleteOut() => ToggleMenu(BuyComplete, false);

        public void OnClickBuyFail() => ToggleMenu(BuyFail, true);
        public void OnClickBuyFailOut() => ToggleMenu(BuyFail, false);

        public void OnClickMakeComplete() => ToggleMenu(MakeComplete, true);
        public void OnClickMakeCompleteOut() => ToggleMenu(MakeComplete, false);

        public void OnClickMakeFail() => ToggleMenu(MakeFail, true);
        public void OnClickMakeFailOut() => ToggleMenu(MakeFail, false);
        
        public void OnClickSellComplete() => ToggleMenu(SellComplete, true);
        public void OnClickSellCompleteOut() => ToggleMenu(SellComplete, false);

        public void OnClickSellFail() => ToggleMenu(SellFail, true);
        public void OnClickSellFailOut() => ToggleMenu(SellFail, false);
    }
}
