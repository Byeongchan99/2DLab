using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : MonoBehaviour
{
    float currentHealth;
    float maxHealth;
    float speed;

    public virtual void Move()
    {
        // Move to the target
    }
}
