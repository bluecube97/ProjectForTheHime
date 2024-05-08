using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HospitalUIController : MonoBehaviour
{
    //병원 UI 컨트롤러 
    private static HospitalUIController instance;
    public GameObject CureMenu;
    public GameObject BuyMenu;
    public GameObject SellMenu;
  
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
    public static HospitalUIController Instance => instance;

    public void OnClickReturn()
    {
        SceneManager.LoadScene("OutingScene");
    }
    public void OnClickCure()
    {
        ActivateMenu(CureMenu);
    }

    public void OnClickCureOut()
    {
        DeactivateMenu(CureMenu);
    }
    public void OnClickBuying()
    {
        ActivateMenu(BuyMenu);
    }

    public void OnClickBuyOuting()
    {
        DeactivateMenu(BuyMenu);
    }
    public void OnClickSelling()
    {
        ActivateMenu(SellMenu);
    }

    public void OnClickSellOuting()
    {
        DeactivateMenu(SellMenu);
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
