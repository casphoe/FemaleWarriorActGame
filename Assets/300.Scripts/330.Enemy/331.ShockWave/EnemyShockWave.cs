using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShockWave : MonoBehaviour
{
    [SerializeField] private Material shockWaveMat;

    private class ShockWave
    {
        public LineRenderer line;
        public float radius;
        public bool hasHit;

        public void Init(Transform origin, string name, Material mat, float startWidth, float offset, bool flipX)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(origin);
            obj.transform.localPosition = Vector3.zero;

            if (flipX)
                obj.transform.localScale = new Vector3(-1f, 1f, 1f);
            else
                obj.transform.localScale = Vector3.one;

            line = obj.AddComponent<LineRenderer>();
            line.material = mat;
            line.startWidth = startWidth;
            line.endWidth = startWidth;
            line.useWorldSpace = false;
            line.numCapVertices = 8;
            line.textureMode = LineTextureMode.Stretch;
            line.positionCount = 0;

            radius = 0.5f + offset;
            hasHit = false;
            SetAlpha(0f);
        }

        public void Draw(float alpha, bool isFacingRight)
        {
            int segments = 50;
            line.positionCount = segments + 1;
            float direction = isFacingRight ? 1f : -1f;
      
            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;

                // 핵심: X축 기준 부채꼴
                float x = direction * (t - 0.5f) * radius * 2f; // 중심 기준으로 양쪽 퍼짐
                float y = Mathf.Sin(t * Mathf.PI) * 0.02f; // y축은 살짝만 흔들림

                line.SetPosition(i, new Vector3(x, y, 0));
            }
            SetAlpha(alpha);
        }

        public void SetAlpha(float alpha)
        {
            Color c = new Color(1, 0, 0, alpha);
            line.startColor = c;
            line.endColor = c;
        }

        public void SetActive(bool state)
        {
            if (line != null)
                line.gameObject.SetActive(state);
        }
    }

    private ShockWave[] shockWaves;
    private int waveCount = 3;
    private float waveRadiusGap = 0.3f;
    private float shockTimer;
    private float shockAlpha;
    private bool isShockActive;
    private bool hasShockHit;
    private float shockDuration = 3f;
    private float shockExpandSpeed = 2f;
    private float shockFadeSpeed = 0.3f;
    private Transform player;
    private float damage;
    private float criticleRate;
    private float criticleDmage;
    private bool hasHit = false;

    Enemy enemy;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        shockWaves = new ShockWave[waveCount];
        enemy = GetComponent<Enemy>();
        for (int i = 0; i < waveCount; i++)
        {
            bool flipX = !enemy.isFacingRight;
            shockWaves[i] = new ShockWave();
            shockWaves[i].Init(enemy.shockWaveAttackTrans, $"ShockWave_{i}", shockWaveMat, 0.5f, i * waveRadiusGap, flipX);
            shockWaves[i].SetActive(false);
        }
    }

    public void Init(float _damage, float _criticleRate, float _criticleDmage)
    {  
        hasHit = false;

        damage = _damage;
        criticleRate = _criticleRate;
        criticleDmage = _criticleDmage;
    }

    public void FireShockWave(bool isFacingRight)
    {
        shockTimer = 0f;
        shockAlpha = 1f;
        hasShockHit = false;
        isShockActive = true;

        bool flipX = !isFacingRight;
        for (int i = 0; i < waveCount; i++)
        {
            shockWaves[i].radius = 0.5f + i * waveRadiusGap;
            shockWaves[i].hasHit = false;         
            shockWaves[i].line.transform.localScale = flipX ? new Vector3(-1f, 1f, 1f) : Vector3.one;
            shockWaves[i].SetActive(true);
        }
    }

    void Update()
    {
        if (isShockActive)
        {
            shockTimer += Time.deltaTime;
            shockAlpha -= shockFadeSpeed * Time.deltaTime;

            for (int i = 0; i < waveCount; i++)
            {               
                shockWaves[i].radius += shockExpandSpeed * Time.deltaTime;
                shockWaves[i].Draw(shockAlpha, enemy.isFacingRight);
            }

            float dist = Vector2.Distance(transform.position, Player.instance.transform.position);
            float maxRadius = shockWaves[waveCount - 1].radius;

            if (!hasShockHit && dist <= maxRadius)
            {
                Player.instance.TakeDamage(damage, criticleRate, criticleDmage, 1);
                hasShockHit = true;
            }

            if (shockTimer >= shockDuration)
            {
                foreach (var wave in shockWaves)
                    wave.SetActive(false);

                isShockActive = false;
            }
        }
    }
}
