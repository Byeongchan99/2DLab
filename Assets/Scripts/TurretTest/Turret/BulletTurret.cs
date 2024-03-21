using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletTurret : BaseTurret
{
    //float rotationSpeed = 2f; // �ͷ��� ȸ���ϴ� �ӵ�
    public Transform rotatePoint; // ȸ����ų �ڽ� ������Ʈ

    protected override void Shoot()
    {
        RotateTurret();
    }

    public override void RotateTurret()
    {
        if (targetPosition != null && rotatePoint != null)
        {
            // �÷��̾ ���� ���� ���� ���
            Vector2 direction = targetPosition.position - rotatePoint.position;
            // atan2�� ����Ͽ� �������� ���� ������ ����� ����, ��(degree)�� ��ȯ
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Z�� ȸ��
            Vector3 rotation = new Vector3(0, 0, angle);
            // ȸ�� �ִϸ��̼��� �Ϸ�� �� ShootProjectile �޼��� ȣ��
            rotatePoint.DOLocalRotate(rotation, 0.5f).SetEase(Ease.OutSine).OnComplete(ShootProjectile);
        }
    }

    // ȸ���� �Ϸ�� �� ȣ��
    private void ShootProjectile()
    {
        // �÷��̾��� ��ġ�� ���� ����ü ����
        Instantiate(currentProjectilePrefabs, firePoint.position, firePoint.rotation);
    }
}
