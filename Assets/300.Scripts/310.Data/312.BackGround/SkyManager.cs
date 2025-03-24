using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkyManager : MonoBehaviour
{
    [SerializeField] Material[] skyMaterial;

    void Update()
    {
        UpdateSkybox();
    }

    void UpdateSkybox()
    {
        int hour = DateTime.Now.Hour;

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
}
