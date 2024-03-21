[System.Serializable]
public class TurretUpgrade
{
    public enum TurretType { Bullet, Laser, Rocket, Mortar }
    public TurretType turretType;

    public enum EnhancementType
    {
        CountIncrease, SpeedIncrease, ProjectileSplit, RemainTimeIncrease, InductionUpgrade
    }
    public EnhancementType enhancementType;
}
