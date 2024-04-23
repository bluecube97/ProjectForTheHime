using UnityEngine.SceneManagement;

namespace Script.UI.MainLevel.StartTurn
{
    using UnityEngine;

    public class LifeTimeManager : MonoBehaviour
    {
        public void OnClickComplete()
        {
            Debug.Log("Complete");
        }
        public void OnClickReturn()
        {
            SceneManager.LoadScene("StartTurnScene");
        }
    }
}