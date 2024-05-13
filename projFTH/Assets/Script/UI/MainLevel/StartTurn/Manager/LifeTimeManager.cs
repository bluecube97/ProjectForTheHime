using Script.UI.MainLevel.StartTurn.Dao;
using Script.UI.MainLevel.StartTurn.VO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.UI.MainLevel.StartTurn.Manager
{
    public class LifeTimeManager : MonoBehaviour
    {
        private static int _nowYear = 2024; // 년도 초기화 (나중에 바뀜)
        private static int _nowMonth = 4; // 월 초기화 (나중에 바뀜)
        private static int _nowDate = 1; // 일 초기화
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
        public Transform calenderLayout; // 달력 레이아웃 참조

        public GameObject startTurn; // 시작 턴 이미지 참조
        public GameObject lifeTimeMain; // 라이프 타임 이미지 참조
        public GameObject convBtn; // 대화 버튼 참조
        private readonly List<Dictionary<string, object>> _planList = new(); // 달력에 적힌 일정을 담는 딕셔너리 리스트

        private bool _isSelectable = true; // TODO 버튼 클릭 가능 여부
        private string _isSelectDate = "Day1"; // 선택된 날짜

        private GameObject _myGameObject;
        private StartTurnDao _std;
        private List<Dictionary<string, object>> _todoList = new(); // TODO리스트를 담는 딕셔너리 리스트
        private GameObject _todoListInstance; // TODOList의 인스턴스

        public void Awake()
        {
            // StartTurnDao를 가져오기 위한 GameObject 생성
            _myGameObject = new GameObject();
            _std = _myGameObject.AddComponent<StartTurnDao>();
        }

        public void Start()
        {
            InitTodoList(); // TODOList 세팅
            todoList.SetActive(false); // 부모 오브젝트 비활성화
        }

        // TODOList 세팅
        private void InitTodoList()
        {
            // 현재 날짜의 연, 월을 입력받아 해당하는 TodoNO를 반환하여 리스트에 저장
            List<int> noList = _std.GetTodoNo(_nowYear, _nowMonth);
            // TodoNO를 이용하여 TodoList를 가져와 리스트에 저장
            _todoList = _std.GetTodoList(noList);
            // TODOList에 인덱스 지정 할 변수
            int index = 1;

            // 현재 날짜 표기
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

                    // 값 초기화
                    string statReward = "";
                    _todoListInstance.name = "TodoBtn" + todoNo;
                    // TODOList의 각 요소에 컴포넌트 추가
                    StartTurnVo todoNameComponent = _todoListInstance.GetComponent<StartTurnVo>();

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

            // 첫 날 선택
            GameObject.Find(_isSelectDate).GetComponent<Outline>().enabled = true;
        }

        // 달력의 날짜 버튼 OnClick 이벤트
        public void OnClickDateBtn(GameObject button)
        {
            _isSelectable = true; // TODO 버튼 클릭 가능
            // 원래 선택되어 있던 버튼 테두리 비활성화
            GameObject.Find(_isSelectDate).GetComponent<Outline>().enabled = false;
            // 선택된 버튼 테두리 활성화
            _isSelectDate = button.name;
            GameObject.Find(_isSelectDate).GetComponent<Outline>().enabled = true;
        }

        // TODO 버튼 OnClick 이벤트
        public void OnClickTodoBtn(GameObject button)
        {
            // TODO 버튼 클릭 가능 여부 판단 및 불가능 시 리턴
            if (!_isSelectable)
            {
                return;
            }

            GameObject dateBtn = GameObject.Find(_isSelectDate); // 선택된 날짜 버튼 참조
            StartTurnVo dateComponent = dateBtn.GetComponent<StartTurnVo>(); // 선택된 날짜 버튼의 컴포넌트 참조
            Text textComponent = dateBtn.GetComponentInChildren<Text>(); // 선택된 날짜 버튼의 텍스트 컴포넌트 참조
            StartTurnVo todoNameComponent = button.GetComponent<StartTurnVo>(); // 선택된 TODO 버튼의 컴포넌트 참조
            textComponent.text = todoNameComponent.todoName; // 선택된 날짜 버튼의 텍스트 변경
            // 선택된 날짜 버튼의 컴포넌트 값 변경
            dateComponent.todoName = todoNameComponent.todoName;
            dateComponent.reward = todoNameComponent.reward;
            dateComponent.loseReward = todoNameComponent.loseReward;
            dateComponent.statReward = todoNameComponent.statReward;
            // 선택된 날짜 버튼의 색상 변경
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

        // 오브젝트의 이름과 색상을 매개변수로 받아 이미지 색상 변경
        private static void ChangeImageColor(string objectName, Color color)
        {
            GameObject obj = GameObject.Find(objectName);
            Image image = obj.GetComponent<Image>();
            image.color = color;
        }

        // 결정 버튼 OnClick 이벤트
        public void OnClickComplete()
        {
            for (int i = 1; i <= 20; i++) // 20일간 반복
            {
                string findDate = "Day" + i; // "Day1" ~ "Day20"
                GameObject findDateBtn = GameObject.Find(findDate); // "Day1" ~ "Day20" 오브젝트 참조
                StartTurnVo dateComponent = findDateBtn.GetComponent<StartTurnVo>(); // "Day1" ~ "Day20" 오브젝트의 컴포넌트 참조
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
                _planList.Add(dic);
            }

            // 시작 턴 이미지 활성화
            startTurn.SetActive(true);
            StartTurn(); // 턴 시작
        }

        // 턴 시작
        private void StartTurn()
        {
            // 각 텍스트 컴포넌트 참조
            Text yearTxtComponent = yearTxt.GetComponentInChildren<Text>();
            Text monthTxtComponent = monthTxt.GetComponentInChildren<Text>();
            Text dateTxtComponent = dateTxt.GetComponentInChildren<Text>();
            Text timeTxtComponent = timeTxt.GetComponentInChildren<Text>();
            Text todoNameTxtComponent = todoNameTxt.GetComponentInChildren<Text>();
            // 라이프 타임의 텍스트 컴포넌트 참조
            Text lifeTimeMainComponent = lifeTimeMain.GetComponentInChildren<Text>();
            // 각 텍스트 컴포넌트에 값 입력
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

            // 아침, 저녁이면 대화 버튼 활성화, 점심에는 비활성화
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

            // 현재 날짜의 TODO 이름이 비어있으면 쉬는날
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
        }

        // 턴 종료
        private void EndTurn()
        {
            startTurn.SetActive(false);
            if (_nowMonth < 12) // 11월까지 월 증가, 일 초기화
            {
                _nowMonth++;
                _nowDate = 1;
            }
            else // 12월이면 연도 증가, 월, 일 초기화
            {
                _nowMonth = 1;
                _nowYear++;
                _nowDate = 1;
            }

            _planList.Clear();
            RemoveTodoList();
            RemoveCalendar();
            InitTodoList();
        }

        private void RemoveTodoList()
        {
            foreach (Transform child in todoListLayout)
            {
                if (!child.gameObject.activeSelf) continue;
                Destroy(child.gameObject);
            }
        }

        private void RemoveCalendar()
        {
            foreach (Transform child in calenderLayout)
            {
                child.gameObject.GetComponent<Image>().color = Color.white;
                child.gameObject.GetComponentInChildren<Text>().text = "";
            }
        }

        // 다음 날짜로 넘어가기
        public void OnClickNextPhase()
        {
            _nowTime++; // 시간 증가
            if (_nowTime > 2) // 저녁이면, 아침으로
            {
                _nowTime = 0;
                _nowDate++;
            }

            if (_nowDate <= 20) // 20일까지 반복
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
            if (!_isSelectable)
            {
                return;
            }

            ChangeImageColor(_isSelectDate, Color.white);
            GameObject dateBtn = GameObject.Find(_isSelectDate);
            Text textComponent = dateBtn.GetComponentInChildren<Text>();

            textComponent.text = "";
        }

        // 이전 씬으로 돌아가기
        public void OnClickReturn()
        {
            SceneManager.LoadScene("StartTurnScene");
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