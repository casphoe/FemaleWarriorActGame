using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeSlide : MonoBehaviour
{
    public float slideForce = 10f;            // 미끄러지는 힘
    public float slideAngleThreshold = 30;   // 이 각도 이상이면 미끄러짐
    public LayerMask groundLayer;             // 땅만 감지
    private Rigidbody2D rb;
    private bool isSliding;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 발 밑 방향으로 Raycast를 쏴서 경사 정보를 얻는다
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        if (hit)
        {
            float angle = Vector2.Angle(hit.normal, Vector2.up); // 땅과의 각도
            if (angle >= slideAngleThreshold)
            {
                // 경사가 충분히 가파르면 슬라이딩 시작
                Vector2 slideDirection = new Vector2(hit.normal.x, -hit.normal.y); // 경사 방향 계산
                rb.velocity += slideDirection * slideForce * Time.fixedDeltaTime;
                isSliding = true;
            }
            else
            {
                isSliding = false;
            }
        }
        else
        {
            isSliding = false;
        }
    }
}
