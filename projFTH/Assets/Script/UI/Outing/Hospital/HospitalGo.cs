using UnityEngine;

namespace Script.UI.Outing.Hospital
{
    public class HospitalGo
    {
        public readonly GameObject BuyListPrefab = GameObject.Find("Buyitem");
        
        public readonly GameObject BuyList = GameObject.Find("Buyitem");
        
        public readonly GameObject BuyListLayout = GameObject.Find("BuyLayout");
        
        
        
        public readonly GameObject CureMenu = GameObject.Find("CureMenuBackGround");
        
        public readonly GameObject BuyMenu = GameObject.Find("HospitalBuyBackGround");
        
        public readonly GameObject SellMenu = GameObject.Find("HospitalSellBackGround");
        
        public  GameObject hospitalInstances;
    }
}