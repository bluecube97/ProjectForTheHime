using UnityEngine;
using UnityEngine.SceneManagement;

public class HospitalManager : MonoBehaviour
{
    public void OnClickReturn()
    {
        SceneManager.LoadScene("OutingScene");
    }
}