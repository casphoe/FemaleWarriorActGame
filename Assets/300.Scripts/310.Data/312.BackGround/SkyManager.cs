using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

//컴퓨터 시간 1시간을 24시간으로 치환해서 낮 밤을 표시

public class SkyManager : MonoBehaviour
{
    [SerializeField] Material[] skyMaterial;

    [Header("Sun Setting")]
    [SerializeField] Transform sun; //해 오브젝트
    Light directionalLight; //빛 오브젝트
    Camera mainCamera;
    [SerializeField] float sunHeight = 5f;           // 해가 떠 있을 높이 (카메라 기준 Y)
    [SerializeField] float sunZOffset = 10f;         // 카메라보다 앞에 보이게 Z 조정
    [SerializeField] float sunXRange = 6f;           // 좌우 이동 범위
    [SerializeField] float sunArcHeight = 3f;   // 아치형 곡선 높이 (최정점)

    [SerializeField] GameObject nightOverlay;

    public Sprite shadowUp;
    public Sprite shadowDown;
    public Sprite shadowLeft;
    public Sprite shadowRight;

    float gameMinutesPerRealSecond = 24f * 60f / 1800f; // 현실 시간 30분 = 게임 24시간
    float gameTime; // 게임 내 시간 (분 단위)
    float startTime;

    public List<Transform> allShadows;

    public float hour;

    public static SkyManager instance;

    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        directionalLight = sun.transform.GetChild(0).GetComponent<Light>();
        instance = this;
        startTime = Time.time;
        Utils.OnOff(nightOverlay, false);
        nightOverlay.GetComponent<Image>().color = new Color(0.05f, 0.05f, 0.2f, 0.5f);
    }

    void Update()
    {
        UpdateGameTime();
        UpdateSkybox();
        UpdateSunPosition();
    }

    void UpdateGameTime()
    {
        float realElapsed = Time.time - startTime;
        gameTime = realElapsed * gameMinutesPerRealSecond;
    }

    // 게임 내 시간 (0~24) 반환
    float GetGameHour()
    {
        return (gameTime / 60f) % 24f; // 60으로 나누고 24로 모듈러
    }

    void UpdateSkybox()
    {
        hour = GetGameHour(); // 예: 6.25, 14.6 등
        if (hour >= 6f && hour < 10f)
        {
            RenderSettings.skybox = skyMaterial[0];        
            nightOverlay.SetActive(false);
        }
        else if (hour >= 10f && hour < 17f)
        {
            RenderSettings.skybox = skyMaterial[1];         
            nightOverlay.SetActive(false);
        }
        else if (hour >= 17f && hour < 20f)
        {
            RenderSettings.skybox = skyMaterial[2];            
            nightOverlay.SetActive(false);
        }
        else
        {
            RenderSettings.skybox = skyMaterial[3];
            nightOverlay.SetActive(true);
        }
        // 변경된 Skybox를 바로 적용
        DynamicGI.UpdateEnvironment();
    }

    void UpdateSunPosition()
    {
        float minute = (gameTime % 60f); // 게임 내 분
        Vector3 camPos = mainCamera.transform.position;

        if (hour >= 6 && hour < 17)
        {
            float t = Mathf.InverseLerp(6f, 17f, hour + (minute / 60f));

            // X: 좌우 이동
            float sunX = Mathf.Lerp(-sunXRange, sunXRange, t);

            // Y: 곡선 경로
            float sunY = camPos.y + sunHeight + Mathf.Sin(t * Mathf.PI) * sunArcHeight;

            // Z: 카메라보다 앞에
            Vector3 sunWorldPos = new Vector3(camPos.x + sunX, sunY, camPos.z + sunZOffset);
            sun.position = sunWorldPos;

            // 회전도 같이 적용
            float rotationAngle = Mathf.Lerp(0f, 360f, t);
            sun.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

            //빛 위치 회전도 태양처럼 동일하게 줘야함
            directionalLight.transform.position = sunWorldPos;
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3(50f - t * 100f, 30f, 0f)); // Y축은 고정, X축만 회전

            //빛 강도 조절
            directionalLight.intensity = Mathf.Lerp(0.3f, 1f, Mathf.Sin(t * Mathf.PI));

            sun.gameObject.SetActive(true);
            directionalLight.gameObject.SetActive(true);

            UpdateShadows(sunWorldPos);
        }
        else
        {
            sun.gameObject.SetActive(false);
            directionalLight.gameObject.SetActive(false);

            foreach (var shadow in allShadows)
                if (shadow != null) shadow.gameObject.SetActive(false);
        }
    }
    #region 그림자
    void UpdateShadows(Vector3 sunPos)
    {
        Vector3 lightDir = (sunPos - mainCamera.transform.position).normalized;

        foreach (var shadow in allShadows)
        {
            if (shadow == null) continue;

            string name = shadow.gameObject.name;

            // 방향에 따라 스프라이트 선택
            if (Mathf.Abs(lightDir.x) > Mathf.Abs(lightDir.y))
                shadow.gameObject.GetComponent<SpriteRenderer>().sprite = lightDir.x > 0 ? shadowRight : shadowLeft;
            else
                shadow.gameObject.GetComponent<SpriteRenderer>().sprite = lightDir.y > 0 ? shadowUp : shadowDown;
            shadow.rotation = Quaternion.identity;
            shadow.gameObject.SetActive(true);
        }
    }

    public void HandleShadowVisibility(GameObject _shadow, SpriteRenderer shadowRenderer, float fadeSpeed)
    {
        if (_shadow == null) return;

        float targetAlpha = PlayerManager.instance.isGround ? 1f : 0f; // 착지하면 보이고, 점프하면 사라짐

        Color currentColor = shadowRenderer.color;
        float newAlpha = Mathf.Lerp(currentColor.a, targetAlpha, Time.deltaTime * fadeSpeed);
        shadowRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
    }

    #endregion
}
