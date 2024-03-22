namespace TurretTest
{
    [System.Serializable]
    public class TurretUpgrade
    {
        public enum TurretType { Bullet, Laser, Rocket, Mortar } // 터렛 종류 - 총알, 레이저, 로켓, 박격포
        public TurretType turretType;

        public enum EnhancementType // 터렛 강화 종류
        {
            // 발사체 개수 증가, 발사체 속도 증가, 발사체 분열, 발사체 지속시간 증가(레이저 터렛 전용), 유도 성능 증가(박격포 터렛 전용)
            CountIncrease, SpeedIncrease, ProjectileSplit, RemainTimeIncrease, InductionUpgrade 
        }
        public EnhancementType enhancementType;
    }
}
