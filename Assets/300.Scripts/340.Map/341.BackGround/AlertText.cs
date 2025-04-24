using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AlertText : MonoBehaviour
{
    private Text txt;
    public string txtData = string.Empty;
    public string createParentName;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(TextSetting());
    }

    IEnumerator TextSetting()
    {
        yield return null;
        txtData = AlertManager.instance.AlertTextSetting(createParentName);
        txt.text = txtData;
    }
}
