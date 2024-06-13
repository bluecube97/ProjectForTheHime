namespace Script.UI.Outing.ClothingStore
{
    public class ClothingVO
    {
        //구매 하는 아이템 받아오는 vo
        public string itemid { get; set; }
        public string itemnm { get; set; }
        public string itemdesc { get; set; }
        public string sellprice { get; set; }
        public string buyprice { get; set; }
        
        //제작 시 필요 값 받아오는 아이템 vo
        public string r_id { get; set; }
        
        public string r_itemid { get; set; }
        public string r_name { get; set; }
        public string r_desc { get; set; }

        public string req_item { get; set; }
        public string req_name { get; set; }

        public string req_itemcnt { get; set; }
    }
}
