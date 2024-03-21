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
        /// <summary> �ͷ� ������ �ʱ� ��ȯ ��Ÿ�� </summary>
        [SerializeField] List<float> turretInitialSpawnCooltimes;
        /// <summary> �ͷ� ������ ��ȯ ��Ÿ�� </summary>
        [SerializeField] List<float> turretSpawnCooltimes;

        /// <summary> �ͷ����� ���� �θ� ������Ʈ </summary>
        [SerializeField]  GameObject turretsParent;
        /// <summary> ��ũ���ͺ� ������Ʈ���� �ε��� �ͷ� ��ȯ ������ </summary>
        [SerializeField]  TurretSpawnerData currentEventData;
        [SerializeField]  List<int> eventSpawnLevels = new List<int>();
        [SerializeField]  List<float> eventSpawnCooltimePercents = new List<float>();

        private float nextSpawnTime = 0f;
        private bool isSpawning = false; // ���� ��ȯ ������ ���θ� ��Ÿ���� �÷���

        private bool isBulletSplitActive = false; // �п� �Ѿ� Ȱ��ȭ ����

        /****************************************************************************
                                       Unity Callbacks
        ****************************************************************************/
        void Awake()
        {
            Init();
        }

        void OnEnable()
        {
            EventManager.StartListening("TurretUpgrade", HandleTurretUpgradeEvent);
        }

        void Start()
        {        
            StartCoroutine(SpawnTurretRoutine());
        }

        void OnDisable()
        {
            EventManager.StopListening("TurretUpgrade", HandleTurretUpgradeEvent);
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

            // �̺�Ʈ �̸��� ���� ��ũ���ͺ� ������Ʈ ������ �ε�
            currentEventData = TurretSpawnerDataFactory.Instance.GetSpawnerDataForEvent("Init");

            // �ͷ� ������ �ʱ� ��ȯ ��Ÿ�� ����
            for (int i = 0; i < turretInitialSpawnCooltimes.Count; i++)
            {
                turretSpawnCooltimes[i] = turretInitialSpawnCooltimes[i];
            }

            // �ͷ� ������ ������ �ʱ�ȭ
            SettingTurretSpawnerDatas(currentEventData);
        }

        /// <summary> �̺�Ʈ ���� </summary>
        private void HandleTurretUpgradeEvent(TurretEnhancement enhancement)
        {
            switch (enhancement.turretType)
            {
                case TurretEnhancement.TurretType.Bullet:
                    // Bullet Turret�� ���׷��̵� ó���� ���� ���� switch ��
                    switch (enhancement.enhancementType)
                    {
                        case TurretEnhancement.EnhancementType.BulletSplit:
                            // Bullet Turret �п� �Ѿ� ���׷��̵� ó��
                            isBulletSplitActive = true;
                            break;
                        case TurretEnhancement.EnhancementType.CountIncrease:
                            // ���� ���� ó��
                            break;
                        case TurretEnhancement.EnhancementType.SpeedIncrease:
                            // �ӵ� ���� ó��
                            break;
                            // ��Ÿ �ʿ��� ��� �߰�
                    }
                    break;
                case TurretEnhancement.TurretType.Laser:
                    // Laser Turret�� ���׷��̵� ó��
                    switch (enhancement.enhancementType)
                    {
                        case TurretEnhancement.EnhancementType.RemainTimeIncrease:
                            // Bullet Turret �п� �Ѿ� ���׷��̵� ó��
                            isBulletSplitActive = true;
                            break;
                        case TurretEnhancement.EnhancementType.CountIncrease:
                            // ���� ���� ó��
                            break;
                        case TurretEnhancement.EnhancementType.SpeedIncrease:
                            // �ӵ� ���� ó��
                            break;
                            // ��Ÿ �ʿ��� ��� �߰�
                    }
                    break;
                case TurretEnhancement.TurretType.Rocket:
                    // Rocket Turret�� ���׷��̵� ó��
                    switch (enhancement.enhancementType)
                    {
                        case TurretEnhancement.EnhancementType.InductionUpgrade:
                            // Bullet Turret �п� �Ѿ� ���׷��̵� ó��
                            isBulletSplitActive = true;
                            break;
                        case TurretEnhancement.EnhancementType.CountIncrease:
                            // ���� ���� ó��
                            break;
                        case TurretEnhancement.EnhancementType.SpeedIncrease:
                            // �ӵ� ���� ó��
                            break;
                            // ��Ÿ �ʿ��� ��� �߰�
                    }
                    break;
                case TurretEnhancement.TurretType.Mortar:
                    // Mortar Turret�� ���׷��̵� ó��
                    switch (enhancement.enhancementType)
                    {
                        case TurretEnhancement.EnhancementType.BulletSplit:
                            // Bullet Turret �п� �Ѿ� ���׷��̵� ó��
                            isBulletSplitActive = true;
                            break;
                        case TurretEnhancement.EnhancementType.CountIncrease:
                            // ���� ���� ó��
                            break;
                        case TurretEnhancement.EnhancementType.SpeedIncrease:
                            // �ӵ� ���� ó��
                            break;
                            // ��Ÿ �ʿ��� ��� �߰�
                    }
                    break;
                    // ��Ÿ �ͷ� ������ ���� ó��
            }
        }

        /// <summary> �̺�Ʈ�� ���� �ͷ� ��ȯ ������ ���� </summary>
        /// <param name="eventName"> ������ �̺�Ʈ �̸� </param>
        void AdjustEvent(string eventName)
        {
            // �̺�Ʈ �̸��� ���� ��ũ���ͺ� ������Ʈ ������ �ε�
            currentEventData = TurretSpawnerDataFactory.Instance.GetSpawnerDataForEvent(eventName);
            SettingTurretSpawnerDatas(currentEventData);
        }

        /// <summary> ��ũ���ͺ� ������Ʈ���� �ͷ� ��ȯ ������ ���� </summary>
        void SettingTurretSpawnerDatas(TurretSpawnerData turretSpawnerData)
        {
            eventSpawnLevels.Clear();
            eventSpawnCooltimePercents.Clear();

            // �ͷ� ������ �����͸� ����Ʈ�� �߰�
            for (int i = 0; i < turretSpawnerData.turretDatas.Count; i++)
            {
                eventSpawnLevels.Add(turretSpawnerData.turretDatas[i].spawnLevel);
                eventSpawnCooltimePercents.Add(turretSpawnerData.turretDatas[i].spawnCooldownPercent);
            }

            // �ͷ� ������ ������ ����
            // �ͷ� ������ ��ȯ ���� ����
            SettingTurretSpawnLevel(eventSpawnLevels.ToArray());
            // ����� �ͷ� ������ ��ȯ ������ �ͷ� ������ ��ȯ Ȯ�� ����
            SettingTurretSpawnChances(turretSpawnLevels.ToArray());
            // �ͷ� ������ ��ȯ ��Ÿ�� ����
            SettingTurretSpawnCooltimes(eventSpawnCooltimePercents.ToArray());
        }

        /// <summary> �ͷ� ������ ��ȯ ���� ���� </summary>
        /// <param name="levels"> ��ž ��ȯ ������ ������ �� �迭 </param>
        void SettingTurretSpawnLevel(params int[] levels)
        {
            // ��ž ��ȯ ������ ������ �� �迭�� ���̿� ��ž ��ȯ ���� �迭�� ���̰� ��ġ�ϴ��� Ȯ��
            if (levels.Length != turretSpawnLevels.Count)
            {
                Debug.LogError("��ž ��ȯ ������ ������ �� �迭�� ���̿� ��ž ��ȯ ���� �迭�� ���̰� �ٸ�" + levels.Length + " " + turretSpawnLevels.Count);
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
                    Debug.LogWarning($"{i}��° �ͷ� ��Ÿ�� ������ �����̸� ��Ÿ�� ����");
                }
                // �ʱ� ��Ÿ���� ������ŭ ��Ÿ�� ����
                float value = turretInitialSpawnCooltimes[i] * cooltimePercents[i];

                // ��Ÿ�ӿ� ���� ����
                turretSpawnCooltimes[i] -= value;
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
            int spawnPositionIndex = ChooseSpawnPosition();
            if (spawnPositionIndex == -1) return; // ��� ������ ��ġ�� ���� ��� ��ȯ���� ����

            Transform spawnPosition = spawnPositions[spawnPositionIndex];

            if (turretToSpawn != null)
            {
                // �ͷ� ��ȯ
                GameObject spawnedTurret = Instantiate(turretToSpawn, spawnPosition.position, Quaternion.identity, turretsParent.transform);
                // ��ȯ�� �ͷ����� BaseTurret ������Ʈ ã��
                BaseTurret turretComponent = spawnedTurret.GetComponent<BaseTurret>();
                if (turretComponent != null)
                {
                    // spawnPoint ����
                    turretComponent.spawnPointIndex = spawnPositionIndex;
                    turretComponent.spawner = this;

                    // �п� �Ѿ� �̺�Ʈ�� Ȱ��ȭ�� ���
                    if (isBulletSplitActive && turretComponent is BulletTurret bulletTurret)
                    {
                        Debug.Log("�п� �Ѿ� ����");
                        bulletTurret.ChangeProjectile(1); // �п� �Ѿ� ������ ����
                    }
                }
                else
                {
                    Debug.LogError("�ͷ��� BaseTurret ������Ʈ�� ����");
                }
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
        int ChooseSpawnPosition()
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
            if (availableSpawnPositions.Count == 0) return -1;

            // ��� ������ ��ġ �߿��� �����ϰ� �ϳ� ����
            int selectedIndex = availableSpawnPositions[Random.Range(0, availableSpawnPositions.Count)];
            // ���õ� ��ġ�� ��� �Ұ��� ���·� ����
            isAvailableSpawnPosition[selectedIndex] = false;

            return selectedIndex;
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
