using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIManage
{
    public class PopupUIManager : MonoBehaviour
    {
        /****************************************************************************
                                         private Fields
        ****************************************************************************/
        /// <summary> 실시간 팝업 관리 링크드 리스트 </summary>
        private LinkedList<PopupUI> _popupLinkedList;

        /// <summary> UI 리스트 </summary>
        [SerializeField] private List<PopupUI> _popupList = new List<PopupUI>();

        /// <summary> Popup UI 인스턴스들을 이름으로 관리하기 위한 딕셔너리 </summary>
        private Dictionary<string, PopupUI> _popupDictionary = new Dictionary<string, PopupUI>();

        /****************************************************************************
                                         Unity Callbacks
        ****************************************************************************/
        private void Awake()
        {
            // 링크드 리스트 생성
            _popupLinkedList = new LinkedList<PopupUI>();
            // 초기화
            Init();
        }

        private void Update()
        {
            // esc 키를 누르면 팝업 닫기
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_popupLinkedList.Count > 0)
                {
                    // 첫 번째 팝업 닫기
                    ClosePopup(_popupLinkedList.First.Value);
                }
            }
            // I키를 누르면 아이템창 팝업
            else if (Input.GetKeyDown(KeyCode.I))
            {
                togglePopup(_popupDictionary["Popup Item"]);
            }
            // K키를 누르면 스킬창 팝업
            else if (Input.GetKeyDown(KeyCode.K))
            {
                togglePopup(_popupDictionary["Popup Skill"]);
            }
            // S키를 누르면 스탯창 팝업
            else if (Input.GetKeyDown(KeyCode.S))
            {
                togglePopup(_popupDictionary["Popup Stat"]);
            }
        }

        /****************************************************************************
                                         private Methods
        ****************************************************************************/
        /// <summary> 초기화 </summary>
        private void Init()
        {
            // 리스트의 PopupUI 인스턴스들을 딕셔너리에 등록 및 비활성화
            foreach (var popup in _popupList)
            {
                RegisterUI(popup.gameObject.name, popup);
                popup.gameObject.SetActive(false);

                // 헤더 Focus 이벤트 등록
                popup.OnFocus += () =>
                {
                    // 링크드 리스트에서 제거하고 새롭게 추가
                    _popupLinkedList.Remove(popup);
                    _popupLinkedList.AddFirst(popup);
                    UpdatePopupUIOrder();
                };

                // 팝업 닫기 버튼 등록
                popup.closeButton.onClick.AddListener(() => ClosePopup(popup));
            }
        }

        /// <summary> PopupUI 인스턴스들을 딕셔너리에 등록하는 메서드 </summary>
        private void RegisterUI(string name, PopupUI popup)
        {
            if (!_popupDictionary.ContainsKey(name))
            {
                _popupDictionary.Add(name, popup);
            }
        }

        /// <summary> 팝업 열기 </summary>
        private void OpenPopup(PopupUI popup)
        {
            // 링크드 리스트에 추가하고
            _popupLinkedList.AddFirst(popup);
            // 활성화
            popup.isOpen = true;
            popup.gameObject.SetActive(true);
            // 순서 업데이트
            UpdatePopupUIOrder();
        }

        /// <summary> 이름을 사용하여 팝업 열기 </summary>
        public void OpenPopup(string UIName)
        {
            if (_popupDictionary.TryGetValue(UIName, out PopupUI popup))
            {
                OpenPopup(popup);
            }
            else
            {
                Debug.LogWarning($"Popup with name {UIName} not found.");
            }
        }

        /// <summary> 팝업 닫기 </summary>
        private void ClosePopup(PopupUI popup)
        {
            // 링크드 리스트에서 제거하고
            _popupLinkedList.Remove(popup);
            // 비활성화
            popup.isOpen = false;
            popup.gameObject.SetActive(false);
            // 순서 업데이트
            UpdatePopupUIOrder();
        }

        /// <summary> 이름을 사용하여 팝업 닫기 </summary>
        public void ClosePopup(string UIName)
        {
            if (_popupDictionary.TryGetValue(UIName, out PopupUI popup))
            {
                ClosePopup(popup);
            }
            else
            {
                Debug.LogWarning($"Popup with name {UIName} not found.");
            }
        }

        /// <summary> 단축키에 따라 팝업 토글 </summary>
        private void togglePopup(PopupUI popup)
        {
            if (popup.isOpen)
            {
                ClosePopup(popup);
            }
            else
            {
                OpenPopup(popup);
            }
        }

        /// <summary> 팝업 UI들의 순서 업데이트 </summary>
        private void UpdatePopupUIOrder()
        {
            foreach (var popup in _popupLinkedList)
            {
                popup.transform.SetAsFirstSibling();
            }
        }
    }
}
