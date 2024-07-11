using Script._3D.Player;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script._3D.UI
{
    public class GroundUI : MonoBehaviour
    {
        private PlayerManager _pm;

        public GameObject placeBtn;
        public GameObject placeBtnLayout;
        public GameObject placeBtnInstance;
        public int linePlaceBtnCnt;
        public GameObject player;
        public int startPlayerPosX;
        public int startPlayerPosZ;
        public GameObject canvas;

        public GameObject cam; // 카메라
        private SuperBlur.SuperBlur _blur; // 블러

        public GameObject uiCanvas; // UI 캔버스
        public Button diceBtn; // 주사위 버튼
        public Text diceValueTxt; // 주사위 값 텍스트
        private int _diceValue; // 주사위 값

        // protected: 상속받은 클래스에서만 접근 가능
        // virtual: 상속받은 클래스에서 재정의 가능
        protected virtual void Awake()
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

            Vector3 startPosition = new(playerPositionComponent.posX * 5.5f, 1.1f, playerPositionComponent.posZ * -5.5f);
            player.transform.position = startPosition;
            player.GetComponent<PlayerManager>().targetPosition = startPosition;

            _blur = cam.GetComponent<SuperBlur.SuperBlur>();
        }

        private void Start()
        {
            _pm = player.GetComponent<PlayerManager>();
            // 주사위 페이즈 시작
            DicePhase();
        }

        private void SetPlaceBtnDistance()
        {
            foreach (Transform child in placeBtnLayout.transform)
            {
                PositionComponentVo positionComponent = child.GetComponent<PositionComponentVo>();
                if (positionComponent == null) continue;

                int distanceX = Mathf.Abs(positionComponent.posX - player.GetComponent<PositionComponentVo>().posX);
                int distanceZ = Mathf.Abs(positionComponent.posZ - player.GetComponent<PositionComponentVo>().posZ);

                Text childTxt = child.GetComponentInChildren<Text>();
                childTxt.text = Convert.ToString(distanceX + distanceZ);
            }
        }

        // 땅 버튼 클릭시 호출
        public void OnClickPlaceBtn(Button button)
        {
            int btnX = button.GetComponent<PositionComponentVo>().posX;
            int btnZ = button.GetComponent<PositionComponentVo>().posZ;

            // 이동 거리 계산
            int distance = CalcDistance(btnX, btnZ);

            _pm.ChangeMoveCnt(PlayerManager.MoveCnt - distance);
            Debug.Log("MoveCnt: " + PlayerManager.MoveCnt);

            player.GetComponent<PlayerManager>().targetPosition = new Vector3(btnX * 5.5f, 1.1f, btnZ * -5.5f);
        }
        // blur 비활성화
        private void DisableBlur()
        {
            _blur.interpolation = 0;
            _blur.downsample = 0;
        }
        // blur 활성화
        private void EnableBlur()
        {
            _blur.interpolation = 0.8f;
            _blur.downsample = 1;
        }
        // 주사위 페이즈
        public void DicePhase()
        {
            _diceValue = 1;
            EnableBlur();
            uiCanvas.SetActive(true);
            diceValueTxt.text = "";

            SetPlaceBtnDistance();
        }
        // 이동 페이즈
        private void MovePhase()
        {
            DisableBlur();
            uiCanvas.SetActive(false);
        }

        private int CalcDistance(int btnX, int btnZ)
        {
            return Mathf.Abs(btnX - player.GetComponent<PositionComponentVo>().posX) +
                   Mathf.Abs(btnZ - player.GetComponent<PositionComponentVo>().posZ);
        }

        // 주사위 굴리기 버튼 클릭 시 호출
        public void OnClickDiceBtn(Button button)
        {
            if (button.GetComponentInChildren<Text>().text == "확인")
            {
                button.GetComponentInChildren<Text>().text = "굴리기!";
                MovePhase();
            }
            else if (button.GetComponentInChildren<Text>().text == "굴리기!")
            {
                StartCoroutine(SetDiceValue(0.2f, callback =>
                {
                    _diceValue = callback;
                }));
            }
        }
        // 주사위를 굴리는 코루틴
        private IEnumerator SetDiceValue(float time, Action<int> callback)
        {
            diceBtn.interactable = false;
            diceValueTxt.text = "주사위를 굴리는 중.";
            yield return new WaitForSeconds(time);
            diceValueTxt.text = "주사위를 굴리는 중..";
            yield return new WaitForSeconds(time);
            diceValueTxt.text = "주사위를 굴리는 중...";
            yield return new WaitForSeconds(time);

            int diceValue = Random.Range(1, 7);
            diceValueTxt.text = diceValue.ToString();
            _pm.ChangeMoveCnt(diceValue);
            diceBtn.interactable = true;
            diceBtn.GetComponentInChildren<Text>().text = "확인";

            callback(diceValue);
        }
    }
}