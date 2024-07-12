using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script._3D.UI
{
    public class Stage1GroundUI : GroundUI
    {
        // 장애물 위치를 저장하는 리스트
        private readonly List<Vector2Int> _blockedPosition = new List<Vector2Int>();
        // protected: 상속받은 클래스에서만 접근 가능
        // override: 상속 한 클래스의 virtual 메서드를 재정의
        protected override void Awake()
        {
            base.Awake();
            // 장애물 위치를 설정하는 메서드
            BlockedPosition();
            // 모든 버튼을 순회하며 장애물이 있는 위치에 접근이 불가하도록 하는 기능
            foreach (Transform child in placeBtnLayout.transform)
            {
                PositionComponentVo positionComponent = child.GetComponent<PositionComponentVo>();
                if (positionComponent == null) continue;

                if (!_blockedPosition.Contains(new Vector2Int(positionComponent.posX, positionComponent.posZ))) continue;
                positionComponent.isBlock = true;
                child.GetComponent<Image>().color = Color.red;
                positionComponent.GetComponent<Button>().interactable = false;
            }
        }

        // 장애물 위치를 설정하는 메서드
        private void BlockedPosition()
        {
            _blockedPosition.Add(new Vector2Int(1, 0));
            _blockedPosition.Add(new Vector2Int(1, 1));
            _blockedPosition.Add(new Vector2Int(1, 2));
            _blockedPosition.Add(new Vector2Int(2, 0));
            _blockedPosition.Add(new Vector2Int(2, 1));
            _blockedPosition.Add(new Vector2Int(2, 2));
        }
    }
}