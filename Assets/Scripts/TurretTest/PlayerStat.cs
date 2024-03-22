using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    public class PlayerStat : MonoBehaviour
    {
        public static PlayerStat Instance { get; private set; } // 싱글톤 인스턴스

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
            }
            else
            {
                Destroy(gameObject); // 중복 인스턴스 제거
            }
        }

        private void Update()
        {
            // 임시로 스페이스바 입력 시 터렛 업그레이드 이벤트 발생
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("스페이스바 입력");
                TurretUpgrade bulletTurretSplit = new TurretUpgrade
                {
                    turretType = TurretUpgrade.TurretType.Bullet,
                    enhancementType = TurretUpgrade.EnhancementType.ProjectileSplit,
                };

                EventManager.TriggerEnhancementEvent("TurretUpgrade", bulletTurretSplit);
            }
        }
    }
}
