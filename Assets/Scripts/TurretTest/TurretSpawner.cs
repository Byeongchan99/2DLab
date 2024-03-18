using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    public class TurretSpawner : MonoBehaviour
    {
        /****************************************************************************
                                      private Fields
        ****************************************************************************/
        /// <summary> ��ȯ ��ġ Transform �迭 </summary>
        [SerializeField] Transform[] spawnPositions;
        /// <summary> �� ��ġ�� ��� ���� ���θ� ��Ÿ���� bool �迭 </summary>
        [SerializeField] private bool[] isAvailableSpawnPosition;
        /// <summary> ��ȯ�� �ͷ� ������ ����Ʈ </summary>
        [SerializeField] List<GameObject> turretPrefabs;
        /// <summary> �ͷ� ������ ��ȯ Ȯ�� </summary>
        [SerializeField] List<float> turretSpawnChances;
        /// <summary> �ͷ� ������ ��ȯ ��Ÿ�� </summary>
        [SerializeField] List<float> turretSpawnCooltimes;
        /// <summary> �ͷ� ������ ���� ������ ��ũ���ͺ� ������Ʈ </summary>
        // [SerializeField] TurretSpawnerData turretSpawnerData; // ���߿� �߰��ϱ�
        private float nextSpawnTime = 0f;
        private bool isSpawning = false; // ���� ��ȯ ������ ���θ� ��Ÿ���� �÷���

        /****************************************************************************
                                       Unity Callbacks
        ****************************************************************************/
        void Awake()
        {
            Init();
        }

        void Start()
        {        
            StartCoroutine(SpawnTurretRoutine());
        }

        /****************************************************************************
                                        private Methods
        ****************************************************************************/
        /// <summary> �ʱ�ȭ </summary>
        void Init()
        {
            // ��� ��ȯ ��ġ�� ��� ���� ���·� �ʱ�ȭ
            isAvailableSpawnPosition = new bool[spawnPositions.Length];
            for (int i = 0; i < isAvailableSpawnPosition.Length; i++)
            {
                isAvailableSpawnPosition[i] = true;
            }

            SettingTurretSpawner(0);
        }

        /// <summary> �ͷ� ��ȯ Ȯ�� �� ��Ÿ�� ���� </summary>
        void SettingTurretSpawner(float time)
        {
            // ��ũ���ͺ� ������Ʈ���� �ð��뿡 ���� �ͷ� ��ȯ Ȯ�� �� ��Ÿ�� ������ �����ͼ� ����
            SettingTurretSpawnChances();
            SettingTurretSpawnCooltimes();
        }

        /// <summary> �ͷ� ���� ���� Ȯ�� ���� </summary>
        void SettingTurretSpawnChances(params float[] chances)
        {
            /*
            for (int i = 0; i < chances.Length; i++)
            {
                turretSpawnChances[i] = chances[i];
            }
            */

            turretSpawnChances = new List<float>(chances);
        }

        /// <summary> �ͷ� ������ ��ȯ ��Ÿ�� ���� </summary>
        void SettingTurretSpawnCooltimes(params float[] cooltimes)
        {
            /*
            for (int i = 0; i < cooltimes.Length; i++)
            {
                turretSpawnCooltimes[i] = cooltimes[i];
            }
            */

            turretSpawnCooltimes = new List<float>(cooltimes);
        }

        /// <summary> �ͷ� ��ȯ �ڷ�ƾ </summary>
        IEnumerator SpawnTurretRoutine()
        {
            while (true) // ���� ������ ���� ���� ���� ���������� �ͷ� ��ȯ
            {
                if (!isSpawning) // ��ȯ ���� �ƴ� ���� ���ο� �ͷ� ��ȯ �õ�
                {
                    isSpawning = true; // ��ȯ ���� �÷��� ����
                    SpawnTurret();
                    // ���õ� �ͷ��� ��Ÿ�ӿ� ���� ���
                    yield return new WaitForSeconds(nextSpawnTime);
                    isSpawning = false; // ��ȯ �Ϸ� �� �÷��� �缳��
                }
                yield return null; // ���� �����ӱ��� ���
            }
        }

        /// <summary> �ͷ� ��ȯ </summary>
        void SpawnTurret()
        {
            // �ͷ� ���� ����
            GameObject turretToSpawn = ChooseTurretType();
            // �ͷ� ��ġ ����
            Transform spawnPosition = ChooseSpawnPosition();

            if (turretToSpawn != null && spawnPosition != null)
            {
                Instantiate(turretToSpawn, spawnPosition.position, Quaternion.identity);
                nextSpawnTime = ChooseCooldown(turretToSpawn); // ���� ��ȯ������ �ð� ����
            }
        }

        /// <summary> �ͷ� ���� ���� </summary>
        GameObject ChooseTurretType()
        {
            float randomChance = Random.value; // 0�� 1 ������ ������ ��
            float currentChance = 0f;
            for (int i = 0; i < turretPrefabs.Count; i++)
            {
                currentChance += turretSpawnChances[i]; // ���� Ȯ�� ������Ʈ
                if (randomChance <= currentChance)
                {
                    return turretPrefabs[i]; // ������ �����ϴ� �ͷ� ����
                }
            }
            return null;
        }

        /// <summary> ��ȯ ��ġ ���� </summary>
        Transform ChooseSpawnPosition()
        {
            List<int> availableSpawnPositions = new List<int>();

            // ��� ������ ��ġ �ε����� ����Ʈ�� �߰�
            for (int i = 0; i < isAvailableSpawnPosition.Length; i++)
            {
                if (isAvailableSpawnPosition[i])
                {
                    availableSpawnPositions.Add(i);
                }
            }

            // ��� ������ ��ġ�� ���� ��� null ��ȯ
            if (availableSpawnPositions.Count == 0) return null;

            // ��� ������ ��ġ �߿��� �����ϰ� �ϳ� ����
            int selectedIndex = availableSpawnPositions[Random.Range(0, availableSpawnPositions.Count)];
            isAvailableSpawnPosition[selectedIndex] = false; // ���õ� ��ġ�� ��� �Ұ��� ���·� ����

            return spawnPositions[selectedIndex];
        }

        /// <summary> ��ȯ�� �ͷ��� ��Ÿ�� ��ȯ </summary>
        float ChooseCooldown(GameObject turret)
        {
            int index = turretPrefabs.IndexOf(turret);
            return turretSpawnCooltimes[index];
        }

        /****************************************************************************
                                      public Methods
        ****************************************************************************/
        /// <summary> ��ȯ ��ġ �ݳ� </summary>
        /// �ͷ��� ��Ȱ��ȭ�� �� ȣ���Ͽ� �ش� ��ġ�� �ٽ� ��� ���� ���·� ����
        public void SetPositionAvailable(int positionIndex)
        {
            if (positionIndex >= 0 && positionIndex < isAvailableSpawnPosition.Length)
            {
                isAvailableSpawnPosition[positionIndex] = true;
            }
        }
    }
}
