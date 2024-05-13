
using Newtonsoft.Json.Linq;
using Script.UI.Outing;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DStateManager : MonoBehaviour
{
   
    public Slider EandI;
    public Slider SandN;
    public Slider TandF;
    public Slider JandP;
    public GameObject sysoutMBTI;
    public InputField inputDaughterNameField;

    private static DStateManager instance;
    public GameObject EandI_Desc;
    public GameObject SandN_Desc;
    public GameObject TandF_Desc;
    public GameObject JandP_Desc;

    //MBTI 관련 값 선언
    private string m = "E";
    private string b = "S";
    private string t = "T";
    private string i = "J";
    private int E = 100;
    private int I = 0;
    private int S = 100;
    private int N = 0;
    private int T = 100;
    private int F = 0;
    private int J = 100;
    private int P = 0;
    private string mbti = "";

    public static DStateManager Instance => instance;

    private void Awake()
    {
        // 인스턴스가 없을 경우 현재 GameObject에 DStateManager 추가합니다.
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    //MBTI의 값 설정
    public void GetEandIvalue()
    {
        int iande = 0;
        iande = (int)EandI.value;
        int value = iande * 20;
        E = 0;
        E  = 100-value;
        I = value;
        if (value < 50)
        {
            m = "E";
        }
        else if(value > 50)
        {
            m = "I";
        }
        Text textComponent = EandI.GetComponentInChildren<Text>();

        if (textComponent != null)
        {
            textComponent.text = "E : " +E +"    "+m+"    "+ "I : " + I;
        }
    }

    public void GetSandNvalue()
    {
        int sandn = 0;
        sandn = (int)SandN.value;
        int value = sandn * 20;
        S = 0;
        S = 100-value;
        N = value;
        if (value < 50)
        {
            b = "S";
        }
        else if (value > 50)
        {
            b = "N";
        }
        Text textComponent = SandN.GetComponentInChildren<Text>();

        if (textComponent != null)
        {
            textComponent.text = "S : " + S + "    " + b + "    " + "N : " + N;
        }
    }
    public void GetTandFvalue()
    {
        int tandf = 0;
        tandf = (int)TandF.value;
        int value = tandf * 20;
        T= 0;
        T = 100-value;
        F = value;
        if (value < 50)
        {
            t = "T";
        }
        else if (value > 50)
        {
            t = "F";
        }
        Text textComponent = TandF.GetComponentInChildren<Text>();

        if (textComponent != null)
        {
            textComponent.text = "T : " + T + "    " + t + "    " + "F : " + F;
        }
    }
    public void GetJandPvalue()
    {
        int jandp = 0;
        jandp = (int)JandP.value;
        int value = jandp * 20;
        J = 0;
        J = 100-value;
        P = value;
        if (value < 50)
        {
            i = "J";
        }
        else if (value > 50)
        {
            i = "P";
        }
        Text textComponent = JandP.GetComponentInChildren<Text>();

        if (textComponent != null)
        {
            textComponent.text = "J : " + J + "    " + i + "    " + "P : " + P;
        }

    }
    // 버튼 클릭 시 딸 초기 스탯(이름,MBTI) 로그  확인 
    public void LogMBTI()
    {
        var DaughterName = inputDaughterNameField.text;
        mbti = m + b + t + i;

        var json = new JObject(); 
        json.Add("name" , DaughterName);
        json.Add("age", 10);
        json.Add("sex", "female");
        json.Add("mbti", mbti);
        json.Add("hp", 100);
        json.Add("mp", 100);
        json.Add("mood", "happiness");
        json.Add("stress", "high");
        json.Add("fatigue", "tired");
        json.Add("E" , E);
        json.Add("I" , I);
        json.Add("S" , S);
        json.Add("N" , N);
        json.Add("T" , T);
        json.Add("F" , F);
        json.Add("J" , J);
        json.Add("P" , P);

        var finaljson = new JObject();
        finaljson.Add("daughter", json);
        var filePath = Application.dataPath + "/JSON/conversationData/daughter_status.json";

        // daughter_status.json 파일이 존재하는지 확인하고 삭제합니다.
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // daughter_status.json 파일을 생성하고 JSON 데이터를 쓰기 위해 FileStream을 사용합니다.
        using (var fileStream = File.Create(filePath))
        {
            // JSON 데이터를 문자열로 변환하여 파일에 씁니다.
            var jsonText = finaljson.ToString();
            var bytes = Encoding.UTF8.GetBytes(jsonText);
            fileStream.Write(bytes, 0, bytes.Length);
        }

        // MainLevelScene을 로드합니다.
        SceneManager.LoadScene("MainLevelScene");

    }

    //조건에 따른 UI 관리를 위한 메서드 선언 
    public void OnClickEandI_Desc()
    {
        ActivateMenu(EandI_Desc);
    } 
    public void OnClicEandI_DescOut()
    {
        DeactivateMenu(EandI_Desc);
    }
    public void OnClickSandN_Desc()
    {
        ActivateMenu(SandN_Desc);
    }
    public void OnClickSandN_DescOut()
    {
        DeactivateMenu(SandN_Desc);
    }
    public void OnClickTandF_Desc()
    {
        ActivateMenu(TandF_Desc);
    }
    public void OnClickTandF_DescOut()
    {
        DeactivateMenu(TandF_Desc);
    }
    public void OnClickJandP_Desc()
    {
        ActivateMenu(JandP_Desc);
    }
    public void OnClickJandP_DescOut()
    {
        DeactivateMenu(JandP_Desc);
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
