using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    public class Bullet : BaseProjectile
    {
        protected override void Move()
        {
            // �߻�ü Ư���� ������ ����
            rb.velocity = transform.right * speed;
        }
    }
}
