using UnityEngine;
using UnityEngine.SceneManagement;

public class VarietyStoreManager : MonoBehaviour
{
    public void OnClickReturn()
    {
        SceneManager.LoadScene("OutingScene");
    }




 public void OpenBuy()
   {
        ActivateBuyMenu(); // 구매 메뉴를 엶
    }

    public void CloseBuy()
   {
        DeactivateBuyMenu   (); // 구매 메뉴를 엶
    }

    public GameObject VarietyStoreBuyBackGround; // 설정 패널 오브젝트

    private void StartBuy()
    {
        // 게임 시작 시 설정 패널을 비활성화
        VarietyStoreBuyBackGround.SetActive(false);
    }

    private void ActivateBuyMenu()    //작동시 활성화
    {
        VarietyStoreBuyBackGround.SetActive(true);
    }

    private void DeactivateBuyMenu()   //작동시 비활성화
    {
        VarietyStoreBuyBackGround.SetActive(false);
    }




      public void OpenSell()
    {
        ActivateSellMenu(); // 판매 메뉴를 엶
    }

    public void CloseSell()
    {
        DeactivateSellMenu(); // 판매 메뉴를 엶
    }

    public GameObject VarietyStoreSellBackGround; // 설정 패널 오브젝트

    private void Start()
    {
        // 게임 시작 시 설정 패널을 비활성화
        VarietyStoreSellBackGround.SetActive(false);
    }

    private void ActivateSellMenu()    //작동시 활성화
    {
        VarietyStoreSellBackGround.SetActive(true);
    }

    private void DeactivateSellMenu()   //작동시 비활성화
    {
        VarietyStoreSellBackGround.SetActive(false);
    }
} 