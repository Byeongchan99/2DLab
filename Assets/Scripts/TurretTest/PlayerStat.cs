using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("스페이스바 입력");
            TurretEnhancement bulletTurretSplit = new TurretEnhancement
            {
                turretType = TurretEnhancement.TurretType.Bullet,
                enhancementType = TurretEnhancement.EnhancementType.BulletSplit,
            };

            EventManager.TriggerEnhancementEvent("TurretUpgrade", bulletTurretSplit);
        }
    }
}
