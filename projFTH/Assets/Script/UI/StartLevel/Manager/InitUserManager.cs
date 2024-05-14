using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Script.UI.StartLevel.Dao;
using System.Collections.Generic;
using System.IO;
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

            StartLevelDao.SetUserInfo(userName, userSex);
            Dictionary<string, string> userInfo = _sld.GetUserInfo();
            if (userInfo != null)
            {
                string userInfo_json = JsonConvert.SerializeObject(userInfo, Formatting.Indented);
                string scriptPath = Application.dataPath + "/JSON/conversationData/parent_status.json";
                File.WriteAllText(scriptPath, userInfo_json);
                Debug.Log("부모정보 json화 완료");
            }
            else 
            {
                Debug.Log("userInfo in Null" + userInfo);
            }
            // Load scene
            SceneManager.LoadScene("FirstSetDStat");

        }
    }
}