using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlashState
{
    Player,Enemy
}

public class Slash : MonoBehaviour
{
    public float damage;
    public float criticleRate;
    public float criticleDamage;
    public float duration;
    public float moveSpeed;
    public float fadeSpeed = 1f;         // 사라지는 속도
    public float expandSpeed = 1.5f;     // 커지는 속도
    public Vector2 moveDir;             // 이동 방향
    public SlashState state;

    public void Init(float damage, float critcleRate, float criticleDamage, float moveSpeed, float duration, Vector2 moveDir, float expandSpeed, SlashState state)
    {
        this.damage = damage;
        this.criticleRate = critcleRate;
        this.criticleDamage = criticleDamage;
        this.duration = duration;
        this.moveSpeed = moveSpeed;
        this.moveDir = moveDir;
        this.expandSpeed = expandSpeed;
        this.state = state;

        if (sr != null)
            sr.flipX = moveDir.x < 0;
        else
        {
            sr = GetComponent<SpriteRenderer>();
            sr.flipX = moveDir.x < 0;
        }
    }

    private SpriteRenderer sr;
    private Vector3 initialScale;
    private bool hasHit = false;

    private float timer = 0;
    private float alpha = 1;
    private float scaledDamage;

    private float dist;

    private void Awake()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;

        sr.flipX = moveDir.x < 0;
    }

    private void OnEnable()
    {
        timer = 0;
        alpha = 1;
        hasHit = false;

        transform.localScale = initialScale;

        sr.flipX = moveDir.x < 0;
        sr.color = new Color(1, 1, 1, 1);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        //오브젝트 가 시간에 따라서 켜지는 기능
        transform.localScale += initialScale * expandSpeed * Time.deltaTime;

        //오브젝트 투명도 감소
        alpha -= fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        sr.color = new Color(1, 1, 1, alpha);

        //충돌 범위
        float attackRadius = transform.localScale.x * 1.5f; // 또는 원하는 값

        // 충돌 체크 (거리 기반)
        if (!hasHit)
        {
            if (state == SlashState.Enemy) //적이 생성
            {
                //현재 오브젝트 좌표에서 충돌 범위안에 Player Layer가 있는 오브젝트의 충돌 콜라이더를 찾음
                Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, attackRadius, LayerMask.GetMask("Player"));
                foreach (Collider2D col in hitTargets) 
                {
                    Player player = col.GetComponent<Player>();
                    if (player != null)
                    {
                        float scaledDamage = damage * Mathf.Lerp(0.5f, 1f, alpha); // 알파값 비례 데미지
                        player.TakeDamage(scaledDamage, criticleRate, criticleDamage, 1);
                        hasHit = true;
                        gameObject.SetActive(false); 
                    }
                }
            }
            else //플레이어 생성
            {
                Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, attackRadius, LayerMask.GetMask("Enemy"));
                foreach (Collider2D col in hitTargets)
                {
                    Enemy enemy = col.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        float scaledDamage = damage * Mathf.Lerp(0.5f, 1f, alpha); // 알파값 비례 데미지(충돌체 생성 위치와 적이 가까이 잇으면 있을 수록 데미지 증가)
                        enemy.TakeDamage(scaledDamage, criticleRate, criticleDamage);
                        hasHit = true;
                        gameObject.SetActive(false);
                    }
                }
            }
        }


        // 지속 시간 초과 시 비활성화
        if (timer >= duration)
        {
            gameObject.SetActive(false);
        }

    }
}
