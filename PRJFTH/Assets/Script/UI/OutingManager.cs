using UnityEngine;
using UnityEngine.SceneManagement;

public class OutingManager : MonoBehaviour
{
    public void OnClickEdu()
    {
        SceneManager.LoadScene("EducateScene");
    }

    public void OnClickRestaurant()
    {
        SceneManager.LoadScene("RestaurantScene");
    }

    public void OnClickSmithy()
    {
        SceneManager.LoadScene("SmithyScene");
    }

    public void OnClickClothingStore()
    {
        SceneManager.LoadScene("ClothingStoreScene");
    }

    public void OnClickVarietyStore()
    {
        SceneManager.LoadScene("VarietyStoreScene");
    }

    public void OnClickHospital()
    {
        SceneManager.LoadScene("HospitalScene");
    }

    public void OnClickAdventure()
    {
        SceneManager.LoadScene("AdventureScene");
    }
}
