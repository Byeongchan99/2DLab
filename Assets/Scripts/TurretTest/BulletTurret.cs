using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletTurret : BaseTurret
{
    float rotationSpeed = 2f; // �ͷ��� ȸ���ϴ� �ӵ�
    public Transform rotatePoint; // ȸ����ų �ڽ� ������Ʈ

    protected override void Shoot()
    {
        RotateTurret();
        // �÷��̾��� ��ġ�� ���� ����ü ����
        Instantiate(currentProjectilePrefabs, firePoint.position, firePoint.rotation);
    }

    public override void RotateTurret()
    {
        if (targetPosition != null && rotatePoint != null)
        {
            // �÷��̾ ���� ���� ���� ���
            Vector2 direction = targetPosition.position - rotatePoint.position;
            // atan2�� ����Ͽ� �������� ���� ������ ����� ����, ��(degree)�� ��ȯ
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Z�� ȸ���� �����ϱ� ���� ���� rotatePoint�� X, Y ȸ���� �����ϰ�, Z���� angle�� ����
            Vector3 rotation = new Vector3(0, 0, angle);
            // DoTween�� ����Ͽ� rotatePoint�� Z�� ȸ���� angle�� �ε巴�� ����
            rotatePoint.DOLocalRotate(rotation, 0.5f).SetEase(Ease.OutSine);
        }
    }

    // �߻��� ����ü�� ��ü�ϴ� �޼���
    public void ChangeProjectile(int projectileIndex)
    {
        currentProjectilePrefabs = projectilePrefabs[projectileIndex];
    }
}
