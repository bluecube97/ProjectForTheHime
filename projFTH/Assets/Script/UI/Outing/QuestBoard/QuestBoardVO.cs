namespace Script.UI.Outing.QuestBoard
{
    public class QuestBoardVO
    {


        //퀘스트 번호
        public int QuestNo { get; set; }
        //퀘스트 이름
        public string QuestNm { get; set; }
        //퀘스트 내용
        public string QuestMemo { get; set; }
        //퀘스트 수락 플래그
        public string SubmitFlag { get; set; }
        //퀘스트 완료플래그
        public string CompleteFlag { get; set; }

        //퀘스트 요구 아이템코드
        public string Qitem { get; set; }
        //퀘스트 요구 아이템 이름
        public string QitemNm { get; set; }
        //퀘스트 요구 아이템 갯수
        public string Qitem_cnt { get; set; }
        //퀘스트 보상 아이템코드
        public string Qreward { get; set; }
        //퀘스트 보상 아이템 이름
        public string QrewardNm { get; set; }
        //퀘스트 보상 아이템 갯수
        public string Qreward_cnt { get; set; }

    }
}
