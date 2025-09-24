using Firebase;
using Firebase.Auth;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Refs")]
    [SerializeField] CanvasGroup canvasGroup;    // Panel_Toast의 CanvasGroup

    [Header("Toast Message")]
    public TMP_Text messageText;

    [Header("Timing (sec)")]
    [SerializeField, Min(0f)] float fadeIn = 0.15f;
    [SerializeField, Min(0f)] float hold = 1.40f;
    [SerializeField, Min(0f)] float fadeOut = 0.35f;

    [Header("Layout")]
    [SerializeField] bool blockRaycastWhileVisible = false;

    Coroutine _co;

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
        // 토스트 초기 상태 숨김
        HideImmediate();
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
    #endregion

    #region 이메일인지 아닌지 여부와 패스워드 강도 알아보게 하는 기능
    bool IsValidEmail(string email)
       => !string.IsNullOrWhiteSpace(email) &&
          Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    bool IsStrongPassword(string pw)
        => !string.IsNullOrWhiteSpace(pw) && pw.Length >= 6; // Firebase 최소 6자
    #endregion

    #region 버튼
    //로그인 버튼
    public async void OnClick_Login()
    {
        var email = inEmail.text.Trim();
        var pw = inPassword.text;

        if (!IsValidEmail(email)) { Toast("이메일 형식을 확인하세요."); return; }
        if (!IsStrongPassword(pw)) { Toast("비밀번호는 6자 이상이어야 합니다."); return; }

        try
        {
            var auth = FirebaseAuth.DefaultInstance;

            // 1) 이 이메일로 가입된 제공자 목록 조회
            var providers = await auth.FetchProvidersForEmailAsync(email);
            if (providers == null || !providers.Any())
            {
                Toast("가입되지 않은 계정입니다.");
                return;
            }

            // 2) 존재하면 로그인 시도
            var user = await FireBaseManager.SignInEmailAsync(email, pw);

            if (inRememberEmail.isOn) PlayerPrefs.SetString(LAST_EMAIL_KEY, email);
            else PlayerPrefs.DeleteKey(LAST_EMAIL_KEY);

            Toast("로그인 성공");
        }
        catch (FirebaseException ex)
        {
            ToastAuthError(ex);
        }
    }

    //회원가입 버튼
    public async void OnClick_SignUp()
    {
        var email = upEmail.text.Trim();
        var pw = upPassword.text;
        var pw2 = upPasswordConfirm.text;
        var nick = upNickname.text.Trim();

        if (!IsValidEmail(email)) { Toast("이메일 형식을 확인하세요."); return; }
        if (!IsStrongPassword(pw)) { Toast("비밀번호는 6자 이상이어야 합니다."); return; }
        if (pw != pw2) { Toast("비밀번호가 일치하지 않습니다."); return; }

        try
        {
            var auth = FirebaseAuth.DefaultInstance;

            FirebaseUser user;

            if (auth.CurrentUser != null && auth.CurrentUser.IsAnonymous)
            {
                // 익명 → 이메일/비번으로 "링크" (UID 유지)
                var cred = EmailAuthProvider.GetCredential(email, pw);
                var result = await auth.CurrentUser.LinkWithCredentialAsync(cred);
                user = result.User;
            }
            else
            {
                // 일반 신규 회원가입
                var result = await auth.CreateUserWithEmailAndPasswordAsync(email, pw);
                user = result.User;
            }

            if (!string.IsNullOrEmpty(nick))
                await user.UpdateUserProfileAsync(new UserProfile { DisplayName = nick });

            Toast("회원가입 완료!");

            ShowSignIn();
        }
        catch (FirebaseException ex)
        {
            ToastAuthError(ex);
        }
    }

    //비밀번호 재설정 메일
    public async void OnClick_SendRequestPassword()
    {
        var email = fgEmail.text.Trim();
        if (!IsValidEmail(email)) { Toast("이메일 형식을 확인하세요."); return; }
        try
        {
            await FireBaseManager.SendPasswordResetAsync(email);
            Toast("재설정 메일을 보냈습니다. 메일함을 확인하세요.");
            ShowSignIn();
        }
        catch (FirebaseException ex)
        {
            ToastAuthError(ex);
        }
    }
    //취소 및 되돌아가기
    public void OnClick_Back()
    {
        ShowSignIn();
    }
    #endregion

    #region Error Mapping
    string MapAuthError(FirebaseException ex)
    {
        var code = (AuthError)ex.ErrorCode;
        switch (code)
        {
            case AuthError.InvalidEmail: return "유효하지 않은 이메일입니다.";
            case AuthError.WrongPassword: return "비밀번호가 올바르지 않습니다.";
            case AuthError.UserNotFound: return "가입되지 않은 계정입니다.";
            case AuthError.EmailAlreadyInUse: return "이미 사용 중인 이메일입니다.";
            case AuthError.WeakPassword: return "비밀번호가 너무 약합니다(6자 이상 권장).";
            case AuthError.MissingEmail: return "이메일을 입력하세요.";
            case AuthError.MissingPassword: return "비밀번호를 입력하세요.";
            case AuthError.TooManyRequests: return "요청이 너무 많습니다. 잠시 후 다시 시도하세요.";
            default: return $"로그인/가입 실패: {code} ({ex.ErrorCode})";
        }
    }

    void ToastAuthError(System.Exception e)
    {
        if (e is FirebaseException fex)
            Toast(MapAuthError(fex));
        else
            Toast($"문제가 발생했습니다: {e.Message}");
    }
    #endregion

    #region 토스트
    void HideImmediate()
    {
        if (!canvasGroup) return;
        canvasGroup.gameObject.SetActive(false);
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        if (messageText) messageText.text = "";
    }

    // 어디서든 호출: Toast("문구"), Toast("문구", 2.0f)
    public void Toast(string msg, float? holdOverride = null)
    {
        if (!canvasGroup || !messageText) { Debug.Log(msg); return; }
        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(CoToast(msg, holdOverride));
    }

    System.Collections.IEnumerator CoToast(string msg, float? holdOverride)
    {
        messageText.text = msg;
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = blockRaycastWhileVisible;
        canvasGroup.interactable = false;

        // Fade In
        yield return FadeTo(1f, fadeIn);

        // Hold
        float h = holdOverride.HasValue ? Mathf.Max(0f, holdOverride.Value) : hold;
        if (h > 0f) yield return new WaitForSecondsRealtime(h);

        // Fade Out
        yield return FadeTo(0f, fadeOut);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.gameObject.SetActive(false);
        _co = null;
        if (msg == "로그인 성공")
        {
            SceneManager.LoadScene(1);
        }
    }

    System.Collections.IEnumerator FadeTo(float target, float duration)
    {
        if (duration <= 0f) { canvasGroup.alpha = target; yield break; }
        float start = canvasGroup.alpha, t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // 일시정지 중에도 보이게
            canvasGroup.alpha = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        canvasGroup.alpha = target;
    }
    #endregion
}
