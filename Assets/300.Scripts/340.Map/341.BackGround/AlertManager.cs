using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//알람의 대한 텍스트 관리 함수
public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;

    private void Awake()
    {
        instance = this;
    }

    public string AlertTextSetting(string name)
    {
        string textData = string.Empty;
        switch(name)
        {
            case "Down":
                string keyTextUp;
                string keyTextDown;

                var mappingUp = GameManager.data.keyMappings[CustomKeyCode.Up];
                var mappingDown = GameManager.data.keyMappings[CustomKeyCode.Down];

                // Up 입력 텍스트 설정
                if (Gamepad.current != null && !string.IsNullOrEmpty(mappingUp.gamepadButton))
                    keyTextUp = mappingUp.gamepadButton;
                else
                    keyTextUp = mappingUp.keyCode.ToString();

                // Down 입력 텍스트 설정
                if (Gamepad.current != null && !string.IsNullOrEmpty(mappingDown.gamepadButton))
                    keyTextDown = mappingDown.gamepadButton;
                else
                    keyTextDown = mappingDown.keyCode.ToString();

                // 텍스트 구성
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        textData = $"{keyTextUp} 키를 눌러 위로 올라가고, {keyTextDown} 키를 눌러 아래로 내려갈 수 있습니다.";
                        break;
                    case LANGUAGE.ENG:
                        textData = $"Press {keyTextUp} to climb up and {keyTextDown} to go down the ladder.";
                        break;
                }
                break;
            case "Trap":
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        textData = "함정이 있습니다 함정에 닿으면 데미지를 입기 때문에 함정의 주의해 주세요.";
                        break;
                    case LANGUAGE.ENG:
                        textData = "There are traps. Be careful of the traps because touching them will cause damage.";
                        break;
                }
                break;
            case "Alert":
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        textData = "이단 점프를 획득 후 위 지역으로 이동이 가능합니다.";
                        break;
                    case LANGUAGE.ENG:
                        textData = "There are traps. Be careful of the traps because touching them will cause damage.";
                        break;
                }
                break;
        }

        return textData;
    }
}
