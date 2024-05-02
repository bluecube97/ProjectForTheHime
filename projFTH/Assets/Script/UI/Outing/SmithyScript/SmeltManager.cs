namespace Script.UI.Outing
{
    using global::System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class SmeltManager : MonoBehaviour 
    { 
        public GameObject smeltListPrefab; 
        public GameObject smeltList; 
        public Transform smeltListLayout; 
  

        private SmeltDao smeltDao;

    
        private List<GameObject> smeltListInstances = new List<GameObject>();

        private void Start()
        {

            smeltDao = GetComponent<SmeltDao>();
            UpdateSmeltList();

        }

        public void UpdateSmeltList()
        {
            smeltList.SetActive(true);

            List<Dictionary<string, object>> SmeltList = smeltDao.GetSmeltList();


            foreach (GameObject smeltInstance in smeltListInstances)
            {
                Destroy(smeltInstance);
            }
            smeltListInstances.Clear();

            
            int i = 0;
            foreach (var dic in SmeltList)
            {
                i++;

                GameObject smeltListInstance = Instantiate(smeltListPrefab, smeltListLayout);
                smeltListInstance.name = "SmeltList" + i;
                smeltListInstances.Add(smeltListInstance);

                Text textComponent = smeltListInstance.GetComponentInChildren<Text>();

                if (textComponent != null)
                {
                    textComponent.text = dic["item_name"] + "." +
                            " : " + dic["item_cost"] + "\r\n" ;
                            
                }
            }
            smeltList.SetActive(false);

        }
        

        public void OnClickSubmit()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            GameObject parentObject = clickedButton.transform.parent.gameObject;
            string parentObjectName = parentObject.name;
            Debug.Log(": " + parentObjectName);

            var SmeltList = smeltDao.GetSmeltList();
            string indexString = parentObjectName.Replace("SmeltList", "");
            int index = int.Parse(indexString);


            
            UpdateSmeltList();
        }

        public void OnClickReturn()
        {
            {
                SceneManager.LoadScene("OutingScene");
            }
        }
    }
}