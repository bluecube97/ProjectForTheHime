using Newtonsoft.Json;
using Script.UI.StartLevel.Dao;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI.StartLevel.Manager
{
    public class InitUserManager : MonoBehaviour
    {
        private GameObject _myGameObject; // StartLevelDao를 담는 빈 오브젝트
        private StartLevelDao _sld; // StartLevelDao를 사용하기 위한 변수

        public InputField inputUserNameField; // 사용자 이름 입력 필드
        public Dropdown inputUserSexDropDown; // 사용자 성별 입력 드롭다운

        private Dictionary<string, object> userinfo = new Dictionary<string, object>();
       

        public void Awake()
        {
            _myGameObject = new GameObject();
            _sld = _myGameObject.AddComponent<StartLevelDao>();
        }

        public void OnClickEnterUserBtn()
        {
            StartCoroutine(OnClickEnterUserBtnCoroutine());
        }
        
        private IEnumerator OnClickEnterUserBtnCoroutine()
        {
            bool userInfoFetched = false;
            StartCoroutine(_sld.GetUserEmail(list =>
            {
                userinfo = list;
                userInfoFetched = true;

            }));
            yield return new WaitUntil(() => userInfoFetched);
            
            // initUserScene에서 userName과 userSex 값을 받아옴
            string userName = inputUserNameField.text;
            string userSex = inputUserSexDropDown.options[inputUserSexDropDown.value].text;

            /*StartLevelDao.SetUserInfo(userName, userSex);
            Dictionary<string, string> userInfo = StartLevelDao.GetUserInfo();*/
            
            string userEmail = userinfo["useremail"].ToString();
            StartCoroutine(_sld.SearchUserInfo(userEmail, count =>
            {
                if (count <= 0 )
                {
                    StartCoroutine(_sld.InsertUserInfo(userEmail, userName, userSex));
                }

                StartCoroutine(_sld.GetUser(userEmail, userInfo =>
                {
                    if (userInfo != null)
                    {
                        string userInfo_json = JsonConvert.SerializeObject(userInfo, Formatting.Indented);
                        string scriptPath = Application.dataPath + "/JSON/conversationData/parent_status.json";
//                         File.WriteAllText(scriptPath, userInfo_json);
                        Debug.Log("부모정보 json화 완료");
                    }
                    else 
                    {
                        Debug.Log("userInfo in Null" + userInfo);
                    }
                    // Load scene
                    SceneManager.LoadScene("FirstSetDStat");
                }));
            }));
          

        }
    }
}