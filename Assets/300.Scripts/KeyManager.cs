using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Gamepad, InputSystem
using UnityEngine.InputSystem.Controls; // ButtonControl
using System.Linq;

public class KeyManager : MonoBehaviour
{
    [SerializeField] Button[] btnKey;
    [SerializeField] Button[] btnKeyGamepad;
    [SerializeField] Text[] txtKey;
    [SerializeField] Text[] txtKeyGamepad;
    //0 : 키보드 설정 키 값 초기화, 저장
    [SerializeField] Button[] btnKeyOption;

    //0: 게임패드 패드 설정 값 초기화, 패드 키 값 저장
    [SerializeField] Button[] btnGamePadOption;

    public static KeyManager instance;


    private void Awake()
    {
        instance = this;
        txtKey = new Text[btnKey.Length];
        txtKeyGamepad = new Text[btnKeyGamepad.Length];
        for (int i = 0; i < btnKey.Length; i++)
        {
            int index = i;
            txtKey[i] = btnKey[i].transform.GetChild(0).GetComponent<Text>();
            btnKey[i].onClick.AddListener(() => StartCoroutine(OnBtnKeyCodeSetting(index)));
        }

        for (int i = 0; i < btnKeyGamepad.Length; i++)
        {
            int index = i;
            txtKeyGamepad[i] = btnKeyGamepad[i].transform.GetChild(0).GetComponent<Text>();
            btnKeyGamepad[i].onClick.AddListener(() => StartCoroutine(OnBtnGamepadCodeSetting(index)));
        }
        btnKeyOption[0].onClick.AddListener(() => OnKeyManagerBtnSetting(0));
        btnKeyOption[1].onClick.AddListener(() => OnKeyManagerBtnSetting(1));
        btnGamePadOption[0].onClick.AddListener(() => OnGamePadBtnSetting(0));
        btnGamePadOption[1].onClick.AddListener(() => OnGamePadBtnSetting(1));
        LoadKeyMappings();
    }

    #region 키보드 설정
    private IEnumerator OnBtnKeyCodeSetting(int num)
    {
        txtKey[num].text = "Press any key...";

        //  Input System 키보드 기준 대기 (아무 키가 눌릴 때 까지 기달림) , or 마우스 입력 대기
        yield return new WaitUntil(() =>
        {
            //wasPressedThisFrame 이번 프레임에 눌렸는가를 의미하는 기능
            return Keyboard.current.anyKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame ||
               Mouse.current.rightButton.wasPressedThisFrame ||
               Mouse.current.middleButton.wasPressedThisFrame;
        });

        //  눌린 키 찾기
        foreach (KeyControl key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                //KeyControl => KeyCode 로 변환하는 기능
                KeyCode unityKey = PlayerManager.ConvertKeyControlToKeyCode(key);
                txtKey[num].text = unityKey.ToString();
                SetKeyBinding((CustomKeyCode)num, unityKey);
                yield break;
            }
        }

        // 마우스 버튼도 확인
        //마우스 왼쪽 버튼을 이번 프레임에 눌렸는지 확인 눌렸다면 UI 업데이트 해당 키를 CustomKeyCode 에 등록
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            txtKey[num].text = KeyCode.Mouse0.ToString();
            SetKeyBinding((CustomKeyCode)num, KeyCode.Mouse0);
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            txtKey[num].text = KeyCode.Mouse1.ToString();
            SetKeyBinding((CustomKeyCode)num, KeyCode.Mouse1);
        }
        else if (Mouse.current.middleButton.wasPressedThisFrame)
        {
            txtKey[num].text = KeyCode.Mouse2.ToString();
            SetKeyBinding((CustomKeyCode)num, KeyCode.Mouse2);
        }

    }
    //키 중복 방지, 기존 키와 스왑 처리 수행하는 기능
    private void SetKeyBinding(CustomKeyCode customKey, KeyCode newKey)
    {
        KeyCode previousKey = GameManager.data.keyMappings[customKey].keyCode;

        // 새로운 키값이 눌렸는지 또는 기존에 잇던 키값이 눌렸는 지 확인
        foreach (var entry in GameManager.data.keyMappings)
        {
            if (entry.Value.keyCode == newKey && entry.Key != customKey)
            {
                // 키를 전에 있던 키 값이랑 교체
                GameManager.data.keyMappings[entry.Key].keyCode = previousKey;
                txtKey[(int)entry.Key].text = previousKey.ToString(); // Update the UI for swapped action

                break;
            }
        }    
        GameManager.data.keyMappings[customKey].keyCode = newKey;
        txtKey[(int)customKey].text = newKey.ToString(); //해당 UI에 설정된 키값을 업데이트함
    }

    //초기화 해줌 키보드를 기존에 설정된 값으로 되돌림
    void DefualtKeySetting()
    {
        //키 설정이 없으면 새로 만들고 있으면 키 값을 덮어씀
        void Set(CustomKeyCode key, KeyCode k)
        {
            if (!GameManager.data.keyMappings.ContainsKey(key))
                GameManager.data.keyMappings[key] = new KeyMapping { customKeyCode = key };

            GameManager.data.keyMappings[key].keyCode = k;
        }
        Set(CustomKeyCode.Left, KeyCode.LeftArrow);
        Set(CustomKeyCode.Right, KeyCode.RightArrow);
        Set(CustomKeyCode.Jump, KeyCode.Space);
        Set(CustomKeyCode.Attack, KeyCode.Z);
        Set(CustomKeyCode.Evasion, KeyCode.LeftControl);
        Set(CustomKeyCode.Equipment, KeyCode.E);
        Set(CustomKeyCode.PlayerInfo, KeyCode.P);
        Set(CustomKeyCode.Skill, KeyCode.K);
        Set(CustomKeyCode.Inventory, KeyCode.I);
        Set(CustomKeyCode.BlockKey, KeyCode.Mouse1);
        Set(CustomKeyCode.ShortcutKey1, KeyCode.Alpha1);
        Set(CustomKeyCode.ShortcutKey2, KeyCode.Alpha2);
        Set(CustomKeyCode.ShortcutKey3, KeyCode.Alpha3);
        Set(CustomKeyCode.ShortcutKey4, KeyCode.Alpha4);
        Set(CustomKeyCode.ShortcutKey5, KeyCode.Alpha5);
        Set(CustomKeyCode.ShortcutKey6, KeyCode.Alpha6);
        Set(CustomKeyCode.ShortcutKey7, KeyCode.Alpha7);
        Set(CustomKeyCode.ActionKey, KeyCode.UpArrow);
        Set(CustomKeyCode.PauseKey, KeyCode.Escape);
        Set(CustomKeyCode.Canel, KeyCode.X);

        // Ui 텍스트 값도 기존에 설정된 키보드 값으로 가져옴
        for (int i = 0; i < btnKey.Length; i++)
        {
            var mapping = GameManager.data.keyMappings[(CustomKeyCode)i];
            txtKey[i].text = mapping.keyCode.ToString(); //  keyCode만 표시
        }

        SaveKeyMappingData();
    }

    void OnKeyManagerBtnSetting(int num)
    {
        switch (num)
        {
            case 0:
                DefualtKeySetting();
                break;
            case 1:
                SaveKeyMappingData();
                break;
        }
    }

    #endregion


    #region 게임패드 설정

    void OnGamePadBtnSetting(int num)
    {
        switch (num)
        {
            case 0:
                DefaultGamepadSetting();
                break;
            case 1:
                SaveKeyMappingData();
                break;
        }
    }

    private IEnumerator OnBtnGamepadCodeSetting(int num)
    {
        txtKeyGamepad[num].text = "Press gamepad...";
        //게임 패드가 연결되어 있고 어떤 버튼이라도 이번 프레임에 눌렸는지 기다림 두가지 조건이 만족할 때 까지 코루틴 대기
        yield return new WaitUntil(() => Gamepad.current != null &&
            Gamepad.current.allControls.Any(c => c is ButtonControl b && b.wasPressedThisFrame));

        //모든 컨트롤 중에 눌린 버튼을 탐색
        foreach (var ctrl in Gamepad.current.allControls)
        {
            if (ctrl is ButtonControl btn && btn.wasPressedThisFrame)
            {
                GameManager.data.keyMappings[(CustomKeyCode)num].gamepadButton = ctrl.name;
                txtKeyGamepad[num].text = ctrl.name;
                yield break;
            }
        }
    }
    //게임 패드 버튼을 설정하지만 이미 같은 버튼이 다른 키에 등록되 있다면 스왑 처리
    private void SetGamepadBinding(CustomKeyCode customKey, string newPadButton)
    {
        //새로 설정하려는 키 가 현재 어떤 버틍를 사용하고 있었는지 저장
        string previousButton = GameManager.data.keyMappings[customKey].gamepadButton;
        //newPadButton 을 쓰고 있는 다른 키 가 있는 확인
        foreach (var entry in GameManager.data.keyMappings)
        {
            if (entry.Value.gamepadButton == newPadButton && entry.Key != customKey)
            {
                //중복된 버튼이 있으면 이전 버튼으로 스왑
                GameManager.data.keyMappings[entry.Key].gamepadButton = previousButton;
                txtKeyGamepad[(int)entry.Key].text = previousButton; // UI 업데이트
                break;
            }
        }

        GameManager.data.keyMappings[customKey].gamepadButton = newPadButton;
        txtKeyGamepad[(int)customKey].text = newPadButton;
    }

    void DefaultGamepadSetting()
    {
        void Set(CustomKeyCode key, string padBtn)
        {
            if (!GameManager.data.keyMappings.ContainsKey(key))
                GameManager.data.keyMappings[key] = new KeyMapping { customKeyCode = key };

            GameManager.data.keyMappings[key].gamepadButton = padBtn;
        }

        Set(CustomKeyCode.Left, "leftStick/left");
        Set(CustomKeyCode.Right, "leftStick/right");
        Set(CustomKeyCode.Jump, "buttonSouth");
        Set(CustomKeyCode.Attack, "buttonWest");
        Set(CustomKeyCode.Evasion, "buttonEast");
        Set(CustomKeyCode.Equipment, "rightShoulder");
        Set(CustomKeyCode.PlayerInfo, "dpad/up");
        Set(CustomKeyCode.Skill, "leftShoulder");
        Set(CustomKeyCode.Inventory, "dpad/down");
        Set(CustomKeyCode.BlockKey, "buttonNorth");
        Set(CustomKeyCode.ShortcutKey1, "dpad/left");
        Set(CustomKeyCode.ShortcutKey2, "dpad/right");
        Set(CustomKeyCode.ShortcutKey3, "leftStick/down");
        Set(CustomKeyCode.ShortcutKey4, "dpad/down");
        Set(CustomKeyCode.ShortcutKey5, "buttonEast+buttonSouth");
        Set(CustomKeyCode.ShortcutKey6, "buttonNorth+buttonSouth");
        Set(CustomKeyCode.ShortcutKey7, "buttonNorth+buttonEast");
        Set(CustomKeyCode.ActionKey, "leftStick/up");
        Set(CustomKeyCode.PauseKey, "start");
        Set(CustomKeyCode.Canel, "select");

        for (int i = 0; i < btnKey.Length; i++)
        {
            var map = GameManager.data.keyMappings[(CustomKeyCode)i];
            txtKeyGamepad[i].text = string.IsNullOrEmpty(map.gamepadButton) ? "None" : map.gamepadButton;
        }

        SaveKeyMappingData();
    }
    #endregion

    void SaveKeyMappingData()
    {
        KeyMappingsData data = new KeyMappingsData();
        data.keyMappings = new KeyMapping[GameManager.data.keyMappings.Count];

        int i = 0;
        foreach (var entry in GameManager.data.keyMappings)
        {
            data.keyMappings[i] = entry.Value;
            i++;
        }

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(GameManager.data.KeyMappingDataSavePath, json);
    }

    void LoadKeyMappings()
    {
        if (System.IO.File.Exists(GameManager.data.KeyMappingDataSavePath))
        {
            string json = System.IO.File.ReadAllText(GameManager.data.KeyMappingDataSavePath);
            KeyMappingsData data = JsonUtility.FromJson<KeyMappingsData>(json);

            GameManager.data.keyMappings.Clear(); // Clear existing mappings

            foreach (var mapping in data.keyMappings)
            {
                GameManager.data.keyMappings[mapping.customKeyCode] = mapping;
            }

            // 키보드 저장된 값을 불러와서 텍스트에 대입
            for (int i = 0; i < btnKey.Length; i++)
            {
                var mapping = GameManager.data.keyMappings[(CustomKeyCode)i];
                txtKey[i].text = mapping.keyCode.ToString(); //  keyCode만 표시
            }

            for(int i = 0; i < btnKeyGamepad.Length; i++)
            {
                var map = GameManager.data.keyMappings[(CustomKeyCode)i];
                txtKeyGamepad[i].text = string.IsNullOrEmpty(map.gamepadButton) ? "None" : map.gamepadButton;
            }
        }
        else
        {
            DefualtKeySetting(); // 파일을 못찾았을 경우 키보드 기본 상태로 가져와서 json파일로 저장
            DefaultGamepadSetting();
        }
    }
}
