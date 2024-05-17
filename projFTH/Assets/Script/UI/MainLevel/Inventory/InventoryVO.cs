namespace Script.UI.MainLevel.Inventory
{
    public class InventoryVO
    {
        public string ItemNo { get; set; } 
        public string ItemNm { get; set; }
        public string ItemDese { get; set; }
        public string ItemCnt { get; set; }
        public InventoryVO()
        {
        }

        public InventoryVO(string itemNo, string itemNm, string itemDese, string itemCnt)
        {
            ItemNo = itemNo;
            ItemNm = itemNm;
            ItemDese = itemDese;
            ItemCnt = itemCnt;
        }
    }
}
