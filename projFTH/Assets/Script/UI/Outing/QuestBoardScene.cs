namespace Script.UI.Outing
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class QuestBoardScene : MonoBehaviour
    {
        public void OnClickReturn()
        {
            SceneManager.LoadScene("OutingScene");
        }
    }
}