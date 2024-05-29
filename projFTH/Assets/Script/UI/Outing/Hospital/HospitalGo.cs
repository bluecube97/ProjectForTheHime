using UnityEngine;

namespace Script.UI.Outing.Hospital
{
    public class HospitalGo
    {
        public readonly GameObject HospitalPrefab = GameObject.Find("SellItemList");
        
        public readonly GameObject Hospital = GameObject.Find("SellItemList");
        
        public readonly GameObject HospitalLayout = GameObject.Find("BuyLayout");
        
        
        
        public readonly GameObject CureMenu = GameObject.Find("CureMenu");
        
        public readonly GameObject BuyMenu = GameObject.Find("BuyMenu");
        
        public readonly GameObject SellMenu = GameObject.Find("SellMenu");
        
        public  GameObject HospitalUIInstance;
        public  GameObject hospitalInstances;
    }
}