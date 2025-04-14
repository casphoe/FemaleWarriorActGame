using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuyPanel : MonoBehaviour
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
                txt.text = $"If you would like to purchase the product, please press the {keyText} button.";
                break;
            case LANGUAGE.KOR:
                txt.text = $"상품을 구매하실 거면 {keyText} 버튼을 입력해주세요.";
                break;
        }
    }
}
