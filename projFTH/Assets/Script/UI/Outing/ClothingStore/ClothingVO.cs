namespace Script.UI.Outing.ClothingStore
{
    public class ClothingVO
    {
   

        public int cloNo { get; set; }
        public string cloNm { get; set; }
        public int slikCnt { get; set; }
        public int linCnt { get; set; }
        public string buyFlag { get; set; }

        public int seq { get; set; }
        public int clsNo { get; set; }
        public string clsNm { get; set; }
        public string clsDes { get; set; }
        public int clspri { get; set; }


        public ClothingVO()
        {
        }

        public ClothingVO(int cloNo, string cloNm, int slikCnt, int linCnt, string buyFlag)
        {
            this.cloNo = cloNo;
            this.cloNm = cloNm;
            this.slikCnt = slikCnt;
            this.linCnt = linCnt;
            this.buyFlag = buyFlag;
        }

        public ClothingVO(int seq, int clsNo, string clsNm, string clsDes, int clspri)
        {
            this.seq = seq;
            this.clsNo = clsNo;
            this.clsNm = clsNm;
            this.clsDes = clsDes;
            this.clspri = clspri;
        }
    }
}
