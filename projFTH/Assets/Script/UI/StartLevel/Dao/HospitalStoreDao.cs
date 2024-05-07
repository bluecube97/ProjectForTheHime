using MySql.Data.MySqlClient;
using UnityEngine;

namespace Script.UI.StartLevel.Dao
{
    public class  HospitalStoreDao : MonoBehaviour
    {
        private readonly string con = "Server=localhost;Database=projfth;Uid=root;Pwd=1234;Charset=utf8mb4";

          public void ChiefDoctorCure(int money)
        {
            if (money >= 100)
            {
                // DB 연결
                using (MySqlConnection connection = new MySqlConnection(con))
                {
                    connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        // SQL 쿼리 작성
                        /*string sql = "INSERT INTO tbl_test (Money) VALUES (@money)";*/
                        string sql = "";

                        
                        // 매개변수 설정
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@money", money);

                        // 쿼리 실행
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}

