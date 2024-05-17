using UnityEngine;
using UnityEngine.Serialization;

namespace Script.UI.MainLevel.StartTurn.VO
{
    public class TodoNameComponentVo : MonoBehaviour
    {
        public string todoName; // 일정 이름
        public int reward; // 보상
        public int loseReward; // 소모 재화
        public string statReward; // 스탯 보상
        public int index; // 인덱스
    }
}