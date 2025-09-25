using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Storage;

public static class StorageSync
{
    // 항상 앱의 실제 버킷으로 Storage 인스턴스를 만든다
    static FirebaseStorage storage
    {
        get
        {
            var app = FirebaseApp.DefaultInstance;
            var bucket = app?.Options?.StorageBucket; // 예: "your-project.appspot.com"
            if (string.IsNullOrEmpty(bucket))
            {
                Debug.LogWarning("[Storage] Options.StorageBucket is empty. Using DefaultInstance.");
                return FirebaseStorage.DefaultInstance;
            }
            // gs:// 접두사로 정확한 버킷 지정
            return FirebaseStorage.GetInstance($"gs://{bucket}");
        }
    }

    // UID별 로컬 경로
    public static string LocalJsonPath(string uid)
     => Path.Combine(Application.persistentDataPath, "users", uid, "playerData.json");


    static StorageReference GetUserJsonRef(string uid)
       => storage.GetReference($"users/{uid}/playerData.json");

    // 클라우드 → 로컬(UID별 경로) → PM.Load
    public static async Task<bool> DownloadPlayerDataAsync(string uid)
    {
        string localPath = LocalJsonPath(uid);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(localPath));
            var cloudRef = GetUserJsonRef(uid);
            await cloudRef.GetFileAsync(localPath);
            Debug.Log($"[Storage] Downloaded {localPath}");
            PM.LoadPlayerData();
            return true;
        }
        catch (StorageException ex)
        {
            // 파일이 처음이라 존재하지 않는 경우(HTTP 404)
            if (ex.ErrorCode.ToString() == "ObjectNotFound" || ex.HttpResultCode == 404)
            {
                Debug.Log("[Storage] No cloud save yet. First time user.");
                return false;
            }

            Debug.LogError($"[Storage] Download error: {ex.ErrorCode} (HTTP {ex.HttpResultCode}) {ex.Message}");
            throw;
        }
    }
    // 로컬(UID별 경로) → 클라우드
    public static async Task UploadPlayerDataAsync(string uid)
    {
        string localPath = LocalJsonPath(uid);
        try
        {
            if (!File.Exists(localPath))
            {
                Debug.LogWarning($"[Storage] Local json not found: {localPath}");
                return;
            }

            var cloudRef = GetUserJsonRef(uid);

            // 기본 업로드
            try
            {
                await cloudRef.PutFileAsync(localPath);
                Debug.Log("[Storage] Uploaded (file)");
                return;
            }
            catch (StorageException ex) when (ex.ErrorCode.ToString() == "ObjectNotFound")
            {
                // 일부 환경에서 PutFile이 오동작시 바이트로 재시도
                Debug.LogWarning("[Storage] PutFileAsync -> ObjectNotFound. Retrying PutBytesAsync...");
            }

            // 2차: PutBytesAsync (스트림/바이트 업로드 재시도)
            byte[] bytes = File.ReadAllBytes(localPath);
            await cloudRef.PutBytesAsync(bytes);
            Debug.Log("[Storage] Uploaded (bytes)");
        }
        catch (StorageException ex)
        {
            Debug.LogError($"[Storage] Upload error: {ex.ErrorCode} (HTTP {ex.HttpResultCode}) {ex.Message}");
            throw;
        }
    }
}