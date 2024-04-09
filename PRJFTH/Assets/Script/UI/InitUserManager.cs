using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitUserManager : MonoBehaviour
{
    public InputField InputUserNameField;
    public Dropdown InputUserSexDropDown;


    // EnterUser 버튼 눌리면
    public void OnClickEnterUserBtn()
    {
        var userName = InputUserNameField.text;
        var userSex = InputUserSexDropDown.options[InputUserSexDropDown.value].text;

            // 씬 불러오기
            SceneManager.LoadScene("MainLevel_S");
        
    }
}