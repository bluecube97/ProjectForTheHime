namespace Script.UI.Outing.QuestBoard
{
    public class QuestBoardVO
    {


        public int QuestNo { get; set; }
        public string QuestNm { get; set; }
        public string QuestMemo { get; set; }
        public string SubmitFlag { get; set; }
        public string CompleteFlag { get; set; }

        public QuestBoardVO()
        {

        }

        public QuestBoardVO(int questNo, string questNm, string questMemo, string submitFlag, string completeFlag)
        {
            QuestNo = questNo;
            QuestNm = questNm;
            QuestMemo = questMemo;
            SubmitFlag = submitFlag;
            CompleteFlag = completeFlag;
        }
    }
}
