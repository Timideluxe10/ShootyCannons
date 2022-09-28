using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

public class SettingsController : MonoBehaviour
{
    private static SettingsController instance;

    [SerializeField] private AudioMixer audioMixer;

    private float soundVolume;
    private float musicVolume;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            GameObject.Destroy(this);
        }
    }
    public static SettingsController Instance { get => instance; }

    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        audioMixer.SetFloat("soundVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void EndApplication()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
