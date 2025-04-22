using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // Gamepad, InputSystem

public class GoddessStatuesMapIcon : MonoBehaviour
{
    public string statueID;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI moveText;
    public RectTransform iconTransform;
    public MapType mapType;
    public int currentMapNum;

    string korStr;
    string engStr;
    string moveKorStr;
    string moveEngStr;

    public void Setup(string id, string displayNameKor, string displayNameEng, MapType type, int mapNum)
    {
        statueID = id;
        this.mapType = type;
        currentMapNum = mapNum;

        var keyMapping = GameManager.data.keyMappings[CustomKeyCode.Attack];
        string keyText = "";

        keyText = keyMapping.keyCode.ToString();
        switch (GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                korStr = displayNameKor;
                nameText.text = korStr;
                if (Gamepad.current != null)
                {
                    moveKorStr = "텔레포트 : " + keyMapping.gamepadButton;
                }
                else
                {
                    moveKorStr = "텔레포트 : " + keyText;
                }

                switch (type)
                {
                    case MapType.GoddessStatue:
                        moveText.text = moveKorStr;
                        break;
                }
                break;
            case LANGUAGE.ENG:
                engStr = displayNameEng;
                nameText.text = engStr;
                if (Gamepad.current != null)
                {
                    moveEngStr = "TelePort : " + keyMapping.gamepadButton;
                }
                else
                {
                    moveEngStr = "TelePort : " + keyText;
                }

                switch (type)
                {
                    case MapType.GoddessStatue:
                        moveText.text = moveEngStr;
                        break;
                }
                break;
        }
    }

    public void LanaugeChange()
    {
        switch (GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:            
                nameText.text = korStr;
                break;
            case LANGUAGE.ENG:            
                nameText.text = engStr;
                break;
        }
    }
}
