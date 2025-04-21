using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM : MonoBehaviour
{
    //플레이어
    public Transform target;

    //카메라 이동 속도
    public float moveSpeed = 5;
    public List<Vector2> minMapLimitPoistion = new List<Vector2>();
    public List<Vector2> maxMapLimitPoistion = new List<Vector2>();

    public static CM instance;
    #region 카메라 흔들림 변수
    private Vector3 shakeOffset = Vector3.zero;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;
    #endregion

    private void Awake()
    {
        instance = this;
    }


    private void LateUpdate()
    {
        CameraMove();
    }

    void CameraMove()
    {
        if(PlayerManager.instance.IsDead == false)
        {
            // 플레이어의 현재 위치를 따라가도록 카메라의 위치를 설정
            Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            //카메라 위치를 최소값과 최대값 사이로 제한
            newPosition.x = Mathf.Clamp(newPosition.x, minMapLimitPoistion[Player.instance.currentMapNum].x, maxMapLimitPoistion[Player.instance.currentMapNum].x);
            newPosition.y = Mathf.Clamp(newPosition.y, minMapLimitPoistion[Player.instance.currentMapNum].y, maxMapLimitPoistion[Player.instance.currentMapNum].y);

            //흔들림 적용
            if(shakeDuration > 0)
            {
                //반지름 1짜리 3D 구 안에서 무작위 위치를 반환
                shakeOffset = Random.insideUnitSphere * shakeMagnitude;
                shakeOffset.z = 0f;
                shakeDuration -= Time.deltaTime;
            }
            else
            {
                shakeOffset = Vector3.zero;
            }

            //카메라 위치를 적용
            transform.position = newPosition + shakeOffset;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }

    public void SnapToTarget()
    {
        if (target == null) return;

        Vector3 snapPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        snapPosition.x = Mathf.Clamp(snapPosition.x, minMapLimitPoistion[Player.instance.currentMapNum].x, maxMapLimitPoistion[Player.instance.currentMapNum].x);
        snapPosition.y = Mathf.Clamp(snapPosition.y, minMapLimitPoistion[Player.instance.currentMapNum].y, maxMapLimitPoistion[Player.instance.currentMapNum].y);

        transform.position = snapPosition;
    }
}
