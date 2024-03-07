using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour, IPointerDownHandler
{
    public Button closeButton;
    public event Action OnFocus;

    /// <summary> 팝업 UI를 마우스로 클릭할 때 </summary>
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnFocus();
    }
}
