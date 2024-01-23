using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
    const float kArea = 25f;

    private Vector2 m_Direction;

    private void RandomizeDirection()
    {
        float angle = Random.Range(-.5f, .5f);
        Vector2 toCenter = -(new Vector2(transform.position.x, transform.position.y).normalized);
        float cosAngle = Mathf.Cos(angle);
        float sinAngle = Mathf.Sin(angle);
        m_Direction = new Vector2(toCenter.x * cosAngle - toCenter.y * sinAngle, toCenter.x * sinAngle + toCenter.y * cosAngle);
    }

    public void Start()
    {
        RandomizeDirection();
    }

    public void Update()
    {
        transform.position += new Vector3(m_Direction.x, m_Direction.y, 0f) * Time.deltaTime * 4f;

        if (new Vector2(transform.position.x, transform.position.y).magnitude > kArea)
        {
            Vector2 normalizedPosition = new Vector2(transform.position.x, transform.position.y).normalized;
            //transform.position = new Vector3(normalizedPosition.x, normalizedPosition.y, transform.position.z);
            RandomizeDirection();
        }
    }
}
