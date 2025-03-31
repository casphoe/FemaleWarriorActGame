using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���� �Լ�
public class SoundManager : MonoBehaviour
{
    //ȯ���� ����� ���带 Ǯ�� �ؼ� ���ĵ� ������ �ʰ� �ϱ� ���ؼ� ���
    [Header("SFX")]
    public AudioSource audioSourcePrefab; // Ǯ���� AudioSource ������
    public int poolSize = 30;
    
    
    public List<Sound> sfxClips = new List<Sound>(); //ȿ���� ��� ����Ʈ
    private List<AudioSource> sfxPool = new List<AudioSource>(); // AudioSource Ǯ
    private Dictionary<string, AudioClip> sfxDict; // �̸����� clip�� ã�� ����

    public static SoundManager Instance;


    private void Awake()
    {
        Instance = this;
        InitializeSounds();
        CreateSfxAudioPool();
    }
    //ȿ���� ������ ���� �Լ�
    void InitializeSounds()
    {
        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var sound in sfxClips)
        {
            //���� �̸��� ������ ������ ������ �����ֱ� ���ؼ� ContainsKey �� üũ��
            if (!sfxDict.ContainsKey(sound.name))
                sfxDict.Add(sound.name, sound.clip);
        }
    }
    //����� Ǯ ����� �Լ�
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

        // �� ��� ���̸� ù ��° �� ��� (Ȥ�� null �����ص� ��)
        return sfxPool[0];
    }
}


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}