using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkyManager : MonoBehaviour
{
    [SerializeField] Material[] skyMaterial;

    [Header("Sun Setting")]
    [SerializeField] Transform sun; //�� ������Ʈ
    Camera mainCamera;
    [SerializeField] float sunHeight = 5f;           // �ذ� �� ���� ���� (ī�޶� ���� Y)
    [SerializeField] float sunZOffset = 10f;         // ī�޶󺸴� �տ� ���̰� Z ����
    [SerializeField] float sunXRange = 6f;           // �¿� �̵� ����
    [SerializeField] float sunArcHeight = 3f;   // ��ġ�� � ���� (������)

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
            // �� �ð�: 6�� ~ 17��
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
        // ����� Skybox�� �ٷ� ����
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

            // X: �¿� �̵�
            float sunX = Mathf.Lerp(-sunXRange, sunXRange, t);

            // Y: � ���
            float sunY = camPos.y + sunHeight + Mathf.Sin(t * Mathf.PI) * sunArcHeight;

            // Z: ī�޶󺸴� �տ�
            Vector3 sunWorldPos = new Vector3(camPos.x + sunX, sunY, camPos.z + sunZOffset);
            sun.position = sunWorldPos;

            // ȸ���� ���� ����
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
