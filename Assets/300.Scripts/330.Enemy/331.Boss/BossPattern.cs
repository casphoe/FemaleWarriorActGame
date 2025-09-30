using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Pattern")]
public class BossPattern : ScriptableObject
{
    public string patternId;            // "JumpSmash", "BombLob", ...
    public float telegraphTime = 0.6f;  // 전조
    public float recoverTime = 0.5f;    // 회수
    public float cooldown = 3.0f;
    public int phaseMin = 1;          // 시작 페이즈
    public int phaseMax = 3;          // 끝 페이즈
    public int weight = 10;           // 선택 가중치
}
