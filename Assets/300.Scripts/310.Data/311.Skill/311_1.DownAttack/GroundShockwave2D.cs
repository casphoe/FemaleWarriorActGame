using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class GroundShockwave2D : MonoBehaviour
{
    [Header("Move/Shape")]
    public float speed = 14f;          // 파동 진행 속도
    public float maxLength = 6;      // 최대 길이
    public float thickness = 0.45f;    // 파동 두께(시각+히트박스 공통)
    public float lifeTime = 0.7f;      // 생존 시간(초)

    [Header("Damage")]
    public int baseDamage = 15;
    public float knockback = 4f;
    public LayerMask enemyMask;        // 적 레이어
    public LayerMask groundMask;       // 지면 레이어

    [Header("Visual")]
    public float uvScrollSpeed = 2f;   // 텍스처 스크롤

    SpriteRenderer sr;
    BoxCollider2D box;
    HashSet<Collider2D> hitOnce = new HashSet<Collider2D>();

    float dir;      // 진행 방향(+1 오른쪽, -1 왼쪽)
    float age;
    float length;   // 현재 길이
    float originY;  // 기준 Y(지면 보정용)

    public void Fire(Vector2 origin, float direction)
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (box == null) box = GetComponent<BoxCollider2D>();

        dir = Mathf.Sign(direction);
        age = 0f;
        length = 0f;
        hitOnce.Clear();

        transform.position = origin;
        originY = origin.y;

        // 초기 사이즈 세팅
        box.isTrigger = true;
        UpdateHitbox();
        UpdateVisual();

        gameObject.SetActive(true);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        age += dt;

        // 길이 확장
        length = Mathf.Min(length + speed * dt, maxLength);

        // 앞단 지면 Y 보정(선택)
        float frontX = transform.position.x + dir * length;
        var rayStart = new Vector2(frontX, originY + 4f);
        var hit = Physics2D.Raycast(rayStart, Vector2.down, 3, groundMask);
        if (hit.collider) transform.position = new Vector2(transform.position.x, hit.point.y + 0.01f);
        else transform.position = new Vector2(transform.position.x, originY);

        UpdateHitbox();
        UpdateVisual();

        if (age >= lifeTime || length >= maxLength)
            gameObject.SetActive(false);
    }

    void UpdateHitbox()
    {
        // BoxCollider2D는 로컬 기준: size/offset으로 “시작점에서 앞쪽으로 뻗는 얇은 판” 구성
        box.size = new Vector2(Mathf.Max(length, 0.01f), thickness);
        box.offset = new Vector2(dir * (length * 0.5f), 0f);
    }

    void UpdateVisual()
    {
        // SpriteRenderer Draw Mode = Tiled 권장
        sr.size = new Vector2(Mathf.Max(length, 0.01f), thickness);
        // 진행 방향에 따라 좌우 뒤집기
        sr.flipX = dir < 0f;

        // 텍스처 스크롤로 파동 느낌
        var mat = sr.sharedMaterial;
        if (mat && mat.HasProperty("_MainTex"))
        {
            var o = mat.GetTextureOffset("_MainTex");
            o.x += uvScrollSpeed * Time.deltaTime * dir;
            mat.SetTextureOffset("_MainTex", o);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyMask) == 0) return;
        if (hitOnce.Contains(other)) return;

        hitOnce.Add(other);

        // 데미지 처리(네 게임 구조에 맞게 연결)
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(baseDamage, PlayerManager.instance.player.critcleRate, PlayerManager.instance.player.critcleDmg, 0);
            var rb = other.attachedRigidbody;
            if (rb)
            {
                float push = Mathf.Sign(dir) * knockback;
                rb.AddForce(new Vector2(push, knockback * 0.6f), ForceMode2D.Impulse);
            }
        }
    }
}
