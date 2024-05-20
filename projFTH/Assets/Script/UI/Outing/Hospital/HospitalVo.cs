using System.Collections.Generic;

namespace Script.UI.Outing.Hospital
{
    public class HospitalVo
    {
        public int price;
        public List<Dictionary<string, object>> SellList = new();
        public Dictionary<string, object> Userinfo = new();

    }
}