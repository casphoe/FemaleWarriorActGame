using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//동굴 바위 함정 (포물선 함정)
public class RockPitFall : MonoBehaviour
{
    [Header("이동 설정")]
    public Vector3 startPoint; //이동 시작 지점
    public Vector3 endPoint; //이동 끝 지점
    public float moveDuration = 2f; //한 방향으로 이동하는데 걸리는 시간

    [Header("오브젝트 설정")]
    public Transform railTransform; // Rail만 회전

    public float rotationAngle = 50f; // 최대 회전 각도

    private float timer;
    private bool movingToEnd = true; //true이면 start -> end false : end -> start으로 이동

    void Start()
    {
        movingToEnd = true;
        timer = 0f;
        transform.localPosition = startPoint; //함정 오브젝트 위치를 시작 지점으로 고정
        UpdateRailRotation(0f); // 시작은 0도
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / moveDuration; //t : 0 시작, 1 이면 끝 상태
        t = Mathf.Clamp01(t);

        //현재 진행 방향을 설정 (true : start -> end, false면 반대)
        Vector3 from = movingToEnd ? startPoint : endPoint;
        Vector3 to = movingToEnd ? endPoint : startPoint;

        // X축 직선 이동 (Y, Z는 고정)
        // from.x 에서 to.x 까지 t(0~!)에 따라 선형 보간
        // x 좌표 값이 시간에 따라 부드럽게 변화
        //ex) from.x = -2 to.x = 2 이면 t = 0 x = -2, t = 0.5 x = 0, t = 1 x = 2
        transform.localPosition = new Vector3(Mathf.Lerp(from.x, to.x, t), from.y, from.z);

        // 부드러운 회전
        UpdateRailRotation(t);

        if (t >= 1f)
        {
            timer = 0f;
            movingToEnd = !movingToEnd;
        }
    }
    //현재 이동 진행률 t에 따라 z 축 회전을 +rotationAngle, -rotationAngle 사이로 조정합니다. 
    void UpdateRailRotation(float t)
    {
        if (railTransform != null)
        {
            float smoothT = t * t * (3f - 2f * t); //시작과 끝은 느리게 중간은 빠르게 -> 자연스럽고 탄력 있는 움직임
            float targetAngle = movingToEnd ? -rotationAngle : rotationAngle; //이동 방향에 따라 목표 회전 각도를 정함 
            float rotation = Mathf.Lerp(0f, targetAngle, smoothT); //회전이 한쪽으로 서서히 가다가 방향 바꾸면 다시 부드럽게 반대로 되돌아 가게 하기 위해 사용
            railTransform.localRotation = Quaternion.Euler(0f, 0f, rotation);
        }
    }
}
