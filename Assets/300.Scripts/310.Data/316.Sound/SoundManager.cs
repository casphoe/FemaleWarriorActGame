using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사운드 관리 함수
public class SoundManager : MonoBehaviour
{
    //환경음 오디오 사운드를 풀링 해서 겹쳐도 깨지지 않게 하기 위해서 사용
    [Header("SFX")]
    public AudioSource audioSourcePrefab; // 풀링할 AudioSource 프리팹
    public int poolSize = 30;
    
    
    public List<Sound> sfxClips = new List<Sound>(); //효과음 재생 리스트
    private List<AudioSource> sfxPool = new List<AudioSource>(); // AudioSource 풀
    private Dictionary<string, AudioClip> sfxDict; // 이름으로 clip을 찾는 사전

    public static SoundManager Instance;


    private void Awake()
    {
        Instance = this;
        InitializeSounds();
        CreateSfxAudioPool();
    }
    //효과음 사전에 대한 함수
    void InitializeSounds()
    {
        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var sound in sfxClips)
        {
            //같은 이름이 여러번 들어오는 현상을 막아주기 위해서 ContainsKey 로 체크함
            if (!sfxDict.ContainsKey(sound.name))
                sfxDict.Add(sound.name, sound.clip);
        }
    }
    //오디오 풀 만드는 함수
    void CreateSfxAudioPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newSource = Instantiate(audioSourcePrefab, transform.GetChild(1).transform);
            newSource.playOnAwake = false;
            sfxPool.Add(newSource);
        }
    }

    public void PlaySFX(string name)
    {
        if (!sfxDict.ContainsKey(name))
        {
            Debug.LogWarning("SFX not found: " + name);
            return;
        }

        AudioSource availableSource = GetAvailableAudioSource();
        if (availableSource != null)
        {
            availableSource.clip = sfxDict[name];
            availableSource.Play();
        }
    }

    AudioSource GetAvailableAudioSource()
    {
        foreach (var source in sfxPool)
        {
            if (!source.isPlaying)
                return source;
        }

        // 다 사용 중이면 첫 번째 꺼 덮어씀 (혹은 null 리턴해도 됨)
        return sfxPool[0];
    }
}


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}