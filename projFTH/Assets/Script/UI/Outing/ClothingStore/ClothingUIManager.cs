using UnityEngine.Serialization;

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
        public GameObject BuyChiceUi; //구매여부 확인 오브젝트
        public GameObject BuyComple; //구매,제작 성공 시 오브젝트
        public GameObject BuyFail; //구매,제작 실패 시 오브젝트
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
        public void OnClickChoiceUi()
        {
            ActivateMenu(ChoiceUi);
        }
        public void OnClickChoiceUiOut()
        {
            DeactivateMenu(ChoiceUi);
        }
        public void OnClickBuyChoiceUi()
        {
            ActivateMenu(BuyChiceUi);
        }
        public void OnClickBuyChoiceUiOut()
        {
            DeactivateMenu(BuyChiceUi);
        }
        public void OnClickBuyComplete()
        {
            ActivateMenu(BuyComple);
        }

        public void OnClickBuyCompleteOut()
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
