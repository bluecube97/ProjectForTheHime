using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestBattle : MonoBehaviour
{

    public GameObject combatSelectionBar;
    public GameObject AttackVal;
    public TextMeshPro[] attackOption;

    // Start is called before the first frame update
    void Start()
    {
        combatSelectionBar.SetActive(false);
    }
    
     public void attackBtn()
    {
        Debug.Log("공격!");
        combatSelectionBar.SetActive(true);

    }
    public void skillkBtn()
    {
        Debug.Log("스킬!");
    }
    public void iteamListBtn()
    {
        Debug.Log("아이템!");
    }
    public void runBtn()
    {
        Debug.Log("둠황챠!");
    }
    public void SelectAttackOption(string option)
    {
        Debug.Log(option + " 선택됨");
    }
}
