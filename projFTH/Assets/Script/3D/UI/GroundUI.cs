using UnityEngine;

namespace Script._3D.UI
{
    public class GroundUI : MonoBehaviour
    {
        public GameObject placeBtn;
        public GameObject placeBtnLayout;
        private void Start()
        {
            for (int i = 0; i < 200; i++)
            {
                Instantiate(placeBtn, placeBtnLayout.transform);
            }
            placeBtn.SetActive(false);
        }
    }
}