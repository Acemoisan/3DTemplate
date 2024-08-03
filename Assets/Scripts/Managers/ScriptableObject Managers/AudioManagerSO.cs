using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioManagerSO", menuName = "Scriptable Objects/Managers/AudioManagerSO")]
public class AudioManagerSO : ScriptableObject
{
    public void PlaySFX(AudioClip clip)
    {
        AudioManager.instance.PlaySFX(clip);
    }
}
