using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TurretEnhancementEvent : UnityEvent<TurretEnhancement> { }

/// <summary> �̺�Ʈ ���� �������� ������ �̺�Ʈ �Ŵ��� </summary>
public class EventManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static EventManager Instance;
    // �̺�Ʈ ��ųʸ�
    private Dictionary<string, TurretEnhancementEvent> enhancementEventDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            enhancementEventDictionary = new Dictionary<string, TurretEnhancementEvent>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // �̺�Ʈ ������ �߰�
    public static void StartListening(string eventName, UnityAction<TurretEnhancement> listener)
    {
        TurretEnhancementEvent thisEvent = null;
        if (Instance.enhancementEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new TurretEnhancementEvent();
            thisEvent.AddListener(listener);
            Instance.enhancementEventDictionary.Add(eventName, thisEvent);
        }
    }

    // �̺�Ʈ ������ ����
    public static void StopListening(string eventName, UnityAction<TurretEnhancement> listener)
    {
        if (Instance == null) return;
        TurretEnhancementEvent thisEvent = null;
        if (Instance.enhancementEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    // �̺�Ʈ Ʈ����
    public static void TriggerEnhancementEvent(string eventName, TurretEnhancement enhancementData)
    {
        TurretEnhancementEvent thisEvent = null;
        if (Instance.enhancementEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(enhancementData);
        }
    }
}
