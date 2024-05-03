using System;
using System.Collections.Generic;
using System.Linq;
using Script.UI.MainLevel.StartTurn.Dao;
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
        private readonly Dictionary<string, int> _calendarChk = new(); // 달력에 일정이 있는지의 여부를 담는 딕셔너리 리스트
        private readonly Dictionary<GameObject, bool> _isButtonActive = new(); // 버튼이 활성화되어 있는지 확인하는 딕셔너리
        private readonly Dictionary<GameObject, bool> _isButtonSelect = new(); // 버튼이 선택되어있는지 확인하는 딕셔너리
        private readonly Dictionary<GameObject, List<string>> _buttonDates = new();
        private IEnumerable<IGrouping<object, Dictionary<string, object>>> _groupTodoDay;

        private GameObject _myGameObject;
        private StartTurnDao _std;
        private List<Dictionary<string, object>> _todoDay = new(); // TODO리스트의 날짜를 담는 딕셔너리 리스트
        private List<Dictionary<string, object>> _todoList = new(); // TODO리스트를 담는 딕셔너리 리스트

        public void Awake()
        {
            _myGameObject = new GameObject();
            _std = _myGameObject.AddComponent<StartTurnDao>();
        }

        public void Start()
        {
            var noList = _std.GetTodoNo(2024, 4);
            _todoList = _std.GetTodoList(noList);
            _todoDay = _std.GetTodoDay(noList);
            _groupTodoDay = _todoDay.GroupBy(dic => dic["TODONO"]);

            for (var i = 1; i <= 20; i++) _calendarChk.Add(i.ToString(), 0);

            foreach (var VARIABLE in _todoDay)
            foreach (var var in VARIABLE)
                Debug.Log("todoDay: " + var);

            foreach (var dic in _todoList)
            {
                // 버튼 프리팹 인스턴스화
                todoListInstance = Instantiate(todoListPrefab, todoListLayout);
                // 버튼이 선택되지 않은 상태로 초기화
                _isButtonSelect[todoListInstance] = false;
                // 버튼이 활성화된 살태로 초기화
                _isButtonActive[todoListInstance] = true;
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
                    var dates = _todoDay.Where(d => (int)d["TODONO"] == todoNo).Select(d => d["DATE"].ToString()).ToList();
                    _buttonDates[todoListInstance] = dates;
                    var statReward = "";

                    todoListInstance.name = "TodoBtn" + todoNo;

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
            }

            todoList.SetActive(false);
        }

        public void OnClickTodoBtn(GameObject button)
        {
            // 버튼이 선택되어있지 않은 상태이고, 버튼이 활성화된 상태이면
            if (!_isButtonSelect[button] && _isButtonActive[button])
            {
                _isButtonSelect[button] = true;
                var index = button.transform.GetSiblingIndex();

                foreach (var dic in _todoDay)
                {
                    var todoNo = Convert.ToInt32(dic["TODONO"]);
                    if (todoNo == (int)_todoList[index - 1]["TODONO"])
                    {
                        var date = dic["DATE"].ToString();
                        Debug.Log(date);
                        var objectName = "Day" + date;
                        var color = FindColor(index);
                        ChangeImageColor(objectName, color);
                        _calendarChk[date]++;
                    }
                }

                foreach (var date in _buttonDates[button])
                {
                    _calendarChk[date]++;
                    if (_calendarChk[date] > 1)
                    {
                        ChangeImageColor(button.name, Color.gray);
                        _isButtonActive[button] = false;
                    }
                }

                foreach (var group in _groupTodoDay)
                foreach (var dic in group)
                    if (_calendarChk[dic["DATE"].ToString()] > 0)
                    {
                        var activeBtn = GameObject.Find("TodoBtn" + group.Key);
                        ChangeImageColor(activeBtn.name, Color.gray);
                        _isButtonActive[activeBtn] = false;
                    }
                ChangeImageColor(button.name, Color.green);
            }
            // 버튼이 이미 선택되어 있는 상태이면
            else if (_isButtonSelect[button] && _isButtonActive[button])
            {
                _isButtonSelect[button] = false;
                var index = button.transform.GetSiblingIndex();

                foreach (var dic in _todoDay)
                {
                    var todoNo = Convert.ToInt32(dic["TODONO"]);
                    if (todoNo == (int)_todoList[index - 1]["TODONO"])
                    {
                        var date = dic["DATE"].ToString();
                        Debug.Log(date);
                        var objectName = "Day" + date;
                        var color = Color.white;
                        ChangeImageColor(objectName, color);
                        _calendarChk[date]--;
                    }
                }

                foreach (var date in _buttonDates[button])
                {
                    _calendarChk[date]--;
                    if (_calendarChk[date] == 0)
                    {
                        ChangeImageColor(button.name, Color.white);
                        _isButtonActive[button] = true;
                    }
                }

                foreach (var group in _groupTodoDay)
                foreach (var dic in group)
                    if (_calendarChk[dic["DATE"].ToString()] > 0)
                    {
                        var activeBtn = GameObject.Find("TodoBtn" + group.Key);
                        ChangeImageColor(activeBtn.name, Color.gray);
                        _isButtonActive[activeBtn] = false;
                    }
                    else
                    {
                        var activeBtn = GameObject.Find("TodoBtn" + group.Key);
                        ChangeImageColor(activeBtn.name, Color.white);
                        _isButtonActive[activeBtn] = true;
                    }

                ChangeImageColor(button.name, Color.white);
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

        public void OnClickReturn()
        {
            SceneManager.LoadScene("StartTurnScene");
        }
    }
}