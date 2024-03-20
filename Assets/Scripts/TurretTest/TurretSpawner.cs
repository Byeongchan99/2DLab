using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        /// <summary> �ͷ� ������ ��ȯ ���� </summary>
        [SerializeField] List<int> turretSpawnLevels;
        /// <summary> �ͷ� ������ ��ȯ Ȯ�� </summary>
        [SerializeField] List<float> turretSpawnChances;
        /// <summary> �ͷ� ������ ��ȯ ��Ÿ�� </summary>
        [SerializeField] List<float> turretSpawnCooltimes;
        /// <summary> �ͷ� ������ ���� ������ ��ũ���ͺ� ������Ʈ </summary>
        [SerializeField] TurretSpawnerData[] turretSpawnerScriptableObjects;
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
        void SettingTurretSpawner(int eventIndex)
        {
            // ��ũ���ͺ� ������Ʈ���� �ʱ� �ͷ� ��ȯ ���� �� ��Ÿ�� ������ �����ͼ� ����
            SettingTurretSpawnLevel();
            SettingTurretSpawnChances();
            SettingTurretSpawnCooltimes();
        }

        void LoadTurretSpawnerSettings(TurretSpawnerData turretSpawnerData)
        {
            // �� �ͷ� �����Ϳ� ���� �ݺ�
            for (int i = 0; i < turretSpawnerData.turretDatas.Count; i++)
            {
                // ������ ��Ÿ�� ���� ������ ����
                if (i < turretSpawnLevels.Count && i < turretSpawnCooltimes.Count)
                {
                    turretSpawnLevels[i] = turretSpawnerData.turretDatas[i].spawnLevel;
                    turretSpawnCooltimes[i] *= turretSpawnerData.turretDatas[i].spawnCooldownPercent;
                }
            }
        }

        /// <summary> �ͷ� ������ ��ȯ ���� ���� </summary>
        /// <param name="levels"> ��ž ��ȯ ������ ������ �� �迭 </param>
        void SettingTurretSpawnLevel(params int[] levels)
        {
            // ��ž ��ȯ ������ ������ �� �迭�� ���̿� ��ž ��ȯ ���� �迭�� ���̰� ��ġ�ϴ��� Ȯ��
            if (levels.Length != turretSpawnLevels.Count)
            {
                Debug.LogError("��ž ��ȯ ������ ������ �� �迭�� ���̿� ��ž ��ȯ ���� �迭�� ���̰� �ٸ�");
                return;
            }

            // �ͷ� ���� ����
            for (int i = 0; i < levels.Length; i++)
            {
                turretSpawnLevels[i] += levels[i];
            }
        }

        /// <summary> �ͷ� ������ ���� Ȯ�� ���� </summary>
        /// <param name="levels"> �� �ͷ��� ���� �迭 </param>
        void SettingTurretSpawnChances(params int[] levels)
        {
            // �� �ͷ��� ���� �迭�� ���̿� �ͷ� ��ȯ Ȯ�� �迭�� ���̰� ��ġ�ϴ��� Ȯ��
            if (levels.Length != turretSpawnChances.Count)
            {
                Debug.LogError("�� �ͷ��� ���� �迭�� ���̿� �ͷ� ��ȯ Ȯ�� �迭�� ���̰� �ٸ�");
                return;
            }

            // ���� �հ� ���
            int levelSum = 0;
            for (int i = 0; i < levels.Length; i++)
            {
                levelSum += levels[i];
            }

            // ���� �հ谡 0�� ���, ��� Ȯ���� �����ϰ� �����Ͽ� ���� ����
            if (levelSum == 0)
            {
                Debug.LogError("���� �հ谡 0. ��� Ȯ�� �����ϰ� ����");
                float equalChance = 100f / levels.Length;
                for (int i = 0; i < levels.Length; i++)
                {
                    turretSpawnChances[i] = equalChance;
                }
                return;
            }

            // �� �ͷ��� ��ȯ Ȯ�� ���
            float chancePerLevel = 100f / levelSum;
            float totalPercentage = 0f; // ������ ���� �� Ȯ�� �հ� ���
            for (int i = 0; i < levels.Length; i++)
            {
                turretSpawnChances[i] = chancePerLevel * levels[i];
                totalPercentage += turretSpawnChances[i];
            }

            // Ȯ���� ������ 100%�� ������� Ȯ�� (�ε��Ҽ��� �������� ���� ���� ���� ���)
            if (Mathf.Abs(100f - totalPercentage) > 0.01f)
            {
                Debug.LogWarning($"Ȯ�� ������ 100%�� �ƴ�. ����: {totalPercentage}%");
            }
        }

        /// <summary> �ͷ� ������ ��ȯ ��Ÿ�� ���� </summary>
        /// <param name="cooltimePercents"> ��Ÿ���� ������ �ۼ�Ʈ �� �迭 </param>
        void SettingTurretSpawnCooltimes(params float[] cooltimePercents)
        {
            // ��Ÿ���� ������ �ۼ�Ʈ �� �迭�� ���̰� �ͷ� ��ȯ ��Ÿ�� �迭�� ���̿� ��ġ�ϴ��� Ȯ��
            if (cooltimePercents.Length != turretSpawnCooltimes.Count)
            {
                Debug.LogError("��Ÿ���� ������ �ۼ�Ʈ �� �迭�� ���̿� �ͷ� ��ȯ ��Ÿ�� �迭�� ���̰� �ٸ�");
                return;
            }

            // �ͷ� ��Ÿ�� ����
            for (int i = 0; i < cooltimePercents.Length; i++)
            {
                // �Էµ� ���� ���� �������� Ȯ��
                if (cooltimePercents[i] < 0)
                {
                    Debug.LogWarning($"{i}��° �ͷ� ��Ÿ�� ������ ������ �� ����");
                    continue;
                }

                // ��Ÿ�ӿ� ���� ����
                turretSpawnCooltimes[i] *= cooltimePercents[i];
            }
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
