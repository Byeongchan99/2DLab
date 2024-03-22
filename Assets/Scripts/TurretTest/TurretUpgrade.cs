namespace TurretTest
{
    [System.Serializable]
    public class TurretUpgrade
    {
        public enum TurretType { Bullet, Laser, Rocket, Mortar } // �ͷ� ���� - �Ѿ�, ������, ����, �ڰ���
        public TurretType turretType;

        public enum EnhancementType // �ͷ� ��ȭ ����
        {
            // �߻�ü ���� ����, �߻�ü �ӵ� ����, �߻�ü �п�, �߻�ü ���ӽð� ����(������ �ͷ� ����), ���� ���� ����(�ڰ��� �ͷ� ����)
            CountIncrease, SpeedIncrease, ProjectileSplit, RemainTimeIncrease, InductionUpgrade 
        }
        public EnhancementType enhancementType;
    }
}
