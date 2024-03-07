using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIManage
{
    public class PopupUIHeader : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField] 
        private RectTransform _moveArea; // 드래그하여 이동할 팝업 UI
        private PopupUI _parentPopupUI; // 부모 PopupUI 컴포넌트 참조

        private Vector2 _startingDragPoint; // 드래그 시작 지점
        private Vector2 _dragBeginMousePoint; // 드래그를 시작할 때의 마우스 위치
        private Vector2 _dragMouseOffset; // 드래그 동안 마우스가 움직인 상대적 위치

        private void Awake()
        {
            // 부모 컴포넌트에서 PopupUI 컴포넌트를 찾아 참조 저장
            _parentPopupUI = GetComponentInParent<PopupUI>();
        }

        /// <summary> 드래그 시작 </summary>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            // 부모 PopupUI의 OnFocus 이벤트를 간접적으로 호출
            _parentPopupUI.TriggerOnFocus();

            // 현재 UI 요소의 위치 저장
            _startingDragPoint = _moveArea.position;
            // 클릭 지점의 위치 저장
            _dragBeginMousePoint = eventData.position;
        }

        /// <summary> 드래그 중 </summary>
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            // 현재 마우스의 위치에서 _dragBeginMousePoint을 뺀 결과(_dragMouseOffset)를 계산
            _dragMouseOffset = eventData.position - _dragBeginMousePoint;
            // UI 요소 위치 업데이트
            _moveArea.position = _startingDragPoint + _dragMouseOffset;
        }
    }
}
