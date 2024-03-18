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
        /// <summary> 소환 위치 Transform 배열 </summary>
        [SerializeField] Transform[] spawnPositions;
        /// <summary> 각 위치의 사용 가능 여부를 나타내는 bool 배열 </summary>
        [SerializeField] private bool[] isAvailableSpawnPosition;
        /// <summary> 소환할 터렛 프리팹 리스트 </summary>
        [SerializeField] List<GameObject> turretPrefabs;
        /// <summary> 터렛 종류별 소환 확률 </summary>
        [SerializeField] List<float> turretSpawnChances;
        /// <summary> 터렛 종류별 소환 쿨타임 </summary>
        [SerializeField] List<float> turretSpawnCooltimes;
        /// <summary> 터렛 스포너 관련 데이터 스크립터블 오브젝트 </summary>
        // [SerializeField] TurretSpawnerData turretSpawnerData; // 나중에 추가하기
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

            SettingTurretSpawner(0);
        }

        /// <summary> 터렛 소환 확률 및 쿨타임 설정 </summary>
        void SettingTurretSpawner(float time)
        {
            // 스크립터블 오브젝트에서 시간대에 따른 터렛 소환 확률 및 쿨타임 정보를 가져와서 설정
            SettingTurretSpawnChances();
            SettingTurretSpawnCooltimes();
        }

        /// <summary> 터렛 종류 선택 확률 변경 </summary>
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

        /// <summary> 터렛 종류별 소환 쿨타임 변경 </summary>
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
