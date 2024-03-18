using UnityEngine;
using UnityEngine.SceneManagement;

public class RestaurantManager : MonoBehaviour
{
    public void OnClickReturn()
    {
        SceneManager.LoadScene("OutingScene");
    }
}
