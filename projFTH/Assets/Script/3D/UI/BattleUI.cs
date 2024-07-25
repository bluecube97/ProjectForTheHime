using Script._3D.Dao;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script._3D.UI
{
    public class BattleUI : MonoBehaviour
    {
        private GroundUI _groundUI;
        private BattleDao _battleDao;

        public GameObject battleCanvas;
        public GameObject behaviorInstance;
        public GameObject behaviorPrefab;
        public GameObject behaviorLayout;

        private void Awake()
        {
            _groundUI = FindObjectOfType<GroundUI>();
            _battleDao = FindObjectOfType<BattleDao>();
        }

        public void StartBattle()
        {
            Debug.Log("Battle Start");
            battleCanvas.SetActive(true);
            SetBehavior("Attack");

            StartCoroutine(BattleDao.GetMobList(_groundUI.appearMobList, list =>
            {
                foreach (Dictionary<string, object> mob in list)
                {
                    Debug.Log("Mob: " + mob["mobno"]);
                    Debug.Log("MobName: " + mob["mobname"]);
                }
            }));
        }

        private void SetBehavior(string action)
        {
            behaviorPrefab.SetActive(true);
            for (int i = 0; i < 8; i++)
            {
                behaviorInstance = Instantiate(behaviorPrefab, behaviorLayout.transform);
                Text behaviorText = behaviorInstance.GetComponentInChildren<Text>();
                behaviorText.text = action + " " + i;
            }
            behaviorPrefab.SetActive(false);
        }

        private void RemoveBehavior()
        {
            foreach (Transform child in behaviorLayout.transform)
            {
                if (!child.gameObject.activeSelf) continue;
                Destroy(child.gameObject);
            }
        }

        public void OnClickAttackBtn()
        {
            RemoveBehavior();
            SetBehavior("Attack");
        }

        public void OnClickSkillBtn()
        {
            RemoveBehavior();
            SetBehavior("Skill");
        }

        public void OnClickItemBtn()
        {
            RemoveBehavior();
            SetBehavior("Item");
        }

        public void OnClickRunBtn()
        {
            RemoveBehavior();
            SetBehavior("Run");
        }
    }
}