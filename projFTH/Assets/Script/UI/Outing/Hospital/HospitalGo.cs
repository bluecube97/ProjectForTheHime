using UnityEngine;

namespace Script.UI.Outing.Hospital
{
    public class HospitalGo
    {
        
        public readonly GameObject BuyListPrefab = GameObject.Find("Buyitem");//buylist이미지 참조
        public readonly GameObject BuyList = GameObject.Find("Buyitem");//buylist이미지 참조
        public readonly GameObject BuyListLayout = GameObject.Find("BuyLayout");//구매 레이아웃 참조
        
        
        public readonly GameObject ChoiceMenu = GameObject.Find("ChoiceBackGround");//선택 이미지 참조
        public readonly GameObject CureMenu = GameObject.Find("CureMenuBackGround");//치료 이미지 참조
        public readonly GameObject BuyMenu = GameObject.Find("HospitalBuyBackGround");//구매목록 이미지 참조
        public readonly GameObject SellMenu = GameObject.Find("HospitalSellBackGround");//판매목록 이미지 참조
        
        public  GameObject BuyListInstances;//buyList의 인스턴스ㄴ
    }
}