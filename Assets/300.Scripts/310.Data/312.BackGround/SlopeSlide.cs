using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeSlide : MonoBehaviour
{
    public float slideForce = 10f;            // 미끄러지는 힘
    public float slideAngleThreshold = 20;   // 이 각도 이상이면 미끄러짐
    public LayerMask groundLayer;             // 땅만 감지
    private Rigidbody2D rb;
    private bool isSliding; // 현재 슬라이딩 중인지 여부 판단

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isSliding = false;
    }

    void Update()
    {
        // 발 밑 방향으로 Raycast를 쏴서 경사 정보를 얻는다
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);

        if (hit)
        {
            float angle = Vector2.Angle(hit.normal, Vector2.up); // 땅과의 각도

            // 경사면 방향 계산: 법선 벡터에 수직인 벡터
            Vector2 slopeDirection = Vector2.Perpendicular(hit.normal).normalized;

            // 위로 향하는 방향이면 반전시켜서 항상 아래로 향하게 함
            if (slopeDirection.y > 0)
                slopeDirection *= -1f;


            float currentSlideSpeed = Vector2.Dot(rb.velocity, slopeDirection);
            float maxSlideSpeed = 8f; // 좀 더 크게
            float acceleration = slideForce * Time.deltaTime;


            // 경사가 충분히 가파르면 슬라이딩
            if (angle >= slideAngleThreshold)
            {
                // 점점 속도가 쌓이게끔 가속도 적용
                if (currentSlideSpeed < maxSlideSpeed)
                {
                    rb.velocity += slopeDirection * acceleration;
                }

                isSliding = true;
            }
            else
            {
                if (angle >= slideAngleThreshold)
                {
                    // 플레이어의 이동 방향이 경사 반대일 때만 감속 적용
                    float moveAgainstSlope = Vector2.Dot(rb.velocity, slopeDirection);

                    // velocity가 충분히 클 때만 감속 적용 (정지/떨림 방지)
                    if (moveAgainstSlope > 0.1f)
                    {
                        float resistanceFactor = 0.5f;
                        rb.velocity -= slopeDirection * moveAgainstSlope * resistanceFactor;
                    }
                }

                isSliding = false;
            }
        }
        else
        {
            isSliding = false;
        }
    }
}
