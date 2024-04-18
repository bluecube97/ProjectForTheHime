using UnityEngine;
using UnityEngine.SceneManagement;

public class ClothingStoreManager : MonoBehaviour
{
    public void OnClickReturn()
    {
        SceneManager.LoadScene("OutingScene");
    }
}