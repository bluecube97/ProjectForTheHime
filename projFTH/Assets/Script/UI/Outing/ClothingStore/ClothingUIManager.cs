namespace Script.UI.Outing
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ClothingUIController : MonoBehaviour
    {
        private static ClothingUIController instance; // �޴��� �ν��Ͻ�
        public GameObject MakeClothingMenu; // �� �г� ������Ʈ
        public GameObject SellMenu; // �Ǹ� �г� ������Ʈ
        public GameObject BuyMenu; // ���� �г� ������Ʈ
        public GameObject ChiceUi;
        public GameObject BuyChiceUi;
        public GameObject BuyComple;
        public GameObject BuyFail;
        private void Awake()
        {
            // �ν��Ͻ��� ���� ��� ���� GameObject�� ClothingUIManager �߰��մϴ�.
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        public static ClothingUIController Instance => instance;

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

            // �ǸŸ޴��� Ȱ��ȭ �Ǿ����� �ʴٸ�

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
