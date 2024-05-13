using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI.Outing.RestaurantScript
{
        public class RestaurantUIController : MonoBehaviour

    {
        private static RestaurantUIController instance;
        public GameObject EatMenu;
        public GameObject SellMenu;
        public GameObject BuyMenu;
        public GameObject ChiceUi;
        public GameObject BuyComple;
        public GameObject BuyFail;



        private void Awake()
        {
            // 인스턴스가 없을 경우 현재 GameObject에 RestaurantManager를 추가합니다.
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

        public void OnClickEating()
        {
            ActivateMenu(EatMenu);
        }

        public void OnClickEatOuting()
        {
            DeactivateMenu(EatMenu);
        }

        public void OnClickSelling()
        {
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
        public void OnClickBuyComple()
        {
            ActivateMenu(BuyComple);
        }

        public void OnClickBuyCompleOut()
        {
            DeactivateMenu(BuyComple);
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
