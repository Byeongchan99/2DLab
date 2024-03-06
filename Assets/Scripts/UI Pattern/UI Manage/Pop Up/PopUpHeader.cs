using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIManage
{
    public class PopUpHeader : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        /****************************************************************************
                                     private Fields
        ****************************************************************************/
        [SerializeField] 
        private RectTransform _moveArea; // �巡���Ͽ� �̵��� �˾� UI

        private Vector2 _startingDragPoint; // �巡�� ���� ����
        private Vector2 _dragBeginMousePoint; // �巡�׸� ������ ���� ���콺 ��ġ
        private Vector2 _dragMouseOffset; // �巡�� ���� ���콺�� ������ ����� ��ġ

        /****************************************************************************
                                     public Methods
        ****************************************************************************/
        /// <summary> �巡�� ���� </summary>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            // ���� UI ����� ��ġ ����
            _startingDragPoint = _moveArea.position;
            // Ŭ�� ������ ��ġ ����
            _dragBeginMousePoint = eventData.position;
        }

        /// <summary> �巡�� �� </summary>
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            // ���� ���콺�� ��ġ���� _dragBeginMousePoint�� �� ���(_dragMouseOffset)�� ���
            _dragMouseOffset = eventData.position - _dragBeginMousePoint;
            // UI ��� ��ġ ������Ʈ
            _moveArea.position = _startingDragPoint + _dragMouseOffset;
        }
    }
}
