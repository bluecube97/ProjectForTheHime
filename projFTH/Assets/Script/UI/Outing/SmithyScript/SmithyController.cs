using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI.Outing.SmithyScript
{
        public class SmithyController : MonoBehaviour

    {
        //UI 인스턴스화ㄴ
        private static SmithyController instance;
        public GameObject SmeltMenu; //제작 목록
        public GameObject SellMenu; //판매목록
        public GameObject BuyMenu; //구매목록
        public GameObject ChoiceUI; //구매 여부 선택
        public GameObject BuyComple; //구매성공 시
        public GameObject BuyFail; // 구매실패 시

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

        public void OnClickChoiceUI()
        {
            ActivateMenu(ChoiceUI);
        }

        public void OnClickChoiceUIOut()
        {
            DeactivateMenu(ChoiceUI);
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
