namespace Script.UI.Outing
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class EducateManager : MonoBehaviour
    {
        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
    }
}