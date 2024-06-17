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
        private LifeTimeGo _ltgo; // LifeTime의 GameObject들을 한번에 관리하기 위한 클래스
        private LifeTimeVo _ltvo; // LifeTime의 변수를 한번에 관리하기 위한 클래스

        private GameObject _myGameObject; // StartTurnDao를 가져오기 위한 GameObject
        private StartTurnDao _std; // StartTurnDao 클래스 참조

        public void Start()
        {
            // LifeTimeVo 생성
            _ltvo = new LifeTimeVo();
            _ltgo = new LifeTimeGo();
            // StartTurnDao를 가져오기 위한 GameObject 생성
            _myGameObject = new GameObject();
            _std = _myGameObject.AddComponent<StartTurnDao>();
            InitTodoList(); // TODOList 세팅
        }

        // 달력의 날짜 버튼 OnClick 이벤트
        public void OnClickDateBtn(GameObject button)
        {
            _ltvo.IsSelectable = true; // TODO 버튼 클릭 가능
            // 원래 선택되어 있던 버튼 테두리 비활성화
            GameObject.Find(_ltvo.IsSelectDate).GetComponent<Outline>().enabled = false;
            // 선택된 버튼 테두리 활성화
            _ltvo.IsSelectDate = button.name;
            GameObject.Find(_ltvo.IsSelectDate).GetComponent<Outline>().enabled = true;
        }

        // TODO 버튼 OnClick 이벤트
        public void OnClickTodoBtn(GameObject button)
        {
            // TODO 버튼 클릭 가능 여부 판단 및 불가능 시 리턴
            if (!_ltvo.IsSelectable) return;

            GameObject dateBtn = GameObject.Find(_ltvo.IsSelectDate); // 선택된 날짜 버튼 참조
            TodoNameComponentVo dateComponent = dateBtn.GetComponent<TodoNameComponentVo>(); // 선택된 날짜 버튼의 컴포넌트 참조
            Text textComponent = dateBtn.GetComponentInChildren<Text>(); // 선택된 날짜 버튼의 텍스트 컴포넌트 참조
            TodoNameComponentVo todoNameComponent = button.GetComponent<TodoNameComponentVo>(); // 선택된 TODO 버튼의 컴포넌트 참조
            textComponent.text = todoNameComponent.todoName; // 선택된 날짜 버튼의 텍스트 변경
            // 선택된 날짜 버튼의 컴포넌트 값 변경
            dateComponent.todoName = todoNameComponent.todoName;
            dateComponent.reward = todoNameComponent.reward;
            dateComponent.loseReward = todoNameComponent.loseReward;
            dateComponent.statReward = todoNameComponent.statReward;
            // 선택된 날짜 버튼의 색상 변경
            Color color = FindColor(todoNameComponent.index);
            ChangeImageColor(_ltvo.IsSelectDate, color);

            // 다음 날짜로 넘어가기
            if (int.Parse(_ltvo.IsSelectDate[3..]) < 20)
            {
                GameObject.Find(_ltvo.IsSelectDate).GetComponent<Outline>().enabled = false;
                _ltvo.IsSelectDate = _ltvo.IsSelectDate[..3] + (int.Parse(_ltvo.IsSelectDate[3..]) + 1);
                GameObject.Find(_ltvo.IsSelectDate).GetComponent<Outline>().enabled = true;
            }
            else
            {
                GameObject.Find(_ltvo.IsSelectDate).GetComponent<Outline>().enabled = false;
                _ltvo.IsSelectable = false;
            }
        }

        // 결정 버튼 OnClick 이벤트
        public void OnClickComplete()
        {
            for (int i = 1; i <= 20; i++) // 20일간 반복
            {
                string findDate = "Day" + i; // "Day1" ~ "Day20"
                GameObject findDateBtn = GameObject.Find(findDate); // "Day1" ~ "Day20" 오브젝트 참조
                TodoNameComponentVo
                    dateComponent = findDateBtn.GetComponent<TodoNameComponentVo>(); // "Day1" ~ "Day20" 오브젝트의 컴포넌트 참조
                // 딕셔너리에 값 저장
                Dictionary<string, object> dic = new()
                {
                    { "DAY", i },
                    { "TODONAME", dateComponent.todoName },
                    { "REWARD", dateComponent.reward },
                    { "LOSEREWARD", dateComponent.loseReward },
                    { "STATREWARD", dateComponent.statReward }
                };
                // 리스트에 딕셔너리 추가
                _ltvo.PlanList.Add(dic);
            }

            OnClickDateBtn(GameObject.Find("Day1")); // 첫 날 선택

            // 시작 턴 이미지 활성화
            _ltgo.StartTurn.SetActive(true);
            StartTurn(); // 턴 시작
        }

        // 다음 날짜로 넘어가기
        public void OnClickNextPhase()
        {
            _ltvo.NowTime++; // 시간 증가
            if (_ltvo.NowTime > 2) // 저녁이면, 아침으로
            {
                _ltvo.NowTime = 0;
                _ltvo.NowDate++;
            }

            if (_ltvo.NowDate <= 20) // 20일까지 반복
            {
                // 턴 시작
                StartTurn();
            }
            else
            {
                // 20일이 끝나면 턴 종료
                EndTurn();
            }
        }

        // 일정 삭제
        public void OnClickDelete()
        {
            if (!_ltvo.IsSelectable)
            {
                return;
            }

            ChangeImageColor(_ltvo.IsSelectDate, Color.white);
            GameObject dateBtn = GameObject.Find(_ltvo.IsSelectDate);
            Text textComponent = dateBtn.GetComponentInChildren<Text>();

            DeleteDateComponent(dateBtn);

            textComponent.text = "";
        }

        // 이전 씬으로 돌아가기
        public void OnClickReturn()
        {
            SceneManager.LoadScene("StartTurnScene");
        }

        // TODOList 세팅
        private void InitTodoList()
        {
            _ltgo.StartTurn.SetActive(false);
            _ltgo.TodoList.SetActive(true);

            // 현재 날짜의 연, 월을 입력받아 해당하는 TodoNO를 반환하여 리스트에 저장
            List<int> noList = new();
            StartCoroutine(_std.GetTodoNo(_ltvo.NowYear, _ltvo.NowMonth, list =>
            {
                noList = list;
                // TodoNO를 이용하여 TodoList를 가져와 리스트에 저장
                StartCoroutine(_std.GetTodoList(noList, list =>
                {
                    _ltvo.TodoList = list;

                    // TODOList에 인덱스 지정 할 변수
                    int index = 1;

                    // 현재 날짜 표기
                    Text nowDateComponent = _ltgo.NowDate.GetComponentInChildren<Text>();
                    nowDateComponent.text = _ltvo.NowYear + "년 " + _ltvo.NowMonth + "월";

                    foreach (Dictionary<string, object> dic in _ltvo.TodoList)
                    {
                        // 버튼 프리팹 인스턴스화
                        _ltgo.TodoListInstance = Instantiate(_ltgo.TodoListPrefab, _ltgo.TodoListLayout.transform);
                        // 이미지 오브젝트에 딕셔너리 값 설정
                        Text todoNameTxtComponent = _ltgo.TodoListInstance.GetComponentInChildren<Text>();
                        if (todoNameTxtComponent != null)
                        {
                            Debug.Log(dic["TODONAME"]);
                            Debug.Log(dic["REWARD"]);
                            Debug.Log(dic["LOSEREWARD"]);
                            Debug.Log(dic["STATREWARD"]);

                            string todoName = dic["TODONAME"].ToString();
                            int reward = Convert.ToInt32(dic["REWARD"]);
                            int loseReward = Convert.ToInt32(dic["LOSEREWARD"]);
                            int statRewardI = Convert.ToInt32(dic["STATREWARD"]);
                            int todoNo = Convert.ToInt32(dic["TODONO"]);

                            // 값 초기화
                            string statReward = "";
                            _ltgo.TodoListInstance.name = "TodoBtn" + todoNo;
                            // TODOList의 각 요소에 컴포넌트 추가
                            TodoNameComponentVo todoNameComponent =
                                _ltgo.TodoListInstance.GetComponent<TodoNameComponentVo>();

                            statReward = (statRewardI % 2) switch
                            {
                                // statReward의 마지막 숫자가 0이면 힘, 1이면 마력
                                0 => "힘 " + (statRewardI / 10),
                                1 => "마력 " + (statRewardI / 10),
                                _ => statReward
                            };
                            // 컴포넌트에 값 저장
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

                    // 부모 오브젝트 비활성화
                    _ltgo.TodoList.SetActive(false);
                    // 첫 날 선택
                    Destroy(GameObject.Find("CalenderOutline"));
                    GameObject.Find("CalenderOutline").transform.SetAsFirstSibling();
                    GameObject.Find(_ltvo.IsSelectDate).GetComponent<Outline>().enabled = true;
                }));
            }));
        }


        // 턴 시작
        private void StartTurn()
        {
            // 각 텍스트 컴포넌트 참조
            Text yearTxtComponent = _ltgo.YearTxt.GetComponentInChildren<Text>();
            Text monthTxtComponent = _ltgo.MonthTxt.GetComponentInChildren<Text>();
            Text dateTxtComponent = _ltgo.DateTxt.GetComponentInChildren<Text>();
            Text timeTxtComponent = _ltgo.TimeTxt.GetComponentInChildren<Text>();
            Text todoNameTxtComponent = _ltgo.TodoNameTxt.GetComponentInChildren<Text>();
            // 라이프 타임의 텍스트 컴포넌트 참조
            Text lifeTimeMainComponent = _ltgo.LifeTimeMain.GetComponentInChildren<Text>();
            // 각 텍스트 컴포넌트에 값 입력
            yearTxtComponent.text = _ltvo.NowYear + "년";
            monthTxtComponent.text = _ltvo.NowMonth + "월";
            dateTxtComponent.text = _ltvo.NowDate + "일";
            timeTxtComponent.text = _ltvo.NowTime switch
            {
                0 => "아침",
                1 => "점심",
                2 => "저녁",
                _ => timeTxtComponent.text
            };

            // 아침, 저녁이면 대화 버튼 활성화, 점심에는 비활성화
            switch (_ltvo.NowTime)
            {
                case 0:
                    _ltgo.ConvBtn.SetActive(true);
                    break;
                case 1:
                    _ltgo.ConvBtn.SetActive(false);
                    break;
                case 2:
                    _ltgo.ConvBtn.SetActive(true);
                    break;
            }

            // 현재 날짜의 TODO 이름이 비어있으면 쉬는날
            if (_ltvo.PlanList[_ltvo.NowDate - 1]["TODONAME"].Equals(""))
            {
                todoNameTxtComponent.text = "쉬는날";
                lifeTimeMainComponent.text = "";
            }
            else
            {
                todoNameTxtComponent.text = _ltvo.PlanList[_ltvo.NowDate - 1]["TODONAME"].ToString();
                lifeTimeMainComponent.text = "보상: " + _ltvo.PlanList[_ltvo.NowDate - 1]["REWARD"] +
                                             "\n소모 재화: " + _ltvo.PlanList[_ltvo.NowDate - 1]["LOSEREWARD"] +
                                             "\n얻는 스탯: " + _ltvo.PlanList[_ltvo.NowDate - 1]["STATREWARD"];
            }
        }

        // 턴 종료
        private void EndTurn()
        {
            _ltgo.StartTurn.SetActive(false);
            if (_ltvo.NowMonth < 12) // 11월까지 월 증가, 일 초기화
            {
                _ltvo.NowMonth++;
                _ltvo.NowDate = 1;
            }
            else // 12월이면 연도 증가, 월, 일 초기화
            {
                _ltvo.NowMonth = 1;
                _ltvo.NowYear++;
                _ltvo.NowDate = 1;
            }

            _ltvo.PlanList.Clear();
            RemoveTodoList();
            RemoveCalendar();
            InitTodoList();
        }

        private void RemoveTodoList()
        {
            foreach (Transform child in _ltgo.TodoListLayout.transform)
            {
                if (!child.gameObject.activeSelf) continue;
                Destroy(child.gameObject);
            }
        }

        private void RemoveCalendar()
        {
            foreach (Transform date in _ltgo.CalenderLayout.transform)
            {
                date.gameObject.GetComponent<Image>().color = Color.white;
                date.gameObject.GetComponentInChildren<Text>().text = "";
                DeleteDateComponent(date.gameObject);
            }
        }

        private static void DeleteDateComponent(GameObject obj)
        {
            TodoNameComponentVo component = obj.GetComponent<TodoNameComponentVo>();
            component.todoName = "";
            component.reward = 0;
            component.loseReward = 0;
            component.statReward = "";
        }

        // 오브젝트의 이름과 색상을 매개변수로 받아 이미지 색상 변경
        private static void ChangeImageColor(string objectName, Color color)
        {
            GameObject obj = GameObject.Find(objectName);
            Image image = obj.GetComponent<Image>();
            image.color = color;
        }

        // TODO 리스트의 인덱스를 입력받아 색상 지정
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
    }
}