using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttackTrajectory : MonoBehaviour
{
    private LineRenderer trajectoryRenderer; //포물선 라인랜더러
    private Transform player;

    Vector3 landingPoint;
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
        trajectoryRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
        trajectoryRenderer.positionCount = 0;

        trajectoryRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trajectoryRenderer.widthMultiplier = 0.1f;   // 두께 조정
        trajectoryRenderer.startColor = Color.red;   // 포물선 색상: 파란색
        trajectoryRenderer.endColor = Color.red;     // 끝 색상: 빨간색
    }

    //다운어택 스텟 가져오는 함수
    public void DownAttackStat(float maxDistance, float Radius)
    {
        attackRadius = Radius;
        maxMoveDistance = maxDistance;
    }

    Vector3 FindGroundPoint(Vector3 startPoint)
    {
        RaycastHit2D hit = Physics2D.Raycast(startPoint, Vector2.down, 10f, LayerMask.GetMask("Ground"));
        return hit.collider != null ? hit.point : startPoint;
    }

    public void StartTrajectory()
    {
        //최대 이동거리로 설정
        Vector3 groundPoint = FindGroundPoint(player.position + new Vector3(maxMoveDistance, 0, 0));

        // 포물선 그리기 및 착지 지점 원 그리기
        Vector3 landingPoint = DrawTrajectory(groundPoint);
    }
    //포물선 그리기
    Vector3 DrawTrajectory(Vector3 target)
    {
        trajectoryRenderer.positionCount = trajectoryResolution;

        Vector3 startPosition = player.transform.GetChild(1).position;
        Vector3 endPosition = target; // 무조건 maxMoveDistance까지 이동
        Vector3 midPosition = (startPosition + endPosition) / 2 + new Vector3(0, maxMoveDistance * 0.5f, 0); // 가운데 높이 조정

        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i <= trajectoryResolution; i++)
        {
            float t = i / (float)trajectoryResolution; // 0 ~ 1 사이의 값
            Vector3 point = (1 - t) * (1 - t) * startPosition + 2 * (1 - t) * t * midPosition + t * t * endPosition;
            points.Add(point);
        }

        trajectoryRenderer.SetPositions(points.ToArray());
        return endPosition; // 포물선 끝점 반환s
    }

    //다운어택 타겟 위치로 이동시키는 함수
    public void ExecuteDownAttack()
    {

    }
}
