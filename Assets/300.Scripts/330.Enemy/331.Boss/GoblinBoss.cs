using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState { Idle, Telegraph, Attack, Recover, Stunned }

public class GoblinBoss : MonoBehaviour
{
    public BossState state;

    public Transform player;
    public Animator anim;
    public BossPatternRunner runner; // 패턴 실행기(아래)
    public float hp = 100f;

    void Update()
    {
        // 페이즈 전환은 hp 기반
        var phase = (hp > 70) ? 1 : (hp > 40) ? 2 : 3;

        if (state == BossState.Idle)
        {
            // 쿨 끝나면 다음 패턴 선택
            if (runner.Ready)
            {
                var next = runner.PickPattern(phase);
                StartCoroutine(runner.Run(next, this));
            }
        }
    }

    public void SetState(BossState s) { state = s; }
}