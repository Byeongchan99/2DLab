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
            base.Move(); // 기본 움직임도 필요하면 호출
        }
    }
}
