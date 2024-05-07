namespace Script.UI.Outing
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ClothingStoreManager : MonoBehaviour
    {
        private static ClothingStoreManager instance; // ESC�޴��� �ν��Ͻ�
        public GameObject MakeClothingMenu; // �� �г� ������Ʈ
        public GameObject SellMenu; // �Ǹ� �г� ������Ʈ
        public GameObject BuyMenu; // ���� �г� ������Ʈ

        private bool MakeClothingMenuActive; // �� ȭ�� Ȱ��ȭ ����
        private bool SellMenuActive; // �Ǹ� ȭ�� Ȱ��ȭ ����
        private bool BuyMenuActive; // ���� ȭ�� Ȱ��ȭ ����

        public static ClothingStoreManager Instance
        {
            get
            {
                // �ν��Ͻ��� ���ٸ� ���� ����
                if (instance == null)
                {
                    instance = FindObjectOfType<ClothingStoreManager>();

                    // ���� �޴��� ���ٸ� ���� ����
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

            // �ǸŸ޴��� Ȱ��ȭ �Ǿ����� �ʴٸ�

            ActivateSellMenu();
        }
        public void OnClickSellOuting()

        {
            DeactivateSellMenu();
        }


        public void OnClickBuying()
        {

            // ���Ÿ޴��� Ȱ��ȭ �Ǿ����� �ʴٸ�


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
