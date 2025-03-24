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
        PlayerManager.instance.isAiming = false;
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

        // 포물선 그리기
        DrawTrajectory(groundPoint);
    }

    //포물선 그리기
    void DrawTrajectory(Vector3 target)
    {
        trajectoryRenderer.positionCount = trajectoryResolution;

        Vector3 startPosition = player.transform.GetChild(0).position;
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
    }

    public void MoveTrajectory()
    {
        if(PlayerManager.instance.isAiming)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3 playerPos = player.transform.position;
            float distance = Vector3.Distance(playerPos, mouseWorldPos);

            //flip 처리
            if (mouseWorldPos.x < playerPos.x)
                player.localScale = new Vector3(-1 * Mathf.Abs(player.localScale.x), player.localScale.y, player.localScale.z); // 왼쪽
            else
                player.localScale = new Vector3(Mathf.Abs(player.localScale.x), player.localScale.y, player.localScale.z); // 오른쪽

            // maxMoveDistance 초과 방지
            if (distance > maxMoveDistance)
            {
                Vector3 direction = (mouseWorldPos - playerPos).normalized;
                mouseWorldPos = playerPos + direction * maxMoveDistance;
            }

            // 지면 위치로 조정
            Vector3 groundPoint = FindGroundPoint(mouseWorldPos);
            DrawTrajectory(groundPoint);
        }
    }

    //다운어택 타겟 위치로 이동시키는 함수
    public void ExecuteDownAttack()
    {
        // trajectoryRenderer 마지막 점으로 이동
        if (trajectoryRenderer.positionCount > 0)
        {
            Vector3 target = trajectoryRenderer.GetPosition(trajectoryRenderer.positionCount - 1);
            StartCoroutine(DownAttackMove(target));
        }
    }

    IEnumerator DownAttackMove(Vector3 target)
    {
        Vector3 start = player.position;
        float elapsed = 0f;
        float duration = Vector3.Distance(start, target) / attackSpeed;

        trajectoryRenderer.positionCount = 0; // 라인 제거

        //중력 제거
        float originalGravity = Player.instance.rb.gravityScale;
        Player.instance.rb.gravityScale = 0;
        Player.instance.rb.velocity = Vector2.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // x 선형 이동
            float x = Mathf.Lerp(start.x, target.x, t);

            // y는 포물선 곡선처럼: -4(a)(t)(1 - t)
            float y = Mathf.Lerp(start.y, target.y, t) + maxMoveDistance * 0.5f * 4 * t * (1 - t);

            Player.instance.rb.MovePosition(new Vector2(x, y));
            yield return null;
        }

        Player.instance.rb.MovePosition(target);

        Player.instance.rb.gravityScale = originalGravity;

        // TODO: 이곳에 충돌 처리나 공격 로직 추가

    }
}
