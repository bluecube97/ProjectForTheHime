using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class initUserManager : MonoBehaviour
{
    public InputField inputUserNameField;
    public Dropdown inputUserSexDropDown;


    // EnterUser 버튼 눌리면
    public void OnClickEnterUserBtn()
    {
        var userName = inputUserNameField.text;
        var userSex = inputUserSexDropDown.options[inputUserSexDropDown.value].text;

            // 씬 불러오기
            SceneManager.LoadScene("MainLevel_S");
        
    }
}