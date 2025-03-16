using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    public class Enemy : MonoBehaviour
    {
        public void ApplySlow(float amount)
        {
            Debug.Log("Applying slow to enemy: " + amount);
        }
    }
}
