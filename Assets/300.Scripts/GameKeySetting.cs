using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameKeySetting : MonoBehaviour
{
    [Header("0 : 키설정, 1 : 게임패드")]
    [SerializeField] Button[] btnUi;

    [Header("키 설정의 따른 버튼들")]
    [SerializeField] Button[] btnKey;

    [Header("키 설정에 따른 키 코드 값 텍스트")]
    Text[] txtKeyCode;

    [Header("게임 패드의 따른 버튼들")]
    [SerializeField] Button[] btnGamePad;

    [Header("게임 패드에 따른 패드 값 텍스트")]
    Text[] txtGamePad;

    [Header("0  : 키보드 설정 기존 데이터로 되돌리는 키 , 1 : 설정된 키 값을 저장하는 값")]
    [SerializeField] Button[] btnKeySetting;

    [Header("0  : 게임 패드 기존 데이터로 되돌리는 키, 1 : 설정된 게임 패드 값을 저장하는 값")]
    [SerializeField] Button[] btnGamePadSetting;

    private void Start()
    {
        txtKeyCode = new Text[btnKey.Length];
        txtGamePad = new Text[btnGamePad.Length];
    }
}