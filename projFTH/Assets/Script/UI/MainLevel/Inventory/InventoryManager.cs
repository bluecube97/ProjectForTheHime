using Script.UI.StartLevel.Dao; // Script.UI.StartLevel.Dao 네임스페이스 사용
using System.Collections.Generic; // 제네릭 컬렉션 사용
using UnityEngine; // Unity 엔진 사용
using UnityEngine.UI; // Unity UI 사용

namespace Script.UI.MainLevel.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        private static InventoryManager instance; // 싱글턴 인스턴스 변수
        public GameObject InventoryMenu; // 인벤토리 메뉴 오브젝트
        public GameObject inventoryPrefab; // 인벤토리 프리팹 참조
        public GameObject inventory; // 인벤토리 오브젝트 참조

        public Transform inventorytLayout; // 인벤토리 레이아웃 참조
        private List<GameObject> inventoryInstances = new List<GameObject>(); // 생성된 인벤토리 인스턴스 리스트

        private InventoryDao inven; // InventoryDao 인스턴스
        private List<InventoryVO> InvenList = new List<InventoryVO>(); // 인벤토리 VO 리스트
        private List<Dictionary<string,object>> inventoryList = new List<Dictionary<string,object>>(); // 인벤토리 리스트

        private Dictionary<string, object> userinfo = new(); // 사용자 정보 딕셔너리
        private StartLevelDao _sld; // StartLevelDao 인스턴스
        private string pid; // 사용자 이메일 ID

        private void Awake()
        {
            // 인스턴스가 없을 경우 현재 GameObject에 InventoryManager를 추가
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public static InventoryManager Instance => instance; // 인스턴스 프로퍼티

        private void Start()
        {
            _sld = GetComponent<StartLevelDao>(); // StartLevelDao 컴포넌트 가져오기
            inven = GetComponent<InventoryDao>(); // InventoryDao 컴포넌트 가져오기
            StartCoroutine(_sld.GetUserEmail(info => // 사용자 이메일 가져오는 코루틴 시작
            {
                userinfo = info;
                pid = userinfo["useremail"].ToString();
                StartCoroutine(inven.GetInventoryList(pid, list => // 인벤토리 리스트 가져오는 코루틴 시작
                {
                    inventoryList = list;
                    StartInven(inventoryList); // 인벤토리 초기화 함수 호출
                }));
            }));
        }

        public void StartInven(List<Dictionary<string,object>> inventoryList)
        {
            inventory.SetActive(true);

            // 기존에 생성된 인벤토리 오브젝트들을 제거
            foreach (GameObject invenInstance in inventoryInstances)
            {
                Destroy(invenInstance);
            }
            inventoryInstances.Clear();

            // 새로운 인벤토리 오브젝트를 생성하고 설정
            foreach (var inven in inventoryList)
            {
                GameObject invenInstance = Instantiate(inventoryPrefab, inventorytLayout);
                invenInstance.name = "Inven" + inven["itemid"];
                inventoryInstances.Add(invenInstance);

                Text textComponent = invenInstance.GetComponentInChildren<Text>();
                if (textComponent == null)
                {
                    return;
                }
                inven.TryGetValue("itemnm", out object itemnm);
                inven.TryGetValue("itemcnt", out object itemcnt);
                inven.TryGetValue("itemdesc", out object itemdesc);
                textComponent.text = itemnm + " X " + itemcnt + "\r\n" + itemdesc;
            }
            inventory.SetActive(false);
        }

        public void OnClickInventory()
        {
            ActivateMenu(InventoryMenu); // 인벤토리 메뉴 활성화 함수 호출
        }

        public void OnClickInverntoryOut()
        {
            DeactivateMenu(InventoryMenu); // 인벤토리 메뉴 비활성화 함수 호출
        }

        private void ActivateMenu(GameObject InventoryMenu)
        {
            InventoryMenu.SetActive(true); // 메뉴 활성화
        }

        private void DeactivateMenu(GameObject InventoryMenu)
        {
            InventoryMenu.SetActive(false); // 메뉴 비활성화
        }
    }
}
