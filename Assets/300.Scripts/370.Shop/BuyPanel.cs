using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        switch(GameManager.data.lanauge)
        {
            case LANGUAGE.ENG:
                txt.text = "If you would like to purchase the product, Please enter the button " + GameManager.data.keyMappings[CustomKeyCode.ActionKey];
                break;
            case LANGUAGE.KOR:
                txt.text = "상품을 구매하실 거면 " + GameManager.data.keyMappings[CustomKeyCode.ActionKey] + " 버튼을 입력해주세요.";
                break;
        }
    }
}
