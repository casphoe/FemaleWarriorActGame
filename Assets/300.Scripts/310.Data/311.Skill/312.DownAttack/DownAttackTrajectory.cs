using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttackTrajectory : MonoBehaviour
{
    private LineRenderer trajectoryRenderer; //포물선 라인랜더러
    private Transform player;
    private LineRenderer landingCircleRenderer; //착지 지점 원 라인 렌더러


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
    }

    //다운어택 스텟 가져오는 함수
    public void DownAttackStat(float maxDistance, float Radius)
    {
        attackRadius = Radius;
        maxMoveDistance = maxDistance;
    }

    public void StartTrajectory()
    {
        //마우스 위치 가져오기
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        //이동 거리 제한
        Vector3 direction = (mousePosition - player.position).normalized;
        float distance = Mathf.Min(Vector3.Distance(player.position, mousePosition), maxMoveDistance);

        targetPosition = player.position + direction * distance;

        //포물선 그리기 및 착지 지점 원 그리기
        DrawTrajectory(targetPosition);
        DrawLandingCircle(targetPosition);
    }
    //포물선 그리기
    void DrawTrajectory(Vector3 target)
    {
        trajectoryRenderer.positionCount = trajectoryResolution;

        Vector3 startPosition = player.position;
        Vector3 velocity = (target - startPosition).normalized * attackSpeed;

        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = i * 0.1f;
            float x = startPosition.x + velocity.x * t;
            float y = startPosition.y + velocity.y * t - 0.5f * gravity * t * t;
            points.Add(new Vector3(x, y, 0));
        }
        trajectoryRenderer.SetPositions(points.ToArray());
    }

    void DrawLandingCircle(Vector3 target)
    {
        landingCircleRenderer.positionCount = circleResolution + 1;
        float angleStep = 360f / circleResolution;

        Vector3[] circlePoints = new Vector3[circleResolution + 1];
        for (int i = 0; i <= circleResolution; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * attackRadius;
            float y = Mathf.Sin(angle) * attackRadius;
            circlePoints[i] = new Vector3(x, y, 0) + target;
        }
        landingCircleRenderer.SetPositions(circlePoints);
    }

    //다운어택 타겟 위치로 이동시키는 함수
    public void ExecuteDownAttack()
    {

    }
}
