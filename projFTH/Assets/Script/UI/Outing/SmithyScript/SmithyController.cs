using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI.Outing.SmithyScript
{
    public class SmithyController : MonoBehaviour
    {
        // Singleton 인스턴스 변수
        private static SmithyController instance;

        // UI 오브젝트 변수
        public GameObject SmeltMenu; // 제작 목록
        public GameObject SellMenu; // 판매 목록
        public GameObject BuyMenu; // 구매 목록
        public GameObject BuyChoiceUI; // 구매 여부 선택
        public GameObject SellChoiceUI; // 판매 여부 선택
        public GameObject SmithyChoiceUI; // 재련 여부 선택
        public GameObject BuyComple; // 구매 성공 시
        public GameObject BuyFail; // 구매 실패 시
        public GameObject SellComplete; // 판매 성공 시
        public GameObject SellFail; // 판매 실패 시
        public GameObject SmithyComplete; // 재련 성공 시
        public GameObject SmithyFail; // 재련 실패 시

        private void Awake()
        {
            // Singleton 인스턴스 초기화
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        // Singleton 인스턴스 접근자
        public static SmithyController Instance => instance;

        // 씬을 OutingScene으로 변경
        public void OnClickReturn() => SceneManager.LoadScene("OutingScene");

        // 메뉴 활성화/비활성화 토글
        public void ToggleMenu(GameObject menu, bool isActive)
        {
            menu.SetActive(isActive);
        }

        // 제작 메뉴 토글
        public void OnClickSmelting() => ToggleMenu(SmeltMenu, true);
        public void OnClickSmeltOuting() => ToggleMenu(SmeltMenu, false);

        // 판매 메뉴 토글
        public void OnClickSelling() => ToggleMenu(SellMenu, true);
        public void OnClickSellOuting() => ToggleMenu(SellMenu, false);

        // 구매 메뉴 토글
        public void OnClickBuying() => ToggleMenu(BuyMenu, true);
        public void OnClickBuyOuting() => ToggleMenu(BuyMenu, false);

        // 구매 여부 UI 토글
        public void OnClickBuyChoiceUI() => ToggleMenu(BuyChoiceUI, true);
        public void OnClickBuyChoiceUIOut() => ToggleMenu(BuyChoiceUI, false);
        
        public void OnClicSellChoiceUI() => ToggleMenu(SellChoiceUI, true);
        public void OnClickSellChoiceUIOut() => ToggleMenu(SellChoiceUI, false);
        
        public void OnClickSmithyChoiceUI() => ToggleMenu(SmithyChoiceUI, true);
        public void OnClickSmithyChoiceUIOut() => ToggleMenu(SmithyChoiceUI, false);

        // 구매 성공 UI 토글
        public void OnClickBuyComple() => ToggleMenu(BuyComple, true);
        public void OnClickBuyCompleOut() => ToggleMenu(BuyComple, false);

        // 구매 실패 UI 토글
        public void OnClickBuyFail() => ToggleMenu(BuyFail, true);
        public void OnClickBuyFailOut() => ToggleMenu(BuyFail, false);

        // 판매 성공 UI 토글
        public void OnClickSellComplete() => ToggleMenu(SellComplete, true);
        public void OnClickSellCompleteOut() => ToggleMenu(SellComplete, false);

        // 판매 실패 UI 토글
        public void OnClickSellFail() => ToggleMenu(SellFail, true);
        public void OnClickSellFailOut() => ToggleMenu(SellFail, false);

        // 재련 성공 UI 토글
        public void OnClickSmithyComplete() => ToggleMenu(SmithyComplete, true);
        public void OnClickSmithyCompleteOut() => ToggleMenu(SmithyComplete, false);

        // 재련 실패 UI 토글
        public void OnClickSmithyFail() => ToggleMenu(SmithyFail, true);
        public void OnClickSmithyFailOut() => ToggleMenu(SmithyFail, false);
    }
}
