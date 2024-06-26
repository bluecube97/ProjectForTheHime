using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI.Outing.RestaurantScript
{
        public class RestaurantUIController : MonoBehaviour

    {
        private static RestaurantUIController instance; // 인스턴스

        public GameObject EatMenu; // 식사 목록 UI
        public GameObject SellMenu; // 판매 목록 UI
        public GameObject BuyMenu; // 구매 목록 UI
        public GameObject ChoiceUi; // 구매 여부 선택 UI
        public GameObject SellChoiceUi; // 판매 여부 선택 UI
        public GameObject SellComplete; // 판매 성공 UI
        public GameObject SellFail; // 판매 실패 UI
        public GameObject BuyComplete; // 구매 성공 UI
        public GameObject BuyFail; // 구매 실패 UI

        private void Awake()
        {
            // 인스턴스가 없을 경우 현재 GameObject에 RestaurantUIController를 추가합니다.
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public static RestaurantUIController Instance => instance;

        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }

        public void ToggleMenu(GameObject menu, bool isActive)
        {
            menu.SetActive(isActive);
        }

        public void OnClickEatMenu() => ToggleMenu(EatMenu, true);
        public void OnClickEatMenuClose() => ToggleMenu(EatMenu, false);

        public void OnClickSellMenu() => ToggleMenu(SellMenu, true);
        public void OnClickSellMenuClose() => ToggleMenu(SellMenu, false);

        public void OnClickBuyMenu() => ToggleMenu(BuyMenu, true);
        public void OnClickBuyMenuClose() => ToggleMenu(BuyMenu, false);

        public void OnClickChoiceUi() => ToggleMenu(ChoiceUi, true);
        public void OnClickChoiceUiClose() => ToggleMenu(ChoiceUi, false);

        public void OnClickSellChoiceUi() => ToggleMenu(SellChoiceUi, true);
        public void OnClickSellChoiceUiClose() => ToggleMenu(SellChoiceUi, false);

        public void OnClickSellComplete() => ToggleMenu(SellComplete, true);
        public void OnClickSellCompleteClose() => ToggleMenu(SellComplete, false);

        public void OnClickSellFail() => ToggleMenu(SellFail, true);
        public void OnClickSellFailClose() => ToggleMenu(SellFail, false);

        public void OnClickBuyComplete() => ToggleMenu(BuyComplete, true);
        public void OnClickBuyCompleteClose() => ToggleMenu(BuyComplete, false);

        public void OnClickBuyFail() => ToggleMenu(BuyFail, true);
        public void OnClickBuyFailClose() => ToggleMenu(BuyFail, false);
    }
}