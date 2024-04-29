using System;
using System.Collections.Generic;
using Script.UI.MainLevel.StartTurn.Dao;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI.MainLevel.StartTurn.Manager
{
    public class LifeTimeManager : MonoBehaviour
    {
        public GameObject todoListPrefab; // TODOList 이미지 프리팹 참조
        public GameObject todoList; // TODOList 이미지 참조
        public Transform todoListLayout; // TODOList들이 들어갈 레이아웃 참조
        public GameObject detailPopup; // TODOList의 세부 정보 팝업창
        private GameObject _myGameObject;
        private StartTurnDao _std;
        public List<Dictionary<string, object>> TodoList = new(); // TODO리스트를 담는 딕셔너리 리스트

        public int TodoNo { get; set; } // TODO의 번호

        public void Awake()
        {
            _myGameObject = new GameObject();
            _std = _myGameObject.AddComponent<StartTurnDao>();
        }

        public void Start()
        {
            var noList = _std.GetTodoNo(2024, 4);
            TodoList = _std.GetTodoList(noList);

            foreach (var dic in TodoList)
            {
                // 이미지 프리팹 인스턴스화
                var todoListInstance = Instantiate(todoListPrefab, todoListLayout);

                // 이미지 오브젝트에 딕셔너리 값 설정
                var textComponent = todoListInstance.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    var todoName = dic["TODONAME"].ToString();
                    var reward = Convert.ToInt32(dic["REWARD"]);
                    var loseReward = Convert.ToInt32(dic["LOSEREWARD"]);
                    var statRewardI = Convert.ToInt32(dic["STATREWARD"]);
                    var statReward = "";
                    // TodoNo를 자식 오브젝트에 저장
                    todoListInstance.name = dic["TODONO"].ToString();

                    // statReward의 마지막 숫자가 0이면 힘, 1이면 마력
                    if (statRewardI % 2 == 0)
                        statReward = "힘 " + statRewardI / 10;
                    else if (statRewardI % 2 == 1) statReward = "마력 " + statRewardI / 10;

                    textComponent.text = todoName +
                                         "\n보상: " + reward +
                                         "\n소모 재화: " + loseReward +
                                         "\n얻는 스탯: " + statReward;
                }
            }

            todoList.SetActive(false);
        }

        private void Update()
        {
            var popupPosition = Input.mousePosition;
            popupPosition.x += 100;
            popupPosition.y += 100;
            detailPopup.transform.position = popupPosition;
        }

        public void OnPointerEnter()
        {
            // 현재 마우스가 가리키고 있는 게임 오브젝트 가져옴
            var nowGameObject = EventSystem.current.currentSelectedGameObject;
            // 팝업 활성화
            detailPopup.SetActive(true);
            // 해당 게임 오브젝트의 TodoNo 가져오기
            var todoNo = 1; // = Convert.ToInt32(nowGameObject.name);
            // 팝업에 세부 정보 설정
            var popupText = detailPopup.GetComponentInChildren<Text>();
            // TodoList에서 해당 Todo 항목의 정보를 가져옴
            foreach (var dic in TodoList)
            {
                if (Convert.ToInt32(dic["TODONO"]) == todoNo)
                {
                    var todoName = dic["TODONAME"].ToString();
                    var reward = dic["REWARD"].ToString();
                    var loseReward = dic["LOSEREWARD"].ToString();
                    var statReward = dic["STATREWARD"].ToString();

                    // 팝업에 Todo 항목의 세부 정보를 출력
                    popupText.text = "Todo Name: " + todoName +
                                     "\nReward: " + reward +
                                     "\nLose Reward: " + loseReward +
                                     "\nStat Reward: " + statReward;
                    break;
                }
            }
        }

        public void OnPointerExit()
        {
            // 팝업 비활성화
            detailPopup.SetActive(false);
        }

        public void OnClickComplete()
        {
            Debug.Log("Complete");
        }

        public void OnClickReturn()
        {
            SceneManager.LoadScene("StartTurnScene");
        }
    }
}