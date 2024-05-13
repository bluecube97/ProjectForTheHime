using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI.Outing.SmithyScript
{
        public class SmithyController : MonoBehaviour

    {
        private static SmithyController instance;
        public GameObject SmeltMenu;
        public GameObject SellMenu;
        public GameObject BuyMenu;
        public GameObject ChiceUi;
        public GameObject BuyComple;
        public GameObject BuyFail;

        private void Awake()
        {
            
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public static SmithyController Instance => instance;

        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }

        public void OnClickSmelting()
        {
            ActivateMenu(SmeltMenu);
        }

        public void OnClickSmeltOuting()
        {
            DeactivateMenu(SmeltMenu);
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
