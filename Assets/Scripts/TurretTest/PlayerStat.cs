using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
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
            // �ӽ÷� �����̽��� �Է� �� �ͷ� ���׷��̵� �̺�Ʈ �߻�
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("�����̽��� �Է�");
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
