using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TurretTest
{
    [System.Serializable]
    public class TurretUpgradeEvent : UnityEvent<TurretUpgrade> { }

    /// <summary> 이벤트 버스 패턴으로 구현한 이벤트 매니저 </summary>
    public class EventManager : MonoBehaviour
    {
        // 싱글톤 인스턴스
        public static EventManager Instance;
        // 이벤트 딕셔너리
        private Dictionary<string, TurretUpgradeEvent> enhancementEventDictionary;

        void Awake()
        {
            // 싱글톤 인스턴스 설정
            if (Instance == null)
            {
                Instance = this;
                enhancementEventDictionary = new Dictionary<string, TurretUpgradeEvent>();
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        /// <summary> 이벤트 리스너 추가 </summary>
        public static void StartListening(string eventName, UnityAction<TurretUpgrade> listener)
        {
            TurretUpgradeEvent thisEvent = null;
            if (Instance.enhancementEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new TurretUpgradeEvent();
                thisEvent.AddListener(listener);
                Instance.enhancementEventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary> 이벤트 리스너 제거 </summary>
        public static void StopListening(string eventName, UnityAction<TurretUpgrade> listener)
        {
            if (Instance == null) return;
            TurretUpgradeEvent thisEvent = null;
            if (Instance.enhancementEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        /// <summary> 이벤트 트리거 </summary>
        public static void TriggerEnhancementEvent(string eventName, TurretUpgrade enhancementData)
        {
            TurretUpgradeEvent thisEvent = null;
            if (Instance.enhancementEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(enhancementData);
            }
        }
    }
}
