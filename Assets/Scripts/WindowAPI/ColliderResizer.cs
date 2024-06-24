using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColliderResizer : MonoBehaviour
{
    private RectTransform rectTransform;
    public EdgeCollider2D edgeCollider;
    public Text text;

    [SerializeField] WindowGame windowGame;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        ResizeCollider();
    }

    void Update()
    {
        ResizeCollider();
    }

    void ResizeCollider()
    {
        Vector2 size = rectTransform.rect.size;
        Vector2[] edgePoints = new Vector2[5];

        // Calculate the corners of the RectTransform
        edgePoints[0] = new Vector2(-size.x / 2, size.y / 2); // Bottom left
        edgePoints[1] = new Vector2(-size.x / 2, -size.y / 2); // Top left
        edgePoints[2] = new Vector2(size.x / 2, -size.y / 2); // Top right
        edgePoints[3] = new Vector2(size.x / 2, size.y / 2); // Bottom right
        edgePoints[4] = new Vector2(-size.x / 2, size.y / 2); // Closing the loop back to bottom left

        edgeCollider.points = edgePoints;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            text.text = "Collision Detected!";
            windowGame.AdjustWindowSize(collision.collider);
        }
    }
}
