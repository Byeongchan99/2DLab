using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIManage
{
    public class PopupUI : MonoBehaviour, IPointerDownHandler
    {
        public Button closeButton;
        // Ŭ�� �� �˾��� ���� ���� �ø��� �׼�
        public event Action OnFocus;

        // �˾��� �����ִ��� ���θ� ��Ÿ���� ������Ƽ
        public bool IsOpen { get; set; } = false;

        // Focus�� ������� ����
        [SerializeField]
        private bool _useFocus = true;


        /// <summary> �˾� UI�� ���콺�� Ŭ���� �� </summary>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnFocus();
        }
    }
}
