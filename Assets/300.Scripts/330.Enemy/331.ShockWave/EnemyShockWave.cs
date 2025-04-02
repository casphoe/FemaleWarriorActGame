using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShockWave : MonoBehaviour
{
    public float speed = 3f;
    public float expandSpeed = 1.5f;       // Ŀ���� �ӵ�
    public float fadeSpeed = 1f;           // ������� �ӵ�
    public float duration = 1.2f;          // �� �ִϸ��̼� ���� �ð�

    private SpriteRenderer sr;
    private Vector3 initialScale;
    private float timer;
    private Vector2 moveDir;

    private float damage;
    private float criticleRate;
    private float criticleDmage;


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;
    }

    private void OnEnable()
    {
        sr.color = new Color(1, 1, 1, 1);
    }

    public void Init(Vector2 direction , float _damage, float _criticleRate, float _critileDamage)
    {
        moveDir = direction.normalized;
        transform.localScale = initialScale;
        sr.color = new Color(1, 1, 1, 1); // ���� ������
        timer = 0f;
        damage = _damage;
        criticleRate = _criticleRate;
        criticleDmage = _critileDamage;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // ������ �̵�
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        // ���� Ŀ��
        transform.localScale += Vector3.one * expandSpeed * Time.deltaTime;

        // ���� �����
        Color c = sr.color;
        c.a -= fadeSpeed * Time.deltaTime;
        sr.color = c;

        // ���� �ð� ���� �� ��Ȱ��ȭ
        if (timer >= duration)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.instance.TakeDamage(damage, criticleRate, criticleDmage, 1);
            gameObject.SetActive(false);
        }
    }
}
