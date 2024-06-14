using Script.UI.MainLevel.Inventory;
using System.Collections.Generic;

namespace Script.UI.Outing.Hospital
{
    public class HospitalVo
    {
        public int price;
        public string itemid;

        public List<Dictionary<string, object>> BuyList = new();
        public Dictionary<string, object> Userinfo = new();
        public List<InventoryVO> invenList = new();
    }
}