using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour
{
    [SerializeField] Button[] btnKey;

    [SerializeField] Text[] txtKey;

    [SerializeField] Button[] btnKeyOption;

    public static KeyManager instance;

    // 매핑 딕셔너리 정의
    private Dictionary<KeyCode, CustomKeyCode> unityToCustomKeyMap = new Dictionary<KeyCode, CustomKeyCode>
{
    { KeyCode.LeftArrow, CustomKeyCode.Left },
    { KeyCode.RightArrow, CustomKeyCode.Right },
    { KeyCode.Space, CustomKeyCode.Jump },
    { KeyCode.Z, CustomKeyCode.Attack },
    { KeyCode.LeftControl, CustomKeyCode.Evasion },
    { KeyCode.E, CustomKeyCode.Equipment },
    { KeyCode.K, CustomKeyCode.Skill },
    { KeyCode.I, CustomKeyCode.Inventory },
    { KeyCode.Alpha1, CustomKeyCode.ShortcutKey1 },
    { KeyCode.Alpha2, CustomKeyCode.ShortcutKey2 },
    { KeyCode.Alpha3, CustomKeyCode.ShortcutKey3 },
    { KeyCode.Alpha4, CustomKeyCode.ShortcutKey4 },
    { KeyCode.Alpha5, CustomKeyCode.ShortcutKey5 },
    { KeyCode.Alpha6, CustomKeyCode.ShortcutKey6 },
    { KeyCode.Alpha7, CustomKeyCode.ShortcutKey7 },
    { KeyCode.UpArrow, CustomKeyCode.ActionKey },
    { KeyCode.Escape, CustomKeyCode.PauseKey },
};


    private void Awake()
    {
        instance = this;
        txtKey = new Text[btnKey.Length];
        for (int i = 0; i < btnKey.Length; i++)
        {
            txtKey[i] = btnKey[i].transform.GetChild(0).GetComponent<Text>();
            int index = i;
            btnKey[i].onClick.AddListener(() => StartCoroutine(OnBtnKeyCodeSetting(index)));
        }
        btnKeyOption[0].onClick.AddListener(() => OnKeyManagerBtnSetting(0));
        btnKeyOption[1].onClick.AddListener(() => OnKeyManagerBtnSetting(1));
        LoadKeyMappings();
    }


    private IEnumerator OnBtnKeyCodeSetting(int num)
    {
        txtKey[num].text = "Press any key...";

        yield return new WaitUntil(() => Input.anyKeyDown);

        // Get the pressed key and display it
        foreach (KeyCode unityKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(unityKey))
            {
                txtKey[num].text = unityKey.ToString();

               
                SetKeyBinding((CustomKeyCode)num, unityKey); // 키 값을 눌렸을 때 실행되는 함수
                       
                break;
            }
        }
    }

    private void SetKeyBinding(CustomKeyCode customKey, KeyCode newKey)
    {
        KeyCode previousKey = GameManager.data.keyMappings[customKey];

        // 새로운 키값이 눌렸는지 또는 기존에 잇던 키값이 눌렸는 지 확인
        foreach (var entry in GameManager.data.keyMappings)
        {
            if (entry.Value == newKey && entry.Key != customKey)
            {
                // 키를 전에 있던 키 값이랑 교체
                GameManager.data.keyMappings[entry.Key] = previousKey;
                txtKey[(int)entry.Key].text = previousKey.ToString(); // Update the UI for swapped action

                break;
            }
        }

        
        GameManager.data.keyMappings[customKey] = newKey;
        txtKey[(int)customKey].text = newKey.ToString(); //해당 UI에 설정된 키값을 업데이트함
    }

    void OnKeyManagerBtnSetting(int num)
    {
        switch(num)
        {
            case 0:
                DefualtKeySetting();
                break;
            case 1:
                SaveKeyMappingData();
                break;
        }
    }

    void SaveKeyMappingData()
    {
        KeyMappingsData data = new KeyMappingsData();
        data.keyMappings = new KeyMapping[GameManager.data.keyMappings.Count];

        int i = 0;
        foreach (var entry in GameManager.data.keyMappings)
        {
            data.keyMappings[i] = new KeyMapping
            {
                customKeyCode = entry.Key,
                keyCode = entry.Value
            };
            i++;
        }

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(GameManager.data.KeyMappingDataSavePath, json);
        Debug.Log("Key mappings saved to: " + GameManager.data.KeyMappingDataSavePath);
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
                GameManager.data.keyMappings[mapping.customKeyCode] = mapping.keyCode;
            }

            // 키보드 저장된 값을 불러와서 텍스트에 대입
            for (int i = 0; i < btnKey.Length; i++)
            {
                txtKey[i].text = GameManager.data.keyMappings[(CustomKeyCode)i].ToString();
            }          
        }
        else
        {          
            DefualtKeySetting(); // 파일을 못찾았을 경우 키보드 기본 상태로 가져와서 json파일로 저장
        }
    }
    //초기화 해줌 키보드를 기존에 설정된 값으로 되돌림
    void DefualtKeySetting()
    {
        GameManager.data.keyMappings.Clear();

        GameManager.data.keyMappings[CustomKeyCode.Left] = KeyCode.LeftArrow;
        GameManager.data.keyMappings[CustomKeyCode.Right] = KeyCode.RightArrow;
        GameManager.data.keyMappings[CustomKeyCode.Jump] = KeyCode.Space;
        GameManager.data.keyMappings[CustomKeyCode.Attack] = KeyCode.Z;
        GameManager.data.keyMappings[CustomKeyCode.Evasion] = KeyCode.LeftControl;
        GameManager.data.keyMappings[CustomKeyCode.Equipment] = KeyCode.E;
        GameManager.data.keyMappings[CustomKeyCode.Skill] = KeyCode.K;
        GameManager.data.keyMappings[CustomKeyCode.Inventory] = KeyCode.I;
        GameManager.data.keyMappings[CustomKeyCode.ShortcutKey1] = KeyCode.Alpha1;
        GameManager.data.keyMappings[CustomKeyCode.ShortcutKey2] = KeyCode.Alpha2;
        GameManager.data.keyMappings[CustomKeyCode.ShortcutKey3] = KeyCode.Alpha3;
        GameManager.data.keyMappings[CustomKeyCode.ShortcutKey4] = KeyCode.Alpha4;
        GameManager.data.keyMappings[CustomKeyCode.ShortcutKey5] = KeyCode.Alpha5;
        GameManager.data.keyMappings[CustomKeyCode.ShortcutKey6] = KeyCode.Alpha6;
        GameManager.data.keyMappings[CustomKeyCode.ShortcutKey7] = KeyCode.Alpha7;
        GameManager.data.keyMappings[CustomKeyCode.ActionKey] = KeyCode.UpArrow;
        GameManager.data.keyMappings[CustomKeyCode.PauseKey] = KeyCode.Escape;

        // Ui 텍스트 값도 기존에 설정된 키보드 값으로 가져옴
        for (int i = 0; i < btnKey.Length; i++)
        {
            txtKey[i].text = GameManager.data.keyMappings[(CustomKeyCode)i].ToString();
        }

        SaveKeyMappingData();
    }
}
