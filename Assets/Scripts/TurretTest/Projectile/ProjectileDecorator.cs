using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDecorator : BaseProjectile
{
    public float splitTime = 2.0f;
    public GameObject projectilePrefab;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(SplitProjectile());
    }

    IEnumerator SplitProjectile()
    {
        yield return new WaitForSeconds(splitTime);

        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, 45));
        Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, -45));

        DestroyProjectile();
    }
}
