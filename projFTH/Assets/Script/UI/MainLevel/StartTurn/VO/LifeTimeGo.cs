using UnityEngine;

namespace Script.UI.MainLevel.StartTurn.VO
{
    // LifeTime의 GameObject들을 한번에 관리하기 위한 클래스
    public class LifeTimeGo
    {
        public readonly GameObject YearTxt = GameObject.Find("YearTxt"); // 년도 텍스트 참조
        public readonly GameObject MonthTxt = GameObject.Find("MonthTxt"); // 월 텍스트 참조
        public readonly GameObject DateTxt = GameObject.Find("DateTxt"); // 일 텍스트 참조
        public readonly GameObject TimeTxt = GameObject.Find("TimeTxt"); // 시간 텍스트 참조
        public readonly GameObject TodoNameTxt = GameObject.Find("TodoNameTxt"); // TODO 이름 텍스트 참조

        public readonly GameObject NowDate = GameObject.Find("NowDate"); // 현재 날짜 참조
        public readonly GameObject TodoListPrefab = GameObject.Find("TODOList"); // TODOList 이미지 프리팹 참조
        public readonly GameObject TodoList = GameObject.Find("TODOList"); // TODOList 이미지 참조

        public readonly GameObject TodoListLayout = GameObject.Find("TODOListLayout"); // TODOList들이 들어갈 레이아웃 참조
        public readonly GameObject CalenderLayout = GameObject.Find("CalenderLayout"); // 달력 레이아웃 참조

        public readonly GameObject StartTurn = GameObject.Find("StartTurnBackGround"); // 시작 턴 이미지 참조
        public readonly GameObject LifeTimeMain = GameObject.Find("LifeTimeMain"); // 라이프 타임 이미지 참조
        public readonly GameObject ConvBtn = GameObject.Find("ConvBtn"); // 대화 버튼 참조

        public GameObject TodoListInstance; // TODOList의 인스턴스
    }
}