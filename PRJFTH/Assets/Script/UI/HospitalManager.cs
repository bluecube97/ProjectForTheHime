using UnityEngine;
using UnityEngine.SceneManagement;

public class HospitalManager : MonoBehaviour
{
    public void OnClickReturn()
    {
        SceneManager.LoadScene("OutingScene");
    }
    




    public void OnClickCure()
   {
        ActivateCureMenu(); // Cure 메뉴를 엶
    }

     public void OnClickCureYes()
    {
        Debug.Log("치료되었습니다");  
        DeactivateCureMenu();
    }

     public void OnClickCureNo()
    {
    DeactivateCureMenu(); //cure 메뉴를 끔
    }


    public GameObject CureMenuBackGround; // 설정 패널 오브젝트

    private void StartCure()
    {
        // 게임 시작 시 설정 패널을 비활성화
        CureMenuBackGround.SetActive(false);
    }

    private void ActivateCureMenu()    //작동시 활성화
    {
        CureMenuBackGround.SetActive(true);
    }

    private void DeactivateCureMenu()   //작동시 비활성화
    {
        CureMenuBackGround.SetActive(false);
    }





      public void OpenBuy()
   {
        ActivateBuyMenu(); // 구매 메뉴를 엶
    }

    public void CloseBuy()
   {
        DeactivateBuyMenu   (); // 구매 메뉴를 엶
    }

    public GameObject HospitalBuyBackGround; // 설정 패널 오브젝트

    private void StartBuy()
    {
        // 게임 시작 시 설정 패널을 비활성화
        HospitalBuyBackGround.SetActive(false);
    }

    private void ActivateBuyMenu()    //작동시 활성화
    {
        HospitalBuyBackGround.SetActive(true);
    }

    private void DeactivateBuyMenu()   //작동시 비활성화
    {
        HospitalBuyBackGround.SetActive(false);
    }




      public void OpenSell()
    {
        ActivateSellMenu(); // 판매 메뉴를 엶
    }

    public void CloseSell()
    {
        DeactivateSellMenu(); // 판매 메뉴를 엶
    }

    public GameObject HospitalSellBackGround; // 설정 패널 오브젝트

    private void Start()
    {
        // 게임 시작 시 설정 패널을 비활성화
        HospitalSellBackGround.SetActive(false);
    }

    private void ActivateSellMenu()    //작동시 활성화
    {
        HospitalSellBackGround.SetActive(true);
    }

    private void DeactivateSellMenu()   //작동시 비활성화
    {
        HospitalSellBackGround.SetActive(false);
    }
} 