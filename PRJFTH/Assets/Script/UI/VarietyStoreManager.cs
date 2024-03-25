using UnityEngine;
using UnityEngine.SceneManagement;

public class VarietyStoreManager : MonoBehaviour
{
    public void OnClickReturn()
    {
        SceneManager.LoadScene("OutingScene");
    }
}