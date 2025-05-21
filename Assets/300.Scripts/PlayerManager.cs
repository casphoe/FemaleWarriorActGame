using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using static System.Runtime.CompilerServices.RuntimeHelpers;
//플레이어를 관리하는 함수
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public PlayerData player;

    public bool isInvisble;
    public bool isJump;
    public bool isGround;
    public bool IsDead;

    public bool isBuy = false; //캐릭터가 Npc 근처에 있을 경우
    public bool isPause = false;
    public bool isState = false; //여신상 근처에 캐릭터가 있을 경우
    public bool isInventroy = false;
    public bool isEquipment = false;
    public bool isSkillPage = false;
    public bool isDownAttacking = false;
    public bool isAiming = false;
    public bool isStun = false;
    public bool isGuarding = false;
    public bool isPlayerOnLadder = false;
    public bool isQuest = false;
    public bool isPlayerInfo = false;

    public bool isInvincibility = false;

    //0 : buy, 1 : 판매
    public int selectShop = -1;
    // 0 : 아이템, 1 : 물약
    public int shopNum = -1;

    public int itemShopNum = -1;

    public int[] hpPotionCount = new int[3];
    public int[] staminaPotionCount = new int[3];

    private void Awake()
    {
        // PlayerManager가 존재 하는지 있는지 확인
        if (instance == null)
        {
            instance = this;
            PM.LoadPlayerData();          
            DontDestroyOnLoad(this); // Keep this object between scene loads
        }
        else if (instance != this)
        {
            //두 개 이상 씬에 존재할 경우 삭제
            Destroy(gameObject);
        }
    }

    public void LevelUp()
    {
        if (player.currentExp >= player.levelUpExp)
        {
            player.currentExp = player.levelUpExp - player.currentExp;
            player.level += 1;
            player.SetLevel(player.level);
        }
    }

    public void PassivePlayerStatSkillSetting(int num, float hp, float stamina, float attackUp, float defenceUp, float crictleRateUp, float crictleDmgUp,
       float hpRestoration, float StaminaRestoration)
    {
        switch (num)
        {
            case 0: //증가
                player.hp += hp;
                player.stamina += stamina;
                Player.instance.currentHp += hp;
                Player.instance.currentStamina += stamina;
                player.attack += attackUp;
                player.defense += defenceUp;
                player.critcleRate += crictleRateUp;
                player.critcleDmg += crictleDmgUp;
                player.hpAutoRestoration = hpRestoration;
                player.staminaAutoRestoration += StaminaRestoration;
                break;
        }
        GameCanvas.instance.SliderEquipChange();
    }

    #region 적 처치 시 경험치 , 돈 획득 함수
    public void AddExp(int baseExp)
    {
        int bonusExp = Mathf.FloorToInt(baseExp * (player.expUp / 100f));
        int totalExp = baseExp + bonusExp;

        player.currentExp += totalExp;

        LevelUp(); //레벨 업 체크

        GameCanvas.instance.LevelAndExpSetting();
    }

    public void AddMoney(int baseMoney)
    {
        int bonusMoney = Mathf.FloorToInt(baseMoney * (player.moneyUp / 100f));
        int totalMoney = baseMoney + bonusMoney;

        player.money += totalMoney;

        GameCanvas.instance.MoneySetting();
    }
    #endregion

    #region 보물상자 열었을 때 체력증가, 스태미나 증가

    #endregion

    #region 키보드 & 게임패드 입력 시 실행 함수
    public static bool GetCustomKeyDown(CustomKeyCode key)
    {

        if (!GameManager.data.keyMappings.ContainsKey(key))
            return false;
        //유저가 커스텀으로 지정한 키값의 매핑 정보를 불려옴
        var map = GameManager.data.keyMappings[key];

        // --- 키보드 & 마우스 입력 ---
        bool keyboardPressed = false;
        //현재 키보드 디바이스가 존재하는 지 여부 확인 &&  CustomKeyCode에 키보드 키를 설정했는지 확인
        if (map.keyCode != KeyCode.None)
        {
            if (Keyboard.current != null)
            {
                // keycode -> keyControl로 변환하는 기능 (ex : KeyCode.Space → Keyboard.current.spaceKey)
                var unityKey = ConvertKeyCode(map.keyCode);
                if (unityKey != null)
                {
                    //현재 프레임에서 처음으로 눌린 경우 true를 반환 지속적으로 누르고 있는중이거나 이전 프레임에 눌렷던 키는 false
                    keyboardPressed = unityKey.wasPressedThisFrame;
                }
            }

            // 마우스 입력 처리
            // 마우스가 현재 연결되어 있는지 확인
            if (Mouse.current != null)
            {
                switch (map.keyCode)
                {
                    //마우스 왼쪽 버튼 이번 프레임에서 처음 눌렸는지 확인 , true 지금 막 눌름, false 안 눌렸거나 이미 누르고 있는 상태
                    case KeyCode.Mouse0:
                        keyboardPressed = Mouse.current.leftButton.wasPressedThisFrame;
                        break;
                    // 마우스 오른쪽 버튼 클릭 감지
                    case KeyCode.Mouse1:
                        keyboardPressed = Mouse.current.rightButton.wasPressedThisFrame;
                        break;
                    //마우스 휠 버튼 클릭 감지
                    case KeyCode.Mouse2:
                        keyboardPressed = Mouse.current.middleButton.wasPressedThisFrame;
                        break;
                }
            }
        }

        //  게임패드 입력 처리
        bool gamepadPressed = false;
        //현재 게임패드가 연결되어있는지 확인 && map.gamepadButton 에 설정된 패드 값이 있는지 확인
        if (!string.IsNullOrEmpty(map.gamepadButton) && Gamepad.current != null)
        {
            //복합 입력 처리하기 위해서 사용(ex : "buttonSouth+buttonWest" → ["buttonSouth", "buttonWest"] )
            string[] buttons = map.gamepadButton.Split('+');
            if (buttons.Length > 1)
            {
                gamepadPressed = true;
                foreach (string btnName in buttons)
                {
                    var control = Gamepad.current[btnName];
                    //해당 컨트롤 버튼 타입인지 확인
                    if (Gamepad.current.TryGetChildControl<ButtonControl>(btnName) is ButtonControl button)
                    {
                        if (!button.wasPressedThisFrame)
                        {
                            gamepadPressed = false;
                            break;
                        }
                    }
                    else
                    {
                        gamepadPressed = false;
                        break;
                    }
                }
            }
            else
            {
                if (Gamepad.current.TryGetChildControl<ButtonControl>(map.gamepadButton) is ButtonControl button)
                    gamepadPressed = button.wasPressedThisFrame;
            }
        }

        return keyboardPressed || gamepadPressed;
    }

    //유저가 설정된 키 가 지금 계속 누르고 있는 상태인지 확인
    public static bool GetCustomKey(CustomKeyCode key)
    {
        if (!GameManager.data.keyMappings.ContainsKey(key))
            return false;

        var map = GameManager.data.keyMappings[key];

        //  키보드 유지 입력
        bool keyboardHeld = false;
        if (map.keyCode != KeyCode.None && Keyboard.current != null)
        {
            var unityKey = ConvertKeyCode(map.keyCode);
            if (unityKey != null)
                //해당 키가 현재 계속 누르고 있는 상태인지 확인
                keyboardHeld = unityKey.isPressed;
        }

        //  게임패드 유지 입력
        bool gamepadHeld = false;
        if (!string.IsNullOrEmpty(map.gamepadButton) && Gamepad.current != null)
        {
            string[] buttons = map.gamepadButton.Split('+');
            if (buttons.Length > 1)
            {
                gamepadHeld = true;
                foreach (string btn in buttons)
                {
                    var control = Gamepad.current[btn];
                    if (control is ButtonControl button)
                    {
                        if (!button.isPressed)
                        {
                            gamepadHeld = false;
                            break;
                        }
                    }
                    else
                    {
                        gamepadHeld = false;
                        break;
                    }
                }
            }
            else
            {
                var control = Gamepad.current[map.gamepadButton];
                if (control is ButtonControl button)
                    gamepadHeld = button.isPressed;
            }
        }

        return keyboardHeld || gamepadHeld;
    }

    public static bool GetCustomKeyUp(CustomKeyCode key)
    {
        if (!GameManager.data.keyMappings.ContainsKey(key))
            return false;

        var map = GameManager.data.keyMappings[key];

        //  키보드 릴리즈 입력
        bool keyboardReleased = false;
        if (map.keyCode != KeyCode.None && Keyboard.current != null)
        {
            var unityKey = ConvertKeyCode(map.keyCode);
            if (unityKey != null)
                keyboardReleased = unityKey.wasReleasedThisFrame;
        }

        //  게임패드 릴리즈 입력
        bool gamepadReleased = false;
        if (!string.IsNullOrEmpty(map.gamepadButton) && Gamepad.current != null)
        {
            string[] buttons = map.gamepadButton.Split('+');
            if (buttons.Length > 1)
            {
                gamepadReleased = true;
                foreach (string btn in buttons)
                {
                    var control = Gamepad.current[btn];
                    if (control is ButtonControl button)
                    {
                        if (!button.wasReleasedThisFrame)
                        {
                            gamepadReleased = false;
                            break;
                        }
                    }
                    else
                    {
                        gamepadReleased = false;
                        break;
                    }
                }
            }
            else
            {
                var control = Gamepad.current[map.gamepadButton];
                if (control is ButtonControl button)
                    gamepadReleased = button.wasReleasedThisFrame;
            }
        }

        return keyboardReleased || gamepadReleased;
    }

    // Unity KeyCode를 -> InputSystem KeyControl로 변환
    public static KeyControl ConvertKeyCode(KeyCode keyCode)
    {
        //키보드 디바이스가 연결되어 있지 않으면 null값을 반환
        if (Keyboard.current == null) //Keyboard.current : UnityEngine.InputSystem.Keyboard 클래스에서 현재 키보드를 나타냄
            return null;

        return keyCode switch
        {
            KeyCode.Space => Keyboard.current.spaceKey,
            KeyCode.LeftArrow => Keyboard.current.leftArrowKey,
            KeyCode.RightArrow => Keyboard.current.rightArrowKey,
            KeyCode.UpArrow => Keyboard.current.upArrowKey,
            KeyCode.DownArrow => Keyboard.current.downArrowKey,
            KeyCode.Z => Keyboard.current.zKey,
            KeyCode.X => Keyboard.current.xKey,
            KeyCode.C => Keyboard.current.cKey,
            KeyCode.V => Keyboard.current.vKey,
            KeyCode.A => Keyboard.current.aKey,
            KeyCode.S => Keyboard.current.sKey,
            KeyCode.D => Keyboard.current.dKey,
            KeyCode.W => Keyboard.current.wKey,
            KeyCode.E => Keyboard.current.eKey,
            KeyCode.K => Keyboard.current.kKey,
            KeyCode.I => Keyboard.current.iKey,
            KeyCode.P => Keyboard.current.pKey,
            KeyCode.Q => Keyboard.current.qKey,
            KeyCode.R => Keyboard.current.rKey,
            KeyCode.LeftControl => Keyboard.current.leftCtrlKey,
            KeyCode.RightControl => Keyboard.current.rightCtrlKey,
            KeyCode.LeftShift => Keyboard.current.leftShiftKey,
            KeyCode.RightShift => Keyboard.current.rightShiftKey,
            KeyCode.Alpha0 => Keyboard.current.digit0Key,
            KeyCode.Alpha1 => Keyboard.current.digit1Key,
            KeyCode.Alpha2 => Keyboard.current.digit2Key,
            KeyCode.Alpha3 => Keyboard.current.digit3Key,
            KeyCode.Alpha4 => Keyboard.current.digit4Key,
            KeyCode.Alpha5 => Keyboard.current.digit5Key,
            KeyCode.Alpha6 => Keyboard.current.digit6Key,
            KeyCode.Alpha7 => Keyboard.current.digit7Key,
            KeyCode.Alpha8 => Keyboard.current.digit8Key,
            KeyCode.Alpha9 => Keyboard.current.digit9Key,
            KeyCode.Escape => Keyboard.current.escapeKey,
            KeyCode.Tab => Keyboard.current.tabKey,
            KeyCode.Backspace => Keyboard.current.backspaceKey,
            KeyCode.Return => Keyboard.current.enterKey,
            KeyCode.Delete => Keyboard.current.deleteKey,
            KeyCode.Insert => Keyboard.current.insertKey,
            _ => null // 매핑 안된 키는 null 반환
        };
    }
    //InputSystem 의 KeyControl  -> Unity  KeyCode로 변환
    public static KeyCode ConvertKeyControlToKeyCode(KeyControl keyControl)
    {
        if (keyControl == null) return KeyCode.None;

        // name을 기준으로 변환
        return keyControl.name switch
        {
            "space" => KeyCode.Space,
            "leftArrow" => KeyCode.LeftArrow,
            "rightArrow" => KeyCode.RightArrow,
            "upArrow" => KeyCode.UpArrow,
            "downArrow" => KeyCode.DownArrow,
            "a" => KeyCode.A,
            "b" => KeyCode.B,
            "c" => KeyCode.C,
            "d" => KeyCode.D,
            "e" => KeyCode.E,
            "f" => KeyCode.F,
            "g" => KeyCode.G,
            "h" => KeyCode.H,
            "i" => KeyCode.I,
            "j" => KeyCode.J,
            "k" => KeyCode.K,
            "l" => KeyCode.L,
            "m" => KeyCode.M,
            "n" => KeyCode.N,
            "o" => KeyCode.O,
            "p" => KeyCode.P,
            "q" => KeyCode.Q,
            "r" => KeyCode.R,
            "s" => KeyCode.S,
            "t" => KeyCode.T,
            "u" => KeyCode.U,
            "v" => KeyCode.V,
            "w" => KeyCode.W,
            "x" => KeyCode.X,
            "y" => KeyCode.Y,
            "z" => KeyCode.Z,
            "digit0" => KeyCode.Alpha0,
            "digit1" => KeyCode.Alpha1,
            "digit2" => KeyCode.Alpha2,
            "digit3" => KeyCode.Alpha3,
            "digit4" => KeyCode.Alpha4,
            "digit5" => KeyCode.Alpha5,
            "digit6" => KeyCode.Alpha6,
            "digit7" => KeyCode.Alpha7,
            "digit8" => KeyCode.Alpha8,
            "digit9" => KeyCode.Alpha9,
            "enter" => KeyCode.Return,
            "escape" => KeyCode.Escape,
            "backspace" => KeyCode.Backspace,
            "tab" => KeyCode.Tab,
            "leftCtrl" => KeyCode.LeftControl,
            "rightCtrl" => KeyCode.RightControl,
            "leftShift" => KeyCode.LeftShift,
            "rightShift" => KeyCode.RightShift,
            _ => KeyCode.None
        };
    }

    #endregion
}
