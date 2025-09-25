using Firebase;
using Firebase.Auth;
using UnityEngine;
using System.Threading.Tasks;
using System.Diagnostics;

public class FireBaseManager : MonoBehaviour
{
    public static FireBaseManager instance;
    public static bool IsReady { get; private set; }
    static TaskCompletionSource<bool> _readyTcs = new TaskCompletionSource<bool>();

    async void Awake()
    {
        var dep = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dep == DependencyStatus.Available)
        {
            var o = FirebaseApp.DefaultInstance.Options;
            UnityEngine.Debug.Log($"[Firebase] ProjectId={o.ProjectId} AppId={o.AppId} StorageBucket={o.StorageBucket}");
            IsReady = true;
            _readyTcs.TrySetResult(true);
        }
        else
        {
            UnityEngine.Debug.LogError($"[Firebase] Dependency error: {dep}");
        }

        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    static FirebaseAuth auth => FirebaseAuth.DefaultInstance;

    // 현재 유저
    public static FirebaseUser CurrentUser => auth.CurrentUser;

    // 익명 로그인 보장(선택: 첫 진입 시 호출)
    public static async Task<FirebaseUser> EnsureAnonymousAsync()
    {
        if (auth.CurrentUser != null) return auth.CurrentUser;
        var result = await auth.SignInAnonymouslyAsync();
        return result.User;
    }

    // 회원가입(이메일/비번) — AuthResult → User 반환
    public static async Task<FirebaseUser> SignUpEmailAsync(string email, string password)
    {
        var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
        return result.User;
    }

    // (중요) 익명 → 정식 계정 “업그레이드”(UID 유지)
    public static async Task<FirebaseUser> LinkEmailPasswordAsync(string email, string password)
    {
        var cred = EmailAuthProvider.GetCredential(email, password);
        var result = await auth.CurrentUser.LinkWithCredentialAsync(cred);
        return result.User;
    }

    // 로그인
    public static async Task<FirebaseUser> SignInEmailAsync(string email, string password)
    {
        var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
        return result.User;
    }

    // 비밀번호 재설정 메일 발송
    public static Task SendPasswordResetAsync(string email)
        => auth.SendPasswordResetEmailAsync(email);

    public static void SignOut() => auth.SignOut();

    public static Task WaitUntilReady() => _readyTcs.Task;

    public static async Task SignOutAndWaitAsync(int timeoutMs = 3000)
    {
        var a = FirebaseAuth.DefaultInstance;

        // 1) 로그아웃 트리거
        a.SignOut();

        // 2) 상태 전파 완료까지 잠깐 대기 (최대 timeoutMs)
        var sw = Stopwatch.StartNew();
        while (a.CurrentUser != null && sw.ElapsedMilliseconds < timeoutMs)
        {
            await Task.Yield(); // 다음 프레임로 양보(메인 루프에서 콜백 처리)
        }

        if (a.CurrentUser != null)
            UnityEngine.Debug.LogWarning("[Firebase] SignOut not observed yet (timeout). Proceeding anyway.");
    }
}
