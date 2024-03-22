using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    public class Bullet : BaseProjectile
    {
        protected override void Move()
        {
            // 발사체 특유의 움직임 로직
            rb.velocity = transform.right * speed;
        }
    }
}
