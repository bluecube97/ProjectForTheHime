namespace Script.UI.Outing.RestaurantScript
{
    public class FoodListVO { 
        public string FoodNo { get; set; }
        public string FoodNm { get; set; }
        public string FoodPr { get; set; }

        public FoodListVO()
        {
        }
        public FoodListVO(string foodNo, string foodNm, string foodPr)
        {
            FoodNo = foodNo;
            FoodNm = foodNm;
            FoodPr = foodPr;
        }
    }
}
