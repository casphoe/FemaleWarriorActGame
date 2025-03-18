using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttackTrajectory : MonoBehaviour
{
    private LineRenderer trajectoryRenderer; //포물선 라인랜더러
    private Transform player;
    private LineRenderer landingCircleRenderer; //착지 지점 원 라인 렌더러

    Vector3 landingPoint;
    private Vector3 targetPosition;
    float gravity = 9.8f;
    //최대 이동거리
    private float maxMoveDistance = 0;
    private int trajectoryResolution = 30; //포물선 정밀도
    private int circleResolution = 50; // 원의 정밀도
    private float attackRadius = 0; //착지 범위 반지름
    private float attackSpeed = 10; //공격 이동 속도

    private void Awake()
    {
        player = this.transform;
        trajectoryRenderer = transform.GetChild(1).GetComponent<LineRenderer>();
        landingCircleRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        trajectoryRenderer.positionCount = 0;
        landingCircleRenderer.positionCount = 0;

        trajectoryRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trajectoryRenderer.widthMultiplier = 0.1f;   // 두께 조정
        trajectoryRenderer.startColor = Color.red;   // 포물선 색상: 파란색
        trajectoryRenderer.endColor = Color.red;     // 끝 색상: 빨간색

        landingCircleRenderer.material = new Material(Shader.Find("Sprites/Default"));

        landingCircleRenderer.widthMultiplier = 1;   // 두께 조정
        landingCircleRenderer.startColor = new Color(1, 0, 0, 0.6f);
        landingCircleRenderer.endColor = new Color(1, 0, 0, 0.15f);
    }

    //다운어택 스텟 가져오는 함수
    public void DownAttackStat(float maxDistance, float Radius)
    {
        attackRadius = Radius;
        maxMoveDistance = maxDistance;
    }

    public void StartTrajectory()
    {
        //최대 이동거리로 설정
        attackSpeed = Mathf.Sqrt(2 * gravity * maxMoveDistance);

        // 최대 이동거리로 목표 지점 설정
        targetPosition = player.position + new Vector3(maxMoveDistance, 0, 0);

        // 포물선 그리기 및 착지 지점 원 그리기
        Vector3 landingPoint = DrawTrajectory(targetPosition);
        DrawLandingCircle(landingPoint);
    }
    //포물선 그리기
    Vector3 DrawTrajectory(Vector3 target)
    {
        trajectoryRenderer.positionCount = trajectoryResolution;

        Vector3 startPosition = player.position;
        Vector3 velocity = new Vector3(maxMoveDistance, 0, 0).normalized * attackSpeed; // X축으로 이동

        List<Vector3> points = new List<Vector3>();
        Vector3 lastPoint = startPosition; // 마지막 점 저장용

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = i * 0.1f;
            float x = startPosition.x + velocity.x * t;
            float y = startPosition.y + velocity.y * t - 0.5f * gravity * t * t;
            Vector3 point = new Vector3(x, y, 0);
            points.Add(point);
            lastPoint = point; // 마지막 점 저장
        }

        trajectoryRenderer.SetPositions(points.ToArray());
        return lastPoint; // 포물선 끝점 반환
    }

    void DrawLandingCircle(Vector3 target)
    {
        landingCircleRenderer.positionCount = circleResolution + 1;
        landingCircleRenderer.useWorldSpace = true; // ✅ 월드 좌표 기준으로 그리기

        float angleStep = 360f / circleResolution;
        Vector3[] circlePoints = new Vector3[circleResolution + 1];

        // 🌟 원의 중심을 정확히 착지 지점에 맞추기
        Vector3 circleCenter = new Vector3(landingPoint.x, landingPoint.y - attackRadius, 0);

        for (int i = 0; i <= circleResolution; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = circleCenter.x + Mathf.Cos(angle) * attackRadius; // ✅ X 위치는 원의 중심을 기준으로 회전
            float y = circleCenter.y + Mathf.Sin(angle) * attackRadius; // ✅ Y 위치는 착지 위치 기준으로 회전
            circlePoints[i] = new Vector3(x, y, 0);
        }

        landingCircleRenderer.SetPositions(circlePoints);
    }

    //다운어택 타겟 위치로 이동시키는 함수
    public void ExecuteDownAttack()
    {

    }
}
