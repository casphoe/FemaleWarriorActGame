using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlessText : MonoBehaviour
{
    private Text txt;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }

    private void OnEnable()
    {
        switch (GameManager.data.lanauge)
        {
            case LANGUAGE.ENG:
                txt.text = "If you want to bless or teleport, Please enter the " + GameManager.data.keyMappings[CustomKeyCode.ActionKey] + " button";
                break;
            case LANGUAGE.KOR:
                txt.text = "축복 또는 텔레포트를 원하시면 " + GameManager.data.keyMappings[CustomKeyCode.ActionKey] + " 버튼을 입력해주세요.";
                break;
        }
    }
}
