using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//동굴 바위 함정 (포물선 함정)
public class RockPitFall : MonoBehaviour
{
    [Header("이동 설정")]
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float moveDuration = 2f;

    [Header("오브젝트 설정")]
    public Transform railTransform; // Rail만 회전

    public float rotationAngle = 50f; // 최대 회전 각도

    private float timer;
    private bool movingToEnd = true;

    void Start()
    {
        timer = 0f;
        transform.localPosition = startPoint;
        UpdateRailRotation(0f); // 시작은 0도
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / moveDuration;
        t = Mathf.Clamp01(t);

        Vector3 from = movingToEnd ? startPoint : endPoint;
        Vector3 to = movingToEnd ? endPoint : startPoint;

        // X축 직선 이동 (Y, Z는 고정)
        transform.localPosition = new Vector3(Mathf.Lerp(from.x, to.x, t), from.y, from.z);

        // 부드러운 회전
        UpdateRailRotation(t);

        if (t >= 1f)
        {
            timer = 0f;
            movingToEnd = !movingToEnd;
        }
    }

    void UpdateRailRotation(float t)
    {
        if (railTransform != null)
        {
            float smoothT = t * t * (3f - 2f * t); // SmoothStep: 부드럽게 가속/감속
            float targetAngle = movingToEnd ? -rotationAngle : rotationAngle;
            float rotation = Mathf.Lerp(0f, targetAngle, smoothT);
            railTransform.localRotation = Quaternion.Euler(0f, 0f, rotation);
        }
    }
}
