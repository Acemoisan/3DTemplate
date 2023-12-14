using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource growlAudioSource;
    [SerializeField] List<AudioClip> randomSfxClips;

    void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(RandomSfxClips());
    }

    IEnumerator RandomSfxClips()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(20f, 45f));
            Debug.Log("Playing random sfx");
            PlayGrowlSFX(randomSfxClips[Random.Range(0, randomSfxClips.Count)]);
        }
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
