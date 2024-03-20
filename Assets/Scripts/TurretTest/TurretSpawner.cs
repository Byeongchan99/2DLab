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
        /// <summary> 소환 위치 Transform 배열 </summary>
        [SerializeField] Transform[] spawnPositions;
        /// <summary> 각 위치의 사용 가능 여부를 나타내는 bool 배열 </summary>
        [SerializeField] private bool[] isAvailableSpawnPosition;
        /// <summary> 소환할 터렛 프리팹 리스트 </summary>
        [SerializeField] List<GameObject> turretPrefabs;
        /// <summary> 터렛 종류별 소환 레벨 </summary>
        [SerializeField] List<int> turretSpawnLevels;
        /// <summary> 터렛 종류별 소환 확률 </summary>
        [SerializeField] List<float> turretSpawnChances;
        /// <summary> 터렛 종류별 소환 쿨타임 </summary>
        [SerializeField] List<float> turretSpawnCooltimes;

        /// <summary> 스크립터블 오브젝트에서 로드한 터렛 소환 데이터 </summary>
        [SerializeField]  TurretSpawnerData currentEventData;
        [SerializeField]  List<int> eventSpawnLevels = new List<int>();
        [SerializeField]  List<float> eventSpawnCooltimePercents = new List<float>();

        private float nextSpawnTime = 0f;
        private bool isSpawning = false; // 현재 소환 중인지 여부를 나타내는 플래그

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
        /// <summary> 초기화 </summary>
        void Init()
        {
            // 모든 소환 위치를 사용 가능 상태로 초기화
            isAvailableSpawnPosition = new bool[spawnPositions.Length];
            for (int i = 0; i < isAvailableSpawnPosition.Length; i++)
            {
                isAvailableSpawnPosition[i] = true;
            }

            // 이벤트 이름에 따른 스크립터블 오브젝트 데이터 로드
            currentEventData = TurretSpawnerDataFactory.Instance.GetSpawnerDataForEvent("Init");
            // 터렛 종류별 데이터 초기화
            SettingTurretSpawnerDatas(currentEventData);
        }

        /// <summary> 스크립터블 오브젝트에서 터렛 소환 데이터 적용 </summary>
        void SettingTurretSpawnerDatas(TurretSpawnerData turretSpawnerData)
        {
            eventSpawnLevels.Clear();
            eventSpawnCooltimePercents.Clear();

            // 터렛 종류별 데이터를 리스트에 추가
            for (int i = 0; i < turretSpawnerData.turretDatas.Count; i++)
            {
                eventSpawnLevels.Add(turretSpawnerData.turretDatas[i].spawnLevel);
                eventSpawnCooltimePercents.Add(turretSpawnerData.turretDatas[i].spawnCooldownPercent);
            }

            // 터렛 종류별 데이터 적용
            // 터렛 종류별 소환 레벨 적용
            SettingTurretSpawnLevel(eventSpawnLevels.ToArray());
            // 적용된 터렛 종류별 소환 레벨로 터렛 종류별 소환 확률 적용
            SettingTurretSpawnChances(turretSpawnLevels.ToArray());
            // 터렛 종류별 소환 쿨타임 적용
            SettingTurretSpawnCooltimes(eventSpawnCooltimePercents.ToArray());
        }

        /// <summary> 터렛 종류별 소환 레벨 변경 </summary>
        /// <param name="levels"> 포탑 소환 레벨을 조정할 값 배열 </param>
        void SettingTurretSpawnLevel(params int[] levels)
        {
            // 포탑 소환 레벨을 조정할 값 배열의 길이와 포탑 소환 레벨 배열의 길이가 일치하는지 확인
            if (levels.Length != turretSpawnLevels.Count)
            {
                Debug.LogError("포탑 소환 레벨을 조정할 값 배열의 길이와 포탑 소환 레벨 배열의 길이가 다름" + levels.Length + " " + turretSpawnLevels.Count);
                return;
            }

            // 터렛 레벨 변경
            for (int i = 0; i < levels.Length; i++)
            {
                turretSpawnLevels[i] += levels[i];
            }
        }

        /// <summary> 터렛 종류별 선택 확률 변경 </summary>
        /// <param name="levels"> 각 터렛의 레벨 배열 </param>
        void SettingTurretSpawnChances(params int[] levels)
        {
            // 각 터렛의 레벨 배열의 길이와 터렛 소환 확률 배열의 길이가 일치하는지 확인
            if (levels.Length != turretSpawnChances.Count)
            {
                Debug.LogError("각 터렛의 레벨 배열의 길이와 터렛 소환 확률 배열의 길이가 다름");
                return;
            }

            // 레벨 합계 계산
            int levelSum = 0;
            for (int i = 0; i < levels.Length; i++)
            {
                levelSum += levels[i];
            }

            // 레벨 합계가 0인 경우, 모든 확률을 동일하게 설정하여 에러 방지
            if (levelSum == 0)
            {
                Debug.LogError("레벨 합계가 0. 모든 확률 동일하게 설정");
                float equalChance = 100f / levels.Length;
                for (int i = 0; i < levels.Length; i++)
                {
                    turretSpawnChances[i] = equalChance;
                }
                return;
            }

            // 각 터렛의 소환 확률 계산
            float chancePerLevel = 100f / levelSum;
            float totalPercentage = 0f; // 검증을 위한 총 확률 합계 계산
            for (int i = 0; i < levels.Length; i++)
            {
                turretSpawnChances[i] = chancePerLevel * levels[i];
                totalPercentage += turretSpawnChances[i];
            }

            // 확률의 총합이 100%에 가까운지 확인 (부동소수점 연산으로 인한 작은 오차 허용)
            if (Mathf.Abs(100f - totalPercentage) > 0.01f)
            {
                Debug.LogWarning($"확률 총합이 100%가 아님. 총합: {totalPercentage}%");
            }
        }

        /// <summary> 터렛 종류별 소환 쿨타임 변경 </summary>
        /// <param name="cooltimePercents"> 쿨타임을 조정할 퍼센트 값 배열 </param>
        void SettingTurretSpawnCooltimes(params float[] cooltimePercents)
        {
            // 쿨타임을 조정할 퍼센트 값 배열의 길이가 터렛 소환 쿨타임 배열의 길이와 일치하는지 확인
            if (cooltimePercents.Length != turretSpawnCooltimes.Count)
            {
                Debug.LogError("쿨타임을 조정할 퍼센트 값 배열의 길이와 터렛 소환 쿨타임 배열의 길이가 다름");
                return;
            }

            // 터렛 쿨타임 변경
            for (int i = 0; i < cooltimePercents.Length; i++)
            {
                // 입력된 비율 값이 음수인지 확인
                if (cooltimePercents[i] < 0)
                {
                    Debug.LogWarning($"{i}번째 터렛 쿨타임 비율이 음수라서 값 무시");
                    continue;
                }

                // 쿨타임에 비율 적용
                turretSpawnCooltimes[i] *= cooltimePercents[i];
            }
        }

        /// <summary> 터렛 소환 코루틴 </summary>
        IEnumerator SpawnTurretRoutine()
        {
            while (true) // 무한 루프를 통해 게임 동안 지속적으로 터렛 소환
            {
                if (!isSpawning) // 소환 중이 아닐 때만 새로운 터렛 소환 시도
                {
                    isSpawning = true; // 소환 시작 플래그 설정
                    SpawnTurret();
                    // 선택된 터렛의 쿨타임에 따라 대기
                    yield return new WaitForSeconds(nextSpawnTime);
                    isSpawning = false; // 소환 완료 후 플래그 재설정
                }
                yield return null; // 다음 프레임까지 대기
            }
        }

        /// <summary> 터렛 소환 </summary>
        void SpawnTurret()
        {
            // 터렛 종류 선택
            GameObject turretToSpawn = ChooseTurretType();
            // 터렛 위치 선택
            Transform spawnPosition = ChooseSpawnPosition();

            if (turretToSpawn != null && spawnPosition != null)
            {
                Instantiate(turretToSpawn, spawnPosition.position, Quaternion.identity);
                nextSpawnTime = ChooseCooldown(turretToSpawn); // 다음 소환까지의 시간 설정
            }
        }

        /// <summary> 터렛 종류 선택 </summary>
        GameObject ChooseTurretType()
        {
            float randomChance = Random.value; // 0과 1 사이의 랜덤한 값
            float currentChance = 0f;

            for (int i = 0; i < turretPrefabs.Count; i++)
            {
                currentChance += turretSpawnChances[i]; // 누적 확률 업데이트
                if (randomChance <= currentChance)
                {
                    return turretPrefabs[i]; // 조건을 만족하는 터렛 선택
                }
            }
            return null;
        }

        /// <summary> 소환 위치 선택 </summary>
        Transform ChooseSpawnPosition()
        {
            List<int> availableSpawnPositions = new List<int>();

            // 사용 가능한 위치 인덱스를 리스트에 추가
            for (int i = 0; i < isAvailableSpawnPosition.Length; i++)
            {
                if (isAvailableSpawnPosition[i])
                {
                    availableSpawnPositions.Add(i);
                }
            }

            // 사용 가능한 위치가 없는 경우 null 반환
            if (availableSpawnPositions.Count == 0) return null;

            // 사용 가능한 위치 중에서 랜덤하게 하나 선택
            int selectedIndex = availableSpawnPositions[Random.Range(0, availableSpawnPositions.Count)];
            isAvailableSpawnPosition[selectedIndex] = false; // 선택된 위치를 사용 불가능 상태로 변경

            return spawnPositions[selectedIndex];
        }

        /// <summary> 소환한 터렛의 쿨타임 반환 </summary>
        float ChooseCooldown(GameObject turret)
        {
            int index = turretPrefabs.IndexOf(turret);
            return turretSpawnCooltimes[index];
        }

        /****************************************************************************
                                      public Methods
        ****************************************************************************/
        /// <summary> 소환 위치 반납 </summary>
        /// 터렛이 비활성화될 때 호출하여 해당 위치를 다시 사용 가능 상태로 변경
        public void SetPositionAvailable(int positionIndex)
        {
            if (positionIndex >= 0 && positionIndex < isAvailableSpawnPosition.Length)
            {
                isAvailableSpawnPosition[positionIndex] = true;
            }
        }
    }
}
