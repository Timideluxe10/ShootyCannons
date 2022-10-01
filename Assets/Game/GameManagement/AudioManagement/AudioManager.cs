using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private bool doPlaySounds = true;

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if(doPlaySounds)
            AudioSource.PlayClipAtPoint(clip, position);
    }
}
