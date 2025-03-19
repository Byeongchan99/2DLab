using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    public class TowerProjectile : MonoBehaviour
    {
        public float speed;
        [SerializeField] private float _damage;
        [SerializeField] private GameObject _target;

        public void Init(GameObject target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        private void Update()
        {
            if (_target != null)
            {
                Vector3 direction = _target.transform.position - transform.position;
                float distanceThisFrame = speed * Time.deltaTime;
             
                transform.Translate(direction.normalized * distanceThisFrame, Space.World);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == _target)
            {
                _target.GetComponent<Enemy>().TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
