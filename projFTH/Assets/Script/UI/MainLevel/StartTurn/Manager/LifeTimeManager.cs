using System;
using System.Collections.Generic;
using Script.UI.MainLevel.StartTurn.Dao;
using Script.UI.MainLevel.StartTurn.VO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI.MainLevel.StartTurn.Manager
{
    public class LifeTimeManager : MonoBehaviour
    {
        public GameObject todoListPrefab; // TODOList 이미지 프리팹 참조
        public GameObject todoList; // TODOList 이미지 참조
        public Transform todoListLayout; // TODOList들이 들어갈 레이아웃 참조
        public GameObject todoListInstance; // TODOList의 인스턴스
        private bool _isSelectable = true;
        private string _isSelectDate = "Day1"; // 선택된 날짜

        private GameObject _myGameObject;
        private StartTurnDao _std;
        private List<Dictionary<string, object>> _todoList = new(); // TODO리스트를 담는 딕셔너리 리스트
        private List<Dictionary<string, object>> _ = new();

        public void Awake()
        {
            _myGameObject = new GameObject();
            _std = _myGameObject.AddComponent<StartTurnDao>();
        }

        public void Start()
        {
            var noList = _std.GetTodoNo(2024, 4);
            _todoList = _std.GetTodoList(noList);
            var index = 1;

            foreach (var dic in _todoList)
            {
                // 버튼 프리팹 인스턴스화
                todoListInstance = Instantiate(todoListPrefab, todoListLayout);
                // 이미지 오브젝트에 딕셔너리 값 설정
                var textComponent = todoListInstance.GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    var todoName = dic["TODONAME"].ToString();
                    var reward = Convert.ToInt32(dic["REWARD"]);
                    var loseReward = Convert.ToInt32(dic["LOSEREWARD"]);
                    var statRewardI = Convert.ToInt32(dic["STATREWARD"]);
                    var todoNo = Convert.ToInt32(dic["TODONO"]);
                    // 버튼이 어떤 날짜에 걸쳐 있는지 저장

                    var statReward = "";
                    todoListInstance.name = "TodoBtn" + todoNo;
                    var todoNameComponent = todoListInstance.GetComponent<StartTurnVo>();
                    todoNameComponent.todoName = todoName;
                    todoNameComponent.index = index;

                    // statReward의 마지막 숫자가 0이면 힘, 1이면 마력
                    if (statRewardI % 2 == 0)
                        statReward = "힘 " + statRewardI / 10;
                    else if (statRewardI % 2 == 1)
                        statReward = "마력 " + statRewardI / 10;

                    textComponent.text = todoName +
                                         "\n보상: " + reward +
                                         "\n소모 재화: " + loseReward +
                                         "\n얻는 스탯: " + statReward;
                }

                index++;
            }

            todoList.SetActive(false);
            GameObject.Find(_isSelectDate).GetComponent<Outline>().enabled = true;
        }

        public void OnClickDateBtn(GameObject button)
        {
            _isSelectable = true;
            GameObject.Find(_isSelectDate).GetComponent<Outline>().enabled = false;
            _isSelectDate = button.name;
            GameObject.Find(_isSelectDate).GetComponent<Outline>().enabled = true;
        }

        public void OnClickTodoBtn(GameObject button)
        {
            if (!_isSelectable) return;
            var dateBtn = GameObject.Find(_isSelectDate);
            var textComponent = dateBtn.GetComponentInChildren<Text>();

            var todoNameComponent = button.GetComponent<StartTurnVo>();
            textComponent.text = todoNameComponent.todoName;
            var color = FindColor(todoNameComponent.index);
            ChangeImageColor(_isSelectDate, color);

            // 다음 날짜로 넘어가기
            if (int.Parse(_isSelectDate[3..]) < 20)
            {
                GameObject.Find(_isSelectDate).GetComponent<Outline>().enabled = false;
                _isSelectDate = _isSelectDate[..3] + (int.Parse(_isSelectDate[3..]) + 1);
                GameObject.Find(_isSelectDate).GetComponent<Outline>().enabled = true;
            }
            else
            {
                GameObject.Find(_isSelectDate).GetComponent<Outline>().enabled = false;
                _isSelectable = false;
            }
        }


        private static Color FindColor(int index)
        {
            int r, g, b;
            Color color;
            switch (index)
            {
                case 1: // 블드리레
                    r = 192;
                    g = 0;
                    b = 0;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
                case 2: // 라임
                    r = 172;
                    g = 185;
                    b = 0;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
                case 3: // 그레이체리
                    r = 162;
                    g = 129;
                    b = 139;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
                case 4: // 딸기우유
                    r = 255;
                    g = 129;
                    b = 211;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
                case 5: // 엔틱화이트
                    r = 223;
                    g = 214;
                    b = 210;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
                case 6: // 암갈색
                    r = 193;
                    g = 159;
                    b = 138;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
                case 7: // 실버블론드
                    r = 255;
                    g = 243;
                    b = 212;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
                case 8: // 당근
                    r = 235;
                    g = 115;
                    b = 28;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
                case 9: // 바나나
                    r = 255;
                    g = 224;
                    b = 98;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
                case 10: // 초코
                    r = 99;
                    g = 49;
                    b = 34;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
                default: // 메론
                    r = 152;
                    g = 226;
                    b = 148;
                    color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
                    break;
            }

            return color;
        }

        private static void ChangeImageColor(string objectName, Color color)
        {
            var obj = GameObject.Find(objectName);
            var image = obj.GetComponent<Image>();
            image.color = color;
        }

        public void OnClickComplete()
        {

            Debug.Log("Complete");
        }

        public void OnClickDelete()
        {
            if (!_isSelectable) return;
            ChangeImageColor(_isSelectDate, Color.white);
            var dateBtn = GameObject.Find(_isSelectDate);
            var textComponent = dateBtn.GetComponentInChildren<Text>();
            textComponent.text = "";
        }

        public void OnClickReturn()
        {
            SceneManager.LoadScene("StartTurnScene");
        }
    }
}