using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//다운어택 에 맞거나 가드 게이지 값이 0이 되었을 때 혼란 상태를 나타내주는 스크립트
public class ConfusionQuestionMark : MonoBehaviour
{
    public float rotateSpeed = 45f;        // 초당 회전 속도
    public float floatSpeed = 2f;          // 위아래 이동 속도
    public float floatHeight = 0.2f;       // 이동 높이
    public float alphaSpeed = 2f;          // 깜빡임 속도

    private Vector3 startPos;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        startPos = transform.position;
        sr.color = new Color(1, 1, 1, 1);
    }

    public void SetBasePosition(Vector3 pos)
    {
        startPos = pos;
        transform.position = startPos;
    }

    void Update()
    {
        // 회전
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        // sin을 이용해서 부드럽게 위 아래로 움직이는 부유 효과를 나타냄
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        Vector3 newPos = startPos + new Vector3(0, offset, 0);
        transform.position = newPos;

        // 깜빡임 (알파 값이 0~1 사이에서 주기적으로 변화하게 함) 
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * alphaSpeed));
        if (sr != null)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }

    //외부에서 시간을 가져오기(종료 시간)
    public void ActivateForDuration(float duration)
    {
        CancelInvoke(); // 중복 방지
        //duration 시간 후의 Disable 이라는 함수를 실행
        Invoke(nameof(Disable), duration);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
