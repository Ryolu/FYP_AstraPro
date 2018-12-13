using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Audio_Manager : MonoBehaviour {
    
    public Slider BGM, SFX;
    public AudioMixer masterMixer;
    public static Audio_Manager Instance = null;

    public Dictionary<string,AudioClip> audioDictionary;
    public AudioClip[] audioList;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        audioDictionary = new Dictionary<string, AudioClip>();

        foreach (var clip in audioList)
        {
            audioDictionary.Add(clip.name, clip);
        }

    }
    public void BGMMaxAudio(float Volume)
    {
        BGM.value = Volume;
        masterMixer.SetFloat("BackgroundMusic", 0);
    }
    public void BGMMinAudio()
    {
        BGM.value = -80;
        masterMixer.SetFloat("BackgroundMusic", -80);
    }
    public void SFXMaxAudio(float Volume)
    {
        SFX.value = Volume;
        masterMixer.SetFloat("SoundEffects", 0);
    }
    public void SFXMinAudio()
    {
        SFX.value = -80;
        masterMixer.SetFloat("SoundEffects", -80);
    }

    public void SetSfxLvl(float sfxLvl)
    {
        masterMixer.SetFloat("SoundEffects", sfxLvl);
    }
    public void SetBgmLvl(float bgmLvl)
    {
        masterMixer.SetFloat("BackgroundMusic", bgmLvl);
    }
}
