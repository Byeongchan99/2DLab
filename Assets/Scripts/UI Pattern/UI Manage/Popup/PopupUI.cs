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
        // 클릭 시 팝업을 가장 위로 올리는 액션
        public event Action OnFocus;

        // 팝업이 열려있는지 여부를 나타내는 프로퍼티
        public bool IsOpen { get; set; } = false;

        // Focus를 사용할지 여부
        [SerializeField]
        private bool _useFocus = true;


        /// <summary> 팝업 UI를 마우스로 클릭할 때 </summary>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnFocus();
        }
    }
}
