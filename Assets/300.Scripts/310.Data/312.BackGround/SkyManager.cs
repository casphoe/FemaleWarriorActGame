using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkyManager : MonoBehaviour
{
    [SerializeField] Material[] skyMaterial;

    [Header("Sun Setting")]
    [SerializeField] Transform sun; //해 오브젝트
    Camera mainCamera;
    [SerializeField] float sunHeight = 5f;           // 해가 떠 있을 높이 (카메라 기준 Y)
    [SerializeField] float sunZOffset = 10f;         // 카메라보다 앞에 보이게 Z 조정
    [SerializeField] float sunXRange = 6f;           // 좌우 이동 범위
    [SerializeField] float sunArcHeight = 3f;   // 아치형 곡선 높이 (최정점)

    int hour;

    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        UpdateSkybox();
        UpdateSunPosition();
    }

    void UpdateSkybox()
    {
        hour = DateTime.Now.Hour;

        if (hour >= 6 && hour < 10)
        {
            // 낮 시간: 6시 ~ 17시
            RenderSettings.skybox = skyMaterial[0];
        }
        else if (hour >= 10 && hour < 17)
        {
            RenderSettings.skybox = skyMaterial[1];
        }
        else if (hour >= 17 && hour < 20)
        {
            RenderSettings.skybox = skyMaterial[2];
        }
        else
        {
            RenderSettings.skybox = skyMaterial[3];
        }
        // 변경된 Skybox를 바로 적용
        DynamicGI.UpdateEnvironment();
    }

    void UpdateSunPosition()
    {
        int hour = DateTime.Now.Hour;
        float minute = DateTime.Now.Minute;
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

            sun.gameObject.SetActive(true);
        }
        else
        {
            sun.gameObject.SetActive(false);
        }
    }
}
