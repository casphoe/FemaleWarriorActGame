using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//�˶��� ���� �ؽ�Ʈ ���� �Լ�
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

                // Up �Է� �ؽ�Ʈ ����
                if (Gamepad.current != null && !string.IsNullOrEmpty(mappingUp.gamepadButton))
                    keyTextUp = mappingUp.gamepadButton;
                else
                    keyTextUp = mappingUp.keyCode.ToString();

                // Down �Է� �ؽ�Ʈ ����
                if (Gamepad.current != null && !string.IsNullOrEmpty(mappingDown.gamepadButton))
                    keyTextDown = mappingDown.gamepadButton;
                else
                    keyTextDown = mappingDown.keyCode.ToString();

                // �ؽ�Ʈ ����
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        textData = $"{keyTextUp} Ű�� ���� ���� �ö󰡰�, {keyTextDown} Ű�� ���� �Ʒ��� ������ �� �ֽ��ϴ�.";
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
                        textData = "������ �ֽ��ϴ� ������ ������ �������� �Ա� ������ ������ ������ �ּ���.";
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
                        textData = "�̴� ������ ȹ�� �� �� �������� �̵��� �����մϴ�.";
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
