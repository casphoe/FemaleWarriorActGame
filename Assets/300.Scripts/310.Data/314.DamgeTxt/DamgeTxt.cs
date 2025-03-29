using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//데미지 텍스트 관련 함수
public class DamgeTxt : MonoBehaviour
{
    TextMeshPro txtMesh;

    float lifeTime = 4; //텍스트 지속시간
    float moveSpeed = 4; // 위로 올라가는 속도

    Color textColor;

    private void Awake()
    {
        txtMesh = GetComponent<TextMeshPro>();
        textColor = txtMesh.color;
    }

    public void damageTxtSetting(int num, float damge)
    {
        txtMesh.text = damge.ToString("N0"); //천의 자리 표시
        textColor.a = 1f;
        switch (num)
        {
            case 0: //노 크리티컬 데미지
                switch (GameManager.data.day)
                {
                    case Day.Afternoon:
                        textColor = new Color(1, 0.92f, 0.016f, 1);
                        break;
                    case Day.Night:
                        textColor = Color.white; 
                        break;
                }             
                break;
            case 1: //크리티컬 데미지
                textColor = Color.red;
                break;            
        }
        txtMesh.color = textColor;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lifeTime)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime; // 위로 이동
            textColor.a = Mathf.Lerp(1f, 0f, elapsedTime / lifeTime); // 알파값 감소
            txtMesh.color = textColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Utils.OnOff(gameObject, false);
    }
}
