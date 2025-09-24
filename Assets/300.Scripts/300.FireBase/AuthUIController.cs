using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Firebase.Auth;

public class AuthUIController : MonoBehaviour
{
   [Header("Panels")]
    public GameObject panelSignIn, panelSignUp, panelForgot;

    [Header("SignIn UI")]
    public TMP_InputField inEmail;
    public TMP_InputField inPassword;
    public Toggle inShowPw;
    public Toggle inRememberEmail;


    [Header("SignUp UI")]
    public TMP_InputField upEmail;
    public TMP_InputField upPassword;
    public TMP_InputField upPasswordConfirm;
    public TMP_InputField upNickname; // optional
    public Toggle upShowPw;

    [Header("Forgot UI")]
    public TMP_InputField fgEmail;

    [Header("Message")]
    public TMP_Text messageText;

    const string LAST_EMAIL_KEY = "last_email";

    private void Start()
    {
        // 이메일 기억 불러오기
        if (PlayerPrefs.HasKey(LAST_EMAIL_KEY))
        {
            inEmail.text = PlayerPrefs.GetString(LAST_EMAIL_KEY);
            inRememberEmail.isOn = true;
        }
        // 초기 패널
        ShowSignIn();
    }

    #region Panel Switch

    public void ShowSignIn()
    {
        SetAll(false);
        panelSignIn.SetActive(true);
        ClearMessage();
    }

    public void ShowSignUp()
    {
        SetAll(false);
        panelSignUp.SetActive(true);
        ClearMessage();
    }
    public void ShowForgot()
    {
        SetAll(false);
        panelForgot.SetActive(true);
        ClearMessage();
    }

    void SetAll(bool v)
    {
        panelSignIn.SetActive(v);
        panelSignUp.SetActive(v);
        panelForgot.SetActive(v);
    }
    #endregion

    #region UI Helpers

    public void TogglePassword_SignIn()
       => TogglePassword(inPassword, inShowPw.isOn);

    public void TogglePassword_SignUp()
    {
        TogglePassword(upPassword, upShowPw.isOn);
        TogglePassword(upPasswordConfirm, upShowPw.isOn);
    }

    void TogglePassword(TMP_InputField field, bool show)
    {
        field.contentType = show ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        field.ForceLabelUpdate();
    }

    void ClearMessage() => messageText.text = "";

    void Toast(string msg) { messageText.text = msg; Debug.Log(msg); }

    #endregion

    #region 이메일인지 아닌지 여부와 패스워드 강도 알아보게 하는 기능
    bool IsValidEmail(string email)
       => !string.IsNullOrWhiteSpace(email) &&
          Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    bool IsStrongPassword(string pw)
        => !string.IsNullOrWhiteSpace(pw) && pw.Length >= 6; // Firebase 최소 6자
    #endregion

    #region 버튼

    #endregion
}
