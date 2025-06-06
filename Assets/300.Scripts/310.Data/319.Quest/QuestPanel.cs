﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    private Text txt;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }

    private void OnEnable()
    {
        string keyText;

        var mapping = GameManager.data.keyMappings[CustomKeyCode.ActionKey];

        // 게임패드 연결되어 있고, 매핑된 버튼이 있다면
        if (Gamepad.current != null && !string.IsNullOrEmpty(mapping.gamepadButton))
        {
            keyText = mapping.gamepadButton;
        }
        else
        {
            keyText = mapping.keyCode.ToString();
        }


        switch (GameManager.data.lanauge)
        {
            case LANGUAGE.ENG:
                txt.text = $"If you wish to accept or complete a quest, please press the {keyText} button.";
                break;
            case LANGUAGE.KOR:
                txt.text = $"퀘스트를 수락하거나 완료하고 싶으면 {keyText} 버튼을 입력해주세요.";
                break;
        }
    }
}
