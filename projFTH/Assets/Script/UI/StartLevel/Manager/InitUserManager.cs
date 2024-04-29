using Script.UI.StartLevel.Dao;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI.StartLevel.Manager
{
    public class InitUserManager : MonoBehaviour
    {
        private GameObject _myGameObject;
        private StartLevelDao _sld;

        public InputField inputUserNameField;
        public Dropdown inputUserSexDropDown;

        public void Awake()
        {
            _myGameObject = new GameObject();
            _sld = _myGameObject.AddComponent<StartLevelDao>();
        }

        public void OnClickEnterUserBtn()
        {
            // initUserScene에서 userName과 userSex 값을 받아옴
            var userName = inputUserNameField.text;
            var userSex = inputUserSexDropDown.options[inputUserSexDropDown.value].text;

            _sld.SetUserInfo(userName, userSex);

            // Load scene
            SceneManager.LoadScene("MainLevelScene");
        }
    }
}