[System.Serializable]
public class TurretEnhancement
{
    public enum TurretType { Bullet, Laser, Rocket, Mortar }
    public TurretType turretType;

    public enum EnhancementType
    {
        CountIncrease, SpeedIncrease, BulletSplit, RemainTimeIncrease, InductionUpgrade
    }
    public EnhancementType enhancementType;
}
