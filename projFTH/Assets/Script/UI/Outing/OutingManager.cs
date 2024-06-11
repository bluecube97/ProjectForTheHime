using Script.UI.System;

namespace Script.UI.Outing
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class OutingManager : MonoBehaviour
    {
        public void OnClickRestaurant()
        {
            StartCoroutine(ConnDB.GetData());
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

        public void OnClickQuestBoard()
        {
            SceneManager.LoadScene("QuestBoardScene");
        }

        public void OnClickAdventure()
        {
            SceneManager.LoadScene("AdventureScene");
        }

        public void OnClickMainLevel()
        {
            SceneManager.LoadScene("MainLevelScene");
        }
    }
}