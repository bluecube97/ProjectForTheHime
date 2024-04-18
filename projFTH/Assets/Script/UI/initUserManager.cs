using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class initUserManager : MonoBehaviour
{
    public InputField inputUserNameField;
    public Dropdown inputUserSexDropDown;

     public void OnClickEnterUserBtn()
     {
         // initUserScene에서 userName과 userSex 값을 받아옴
         var userName = inputUserNameField.text;
         var userSex = inputUserSexDropDown.options[inputUserSexDropDown.value].text;

         // DB 연결


         // Load scene
         SceneManager.LoadScene("MainLevel_S");
    }
}