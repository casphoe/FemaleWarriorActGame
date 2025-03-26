using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeSlide : MonoBehaviour
{
    public float slideForce = 5f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        if (hit)
        {
            float angle = Vector2.Angle(hit.normal, Vector2.up);
            if (angle > 30f)
            { // 경사 각도 기준
                Vector2 slideDir = new Vector2(hit.normal.x, -hit.normal.y);
                rb.velocity += slideDir * slideForce * Time.deltaTime;
            }
        }
    }
}
