using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
