namespace Script.UI.Outing.RestaurantScript
{
    public class FoodListVO { 
        public int FoodNo { get; set; }
        public string FoodNm { get; set; }
        public int FoodPr { get; set; }

        public FoodListVO()
        {
        }
        public FoodListVO(int foodNo, string foodNm, int foodPr)
        {
            FoodNo = foodNo;
            FoodNm = foodNm;
            FoodPr = foodPr;
        }
    }
}
