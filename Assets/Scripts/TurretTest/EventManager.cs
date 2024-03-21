using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TurretEnhancementEvent : UnityEvent<TurretEnhancement> { }

/// <summary> 이벤트 버스 패턴으로 구현한 이벤트 매니저 </summary>
public class EventManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static EventManager Instance;
    // 이벤트 딕셔너리
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

    // 이벤트 리스너 추가
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

    // 이벤트 리스너 제거
    public static void StopListening(string eventName, UnityAction<TurretEnhancement> listener)
    {
        if (Instance == null) return;
        TurretEnhancementEvent thisEvent = null;
        if (Instance.enhancementEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    // 이벤트 트리거
    public static void TriggerEnhancementEvent(string eventName, TurretEnhancement enhancementData)
    {
        TurretEnhancementEvent thisEvent = null;
        if (Instance.enhancementEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(enhancementData);
        }
    }
}
