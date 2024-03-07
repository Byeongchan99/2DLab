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

    /// <summary> �˾� UI�� ���콺�� Ŭ���� �� </summary>
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnFocus();
    }
}
