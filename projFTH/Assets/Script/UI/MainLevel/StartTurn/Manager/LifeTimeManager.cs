using Script.UI.MainLevel.StartTurn.Dao;
using Script.UI.MainLevel.StartTurn.VO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI.MainLevel.StartTurn.Manager
{
    public class LifeTimeManager : MonoBehaviour
    {
        private static int _nowYear = 2024;
        private static int _nowMonth = 4;
        private static int _nowDate = 1;
        private static int _nowTime; // 0: 아침, 1: 점심, 2: 저녁

        public GameObject yearTxt; // 년도 텍스트 참조
        public GameObject monthTxt; // 월 텍스트 참조
        public GameObject dateTxt; // 일 텍스트 참조
        public GameObject timeTxt; // 시간 텍스트 참조
        public GameObject todoNameTxt; // TODO 이름 텍스트 참조

        public GameObject nowDate; // 현재 날짜 참조
        public GameObject todoListPrefab; // TODOList 이미지 프리팹 참조
        public GameObject todoList; // TODOList 이미지 참조
        public Transform todoListLayout; // TODOList들이 들어갈 레이아웃 참조

        public GameObject startTurn; // 시작 턴 이미지 참조
        public GameObject lifeTimeMain; // 라이프 타임 이미지 참조
        public GameObject convBtn; // 대화 버튼 참조
        private readonly List<Dictionary<string, object>> _planList = new();

        private bool _isSelectable = true;
        private string _isSelectDate = "Day1"; // 선택된 날짜

        private GameObject _myGameObject;
        private StartTurnDao _std;
        private List<Dictionary<string, object>> _todoList = new(); // TODO리스트를 담는 딕셔너리 리스트
        private GameObject _todoListInstance; // TODOList의 인스턴스

        public void Awake()
        {
            _myGameObject = new GameObject();
            _std = _myGameObject.AddComponent<StartTurnDao>();
        }

        public void Start()
        {
            List<int> noList = _std.GetTodoNo(_nowYear, _nowMonth);
            _todoList = _std.GetTodoList(noList);
            int index = 1;
            Text nowDateComponent = nowDate.GetComponentInChildren<Text>();
            nowDateComponent.text = _nowYear + "년 " + _nowMonth + "월";

            foreach (Dictionary<string, object> dic in _todoList)
            {
                // 버튼 프리팹 인스턴스화
                _todoListInstance = Instantiate(todoListPrefab, todoListLayout);
                // 이미지 오브젝트에 딕셔너리 값 설정
                Text todoNameTxtComponent = _todoListInstance.GetComponentInChildren<Text>();
                if (todoNameTxtComponent != null)
                {
                    string todoName = dic["TODONAME"].ToString();
                    int reward = Convert.ToInt32(dic["REWARD"]);
                    int loseReward = Convert.ToInt32(dic["LOSEREWARD"]);
                    int statRewardI = Convert.ToInt32(dic["STATREWARD"]);
                    int todoNo = Convert.ToInt32(dic["TODONO"]);
                    // 버튼이 어떤 날짜에 걸쳐 있는지 저장

                    string statReward = "";
                    _todoListInstance.name = "TodoBtn" + todoNo;
                    StartTurnVo todoNameComponent = _todoListInstance.GetComponent<StartTurnVo>();

                    statReward = (statRewardI % 2) switch
                    {
                        // statReward의 마지막 숫자가 0이면 힘, 1이면 마력
                        0 => "힘 " + (statRewardI / 10),
                        1 => "마력 " + (statRewardI / 10),
                        _ => statReward
                    };

                    todoNameComponent.todoName = todoName;
                    todoNameComponent.reward = reward;
                    todoNameComponent.loseReward = loseReward;
                    todoNameComponent.statReward = statReward;
                    todoNameComponent.index = index;

                    todoNameTxtComponent.text = todoName +
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
            if (!_isSelectable)
            {
                return;
            }

            GameObject dateBtn = GameObject.Find(_isSelectDate);
            StartTurnVo dateComponent = dateBtn.GetComponent<StartTurnVo>();
            Text textComponent = dateBtn.GetComponentInChildren<Text>();
            StartTurnVo todoNameComponent = button.GetComponent<StartTurnVo>();
            textComponent.text = todoNameComponent.todoName;

            dateComponent.todoName = todoNameComponent.todoName;
            dateComponent.reward = todoNameComponent.reward;
            dateComponent.loseReward = todoNameComponent.loseReward;
            dateComponent.statReward = todoNameComponent.statReward;

            Color color = FindColor(todoNameComponent.index);
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
            GameObject obj = GameObject.Find(objectName);
            Image image = obj.GetComponent<Image>();
            image.color = color;
        }

        public void OnClickComplete()
        {
            for (int i = 1; i <= 20; i++)
            {
                string findDate = "Day" + i;
                GameObject findDateBtn = GameObject.Find(findDate);
                StartTurnVo dateComponent = findDateBtn.GetComponent<StartTurnVo>();

                Dictionary<string, object> dic = new()
                {
                    { "DAY", i },
                    { "TODONAME", dateComponent.todoName },
                    { "REWARD", dateComponent.reward },
                    { "LOSEREWARD", dateComponent.loseReward },
                    { "STATREWARD", dateComponent.statReward }
                };

                _planList.Add(dic);
            }

            startTurn.SetActive(true);
            StartTurn();
        }

        private void StartTurn()
        {
            Text yearTxtComponent = yearTxt.GetComponentInChildren<Text>();
            Text monthTxtComponent = monthTxt.GetComponentInChildren<Text>();
            Text dateTxtComponent = dateTxt.GetComponentInChildren<Text>();
            Text timeTxtComponent = timeTxt.GetComponentInChildren<Text>();
            Text todoNameTxtComponent = todoNameTxt.GetComponentInChildren<Text>();
            Text lifeTimeMainComponent = lifeTimeMain.GetComponentInChildren<Text>();

            yearTxtComponent.text = _nowYear + "년";
            monthTxtComponent.text = _nowMonth + "월";
            dateTxtComponent.text = _nowDate + "일";
            timeTxtComponent.text = _nowTime switch
            {
                0 => "아침",
                1 => "점심",
                2 => "저녁",
                _ => timeTxtComponent.text
            };

            switch (_nowTime)
            {
                case 0:
                    convBtn.SetActive(true);
                    break;
                case 1:
                    convBtn.SetActive(false);
                    break;
                case 2:
                    convBtn.SetActive(true);
                    break;
            }

            if (_planList[_nowDate - 1]["TODONAME"].Equals(""))
            {
                todoNameTxtComponent.text = "쉬는날";
                lifeTimeMainComponent.text = "";
            }
            else
            {
                todoNameTxtComponent.text = _planList[_nowDate - 1]["TODONAME"].ToString();
                lifeTimeMainComponent.text = "보상: " + _planList[_nowDate - 1]["REWARD"] +
                                             "\n소모 재화: " + _planList[_nowDate - 1]["LOSEREWARD"] +
                                             "\n얻는 스탯: " + _planList[_nowDate - 1]["STATREWARD"];
            }

            if (_nowTime is 0 or 2)
            {
                // 대화
            }
            else
            {
                // 일과
            }
        }

        private void EndTurn()
        {
            startTurn.SetActive(false);
            if (_nowMonth < 12)
            {
                _nowMonth++;
                _nowDate = 1;
            }
            else
            {
                _nowMonth = 1;
                _nowYear++;
                _nowDate = 1;
            }

            Text nowDateComponent = nowDate.GetComponentInChildren<Text>();
            nowDateComponent.text = _nowYear + "년 " + _nowMonth + "월";
        }

        public void OnClickNextPhase()
        {
            _nowTime++;
            if (_nowTime > 2)
            {
                _nowTime = 0;
                _nowDate++;
            }

            if (_nowDate <= 20)
            {
                StartTurn();
            }
            else
            {
                EndTurn();
            }
        }

        public void OnClickDelete()
        {
            if (!_isSelectable)
            {
                return;
            }

            ChangeImageColor(_isSelectDate, Color.white);
            GameObject dateBtn = GameObject.Find(_isSelectDate);
            Text textComponent = dateBtn.GetComponentInChildren<Text>();

            textComponent.text = "";
        }

        public void OnClickReturn()
        {
            SceneManager.LoadScene("StartTurnScene");
        }
    }
}