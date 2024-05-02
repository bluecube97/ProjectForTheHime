using Script.UI.Outing;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



    public class SmithyManager : MonoBehaviour
{
    public GameObject weaponListPrefab;
    public GameObject weaponList; 
    public Transform weaponListLayout;

    private PointerEventData eventData;

    private SmithyDao smithyDao;
    private SmithyController SmithyController;

    string weaponName = "";
    int weaponPrice = 0;
    private void Start()
    { smithyDao = FindObjectOfType<SmithyDao>();
       // smithyDao = GetComponent<SmithyDao>(); // RestaurantDao 컴포넌트를 가져와서 초기화합니다.
        SmithyController = FindObjectOfType<SmithyController>(); // RestaurantManager를 찾아서 초기화합니다.

        var WeaponList = smithyDao.GetWeaponList();

        int i = 0;

        foreach (var dic in WeaponList)
        {
            i++;
            string i_ = i.ToString();
            GameObject weaponListInstance = Instantiate(weaponListPrefab, weaponListLayout);
            weaponListInstance.name = "weaponlist" + i_;
            Text textComponent = weaponListInstance.GetComponentInChildren<Text>();

            if (textComponent != null)
            {
                textComponent.text = dic["WEAPONNM"] + "\r\n" +
                               " " + dic["WEAPONPRICE"];

            }
        }
        weaponList.SetActive(false);
    }

    public void GetclickWeaponList()
    {
        GameObject clickList = EventSystem.current.currentSelectedGameObject;
        var WeaponList = smithyDao.GetWeaponList();

        string objectName = clickList.name;

        string indexString = objectName.Replace("weaponlist", "");

        int index = int.Parse(indexString);

        Dictionary<string, object> weaponInfo = WeaponList[index - 1];
        weaponName = (string)weaponInfo["WEAPONNM"];
        weaponPrice = (int)weaponInfo["WEAPONPRICE"];
       
        
        Debug.Log("이름 " + weaponName);
        Debug.Log("가격 " + weaponPrice);

    }
        public void ProcessPayment()
    {
        int userCash = smithyDao.GetUserInfoFromDB();
        int NowCash = userCash - weaponPrice;
        Debug.Log("DB 유저 현금 " +userCash);
        Debug.Log("계산 후 금액 " + NowCash);
        if (NowCash >= 0)
        {
            smithyDao.UpdateUserCash(NowCash);
            SmithyController.OnClickBuyComple();
        }
        else
        {
            Debug.Log("Not enough cash!");
            SmithyController.OnClickBuyFail();

        }
    }
}
