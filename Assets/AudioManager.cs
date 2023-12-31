using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Sound{
    CrossBow,
    SwingTool,
    PlayerAttack,
    EnemyHit,
    EnemyDie,
    PickupItem,
    Treasure
}

[System.Serializable]
public class SoundAudioClip
{
    public Sound sound;
    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource growlAudioSource;
    [SerializeField] List<SoundAudioClip> soundAudioClips;

    Dictionary<Sound, float> soundTimerDictionary;

    void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(gameObject); }

        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerAttack] = 0f;
        soundTimerDictionary[Sound.SwingTool] = 0f;
    }

    bool CanPlaySound(Sound sound)
    {


            if(sound == Sound.SwingTool || sound == Sound.PlayerAttack)
            {
                if(soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerAttackTimerMax = 0.25f;
                    if(lastTimePlayed + playerAttackTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else { return false; }
                }
                else { return true; }
            }
            else { return true; }
        
    }

    // void Start()
    // {
    //     DontDestroyOnLoad(gameObject);
    //     StartCoroutine(RandomSfxClips());
    // }

    // IEnumerator RandomSfxClips()
    // {
    //     while(true)
    //     {
    //         yield return new WaitForSeconds(Random.Range(20f, 45f));
    //         Debug.Log("Playing random sfx");
    //         PlayGrowlSFX(randomSfxClips[Random.Range(0, randomSfxClips.Count)]);
    //     }
    // }

    public void PlaySound(Sound sound)
    {
        if(CanPlaySound(sound) == false) { return; }
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(instance.GetAudioClip(sound));
    }

    //3D Audio!
    public void PlaySound(Sound sound, Vector3 pos)
    {
        if(CanPlaySound(sound) == false) { return; }
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.position = pos;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = instance.GetAudioClip(sound);
        audioSource.maxDistance = 100f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;
        audioSource.Play();
        //Destroy(soundGameObject, audioSource.clip.length);
    }


    public AudioClip GetAudioClip(Sound sound)
    {
        foreach(SoundAudioClip soundAudioClip in soundAudioClips)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found");
        return null;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    public void PlayGrowlSFX(AudioClip clip)
    {
        growlAudioSource.PlayOneShot(clip);
    }
}
