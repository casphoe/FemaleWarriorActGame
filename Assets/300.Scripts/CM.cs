using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM : MonoBehaviour
{
    //플레이어
    public Transform target;

    //카메라 이동 속도
    public float moveSpeed = 5;
    [Tooltip("각 맵의 좌하단(최소) 경계")]
    public List<Vector2> minMapLimitPoistion = new List<Vector2>();
    [Tooltip("각 맵의 우상단(최대) 경계")]
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

    private int GetActiveMapNum()
    {
        if (Player.instance != null) return Player.instance.currentMapNum; 
        return 0;
    }

    private void CameraMove()
    {
        if (target == null) return;
        if (PlayerManager.instance != null && PlayerManager.instance.IsDead) return;

        int mapNum = GetActiveMapNum();

        Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        if (TryGetBounds(mapNum, out var min, out var max))
        {
            newPosition.x = Mathf.Clamp(newPosition.x, min.x, max.x);
            newPosition.y = Mathf.Clamp(newPosition.y, min.y, max.y);
        }
        // 경계가 아직 없으면 클램프 없이 따라가게 둠

        if (shakeDuration > 0f)
        {
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeOffset.z = 0f;
            shakeDuration -= Time.deltaTime;
        }
        else shakeOffset = Vector3.zero;

        transform.position = newPosition + shakeOffset;
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }

    /// <summary>
    /// 맵 전환 + 즉시 스냅. 내부 상태를 저장하지 않고, 단일 소스(PM.playerData)만 갱신.
    /// </summary>
    public void SetMap(int mapNum, bool snapToTarget = true)
    {
        // 단일 소스 갱신
        if (PM.playerData != null) PM.playerData.currentMapNum = mapNum;
        if (Player.instance != null) Player.instance.currentMapNum = mapNum; // 폴백/호환

        if (!snapToTarget || target == null) return;

        if (TryGetBounds(mapNum, out var min, out var max))
        {
            Vector3 pos = new Vector3(target.position.x, target.position.y, transform.position.z);
            pos.x = Mathf.Clamp(pos.x, min.x, max.x);
            pos.y = Mathf.Clamp(pos.y, min.y, max.y);
            transform.position = pos;
        }
        else
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }

    public void SnapToTarget(int currentMapNum) => SetMap(currentMapNum, true);

    private bool TryGetBounds(int mapNum, out Vector2 min, out Vector2 max)
    {
        min = default; max = default;
        if (mapNum < 0) return false;
        if (mapNum >= minMapLimitPoistion.Count) return false;
        if (mapNum >= maxMapLimitPoistion.Count) return false;

        min = minMapLimitPoistion[mapNum];
        max = maxMapLimitPoistion[mapNum];
        return true;
    }

    private void EnsureSize(List<Vector2> list, int size)
    {
        while (list.Count < size) list.Add(Vector2.zero);
    }
}
