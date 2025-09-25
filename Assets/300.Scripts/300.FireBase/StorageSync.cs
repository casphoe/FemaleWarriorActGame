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

    // 로컬 JSON 파일 경로
    public static string LocalJsonPath => Path.Combine(Application.persistentDataPath, "playerData.json");

    static StorageReference GetUserJsonRef(string uid)
        => storage.GetReference($"users/{uid}/playerData.json");

    public static async Task<bool> DownloadPlayerDataAsync(string uid)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LocalJsonPath));
            var cloudRef = GetUserJsonRef(uid);
            await cloudRef.GetFileAsync(LocalJsonPath);
            Debug.Log("[Storage] Downloaded playerData.json");
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

    public static async Task UploadPlayerDataAsync(string uid)
    {
        try
        {
            if (!File.Exists(LocalJsonPath))
            {
                Debug.LogWarning("[Storage] Local json not found; nothing to upload.");
                return;
            }

            var cloudRef = GetUserJsonRef(uid);

            // 1차: PutFileAsync (가장 간단)
            try
            {
                await cloudRef.PutFileAsync(LocalJsonPath);
                Debug.Log("[Storage] Uploaded playerData.json (file)");
                return;
            }
            catch (StorageException ex) when (ex.ErrorCode.ToString() == "ObjectNotFound")
            {
                // 일부 환경에서 버킷/레퍼런스 이슈로 PutFile이 -13010을 낼 수 있어 백업 경로로 재시도
                Debug.LogWarning("[Storage] PutFileAsync returned ObjectNotFound. Retrying with PutBytesAsync...");
            }

            // 2차: PutBytesAsync (스트림/바이트 업로드 재시도)
            byte[] bytes = File.ReadAllBytes(LocalJsonPath);
            await cloudRef.PutBytesAsync(bytes);
            Debug.Log("[Storage] Uploaded playerData.json (bytes)");
        }
        catch (StorageException ex)
        {
            Debug.LogError($"[Storage] Upload error: {ex.ErrorCode} (HTTP {ex.HttpResultCode}) {ex.Message}");
            throw;
        }
    }
}