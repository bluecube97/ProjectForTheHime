namespace Script.UI.MainLevel.StartTurn
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class StartTurnScene : MonoBehaviour
    {
        public void OnClickBattleBtn()
        {
            SceneManager.LoadScene("BattleScene");
        }

        public void OnClickLifeTimeBtn()
        {
            SceneManager.LoadScene("LifeTimeScene");
        }

        public void OnClickReturn()
        {
            SceneManager.LoadScene("MainLevelScene");
        }
    }
}