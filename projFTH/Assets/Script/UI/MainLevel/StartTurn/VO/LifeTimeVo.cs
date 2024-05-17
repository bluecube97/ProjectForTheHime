using System.Collections.Generic;

namespace Script.UI.MainLevel.StartTurn.VO
{
    // LifeTime의 변수를 한번에 관리하기 위한 클래스
    public class LifeTimeVo
    {
        public int NowYear = 2024; // 년도 초기화 (나중에 바뀜)
        public int NowMonth = 4;
        public int NowDate = 1; // 일 초기화
        public int NowTime = 0; // 0: 아침, 1: 점심, 2: 저녁

        public bool IsSelectable = true; // TODO 버튼 클릭 가능 여부
        public string IsSelectDate = "Day1"; // 선택된 날짜

        public readonly List<Dictionary<string, object>> PlanList = new(); // 달력에 적힌 일정을 담는 딕셔너리 리스트
        public List<Dictionary<string, object>> TodoList = new(); // TODO리스트를 담는 딕셔너리 리스트
    }
}