using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//다리 조각이 플레이어가 닿았을 때 일정 시간 동안 위아래로 흔들리게 만드는 스크립트
public class BridgeShaker : MonoBehaviour
{
    //다리 원래 위치 흔들림이 끝났을 때 제자리로 되돌리기 위해 사용
    private Vector3 originalPos;
    //흔들리고 있는지 판단하기 위한 변수
    private bool isShaking = false;
    private float shakeTime = 0f;
    //흔들림 총 지속시간
    private float shakeDuration = 0.75f;
    //흔들림 세기
    private float shakeAmount = 0.1f;
    private BoxCollider2D col;

    void Start()
    {
        originalPos = transform.position;
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isShaking && Player.instance.moveDirection != 0)
        {
            shakeTime += Time.deltaTime;
            //좌우로 흔들리게 함 , 진동/파형을 만들기 위해서 사용
            float offsetY = Mathf.Sin(shakeTime * 30f) * shakeAmount;
            float offsetX = Mathf.Cos(shakeTime * 20f) * shakeAmount * 0.3f;
            transform.position = originalPos + new Vector3(offsetX, offsetY, 0f);

            if (shakeTime >= shakeDuration)
            {
                isShaking = false;
                shakeTime = 0f;
                transform.position = originalPos;
            }
        }
    }

    public void Shake()
    {
        if (isShaking) return; // 이미 흔들리는 중이면 무시
        isShaking = true;
        shakeTime = 0f;
    }

    // 플레이어가 올라왔을 때 감지하는 트리거
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Shake();
        }
    }
}
