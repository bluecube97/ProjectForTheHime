namespace Script.UI.MainLevel.Inventory
{
    public class InventoryVO
    {
        public int ItemNo { get; set; } 
        public string ItemNm { get; set; }
        public string ItemDese { get; set; }
        public int ItemCnt { get; set; }
        public InventoryVO()
        {
        }

        public InventoryVO(int itemNo, string itemNm, string itemDese, int itemCnt)
        {
            ItemNo = itemNo;
            ItemNm = itemNm;
            ItemDese = itemDese;
            ItemCnt = itemCnt;
        }
    }
}
