using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace Script._3D.UI
{
    public class GroundUI : MonoBehaviour
    {
        public GameObject placeBtn;
        public GameObject placeBtnLayout;
        public GameObject placeBtnInstance;
        public int linePlaceBtnCnt;
        public GameObject player;
        public int startPlayerPosX;
        public int startPlayerPosZ;
        public GameObject canvas;

        private void Awake()
        {
            int maxPlaceBtn = linePlaceBtnCnt * linePlaceBtnCnt;

            RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
            canvasRectTransform.sizeDelta = new Vector2(70 + 50 * linePlaceBtnCnt + 5 * (linePlaceBtnCnt - 1),
                70 + 50 * linePlaceBtnCnt + 5 * (linePlaceBtnCnt - 1));

            for (int i = 0; i < maxPlaceBtn; i++)
            {
                placeBtnInstance = Instantiate(placeBtn, placeBtnLayout.transform);
                PositionComponentVo positionComponent = placeBtnInstance.GetComponent<PositionComponentVo>();
                positionComponent.posX = (i % linePlaceBtnCnt) - (linePlaceBtnCnt / 2);
                positionComponent.posZ = (i / linePlaceBtnCnt) - (linePlaceBtnCnt / 2);
                Text placeBtnTxtComponent = placeBtnInstance.GetComponentInChildren<Text>();
                placeBtnTxtComponent.text = positionComponent.posX + ", " + positionComponent.posZ;
            }

            placeBtn.SetActive(false);

            PositionComponentVo playerPositionComponent = player.GetComponent<PositionComponentVo>();
            playerPositionComponent.posX = startPlayerPosX;
            playerPositionComponent.posZ = startPlayerPosZ;

            player.transform.position = new Vector3(playerPositionComponent.posX, 0, playerPositionComponent.posZ);
        }

        public void OnClickPlaceBtn(Button button)
        {
            int btnX = button.GetComponent<PositionComponentVo>().posX;
            int btnZ = button.GetComponent<PositionComponentVo>().posZ;
            player.transform.position = new Vector3(btnX * 5.5f, 0, btnZ * -5.5f);

        }
    }
}