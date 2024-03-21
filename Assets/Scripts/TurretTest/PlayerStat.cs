using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance { get; private set; } // �̱��� �ν��Ͻ�

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ� ����
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("�����̽��� �Է�");
            TurretEnhancement bulletTurretSplit = new TurretEnhancement
            {
                turretType = TurretEnhancement.TurretType.Bullet,
                enhancementType = TurretEnhancement.EnhancementType.BulletSplit,
            };

            EventManager.TriggerEnhancementEvent("TurretUpgrade", bulletTurretSplit);
        }
    }
}
