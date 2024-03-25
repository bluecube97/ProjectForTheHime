using UnityEngine;
using UnityEngine.SceneManagement;

public class SmithyManager : MonoBehaviour
{
    public void OnClickReturn()
    {
        SceneManager.LoadScene("OutingScene");
    }
}