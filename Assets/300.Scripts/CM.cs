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

            //카메라 위치를 적용
            transform.position = newPosition;
        }
    }
}
