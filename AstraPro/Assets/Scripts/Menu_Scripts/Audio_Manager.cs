using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Audio_Manager : MonoBehaviour {
    //for setting audio
    public Slider BGM, SFX;
    public AudioMixer masterMixer;
    public static Audio_Manager Instance = null;

    //list to keep all the audio
    public Dictionary<string,AudioClip> audioDictionary;
    public AudioClip[] audioList;

    //setting the audio manager game object not to be destory
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    //start of the to add the sound form dictionary 
    void Start()
    {
        audioDictionary = new Dictionary<string, AudioClip>();

        foreach (var clip in audioList)
        {
            audioDictionary.Add(clip.name, clip);
        }

    }
    //max BGM sound output
    public void BGMMaxAudio(float Volume)
    {
        BGM.value = Volume;
        masterMixer.SetFloat("BackgroundMusic", 0);
    }
    //min BGM sound output
    public void BGMMinAudio()
    {
        BGM.value = -80;
        masterMixer.SetFloat("BackgroundMusic", -80);
    }
    //max SFX sound output 
    public void SFXMaxAudio(float Volume)
    {
        SFX.value = Volume;
        masterMixer.SetFloat("SoundEffects", 0);
    }
    //min SFX sound output
    public void SFXMinAudio()
    {
        SFX.value = -80;
        masterMixer.SetFloat("SoundEffects", -80);
    }
    //Checking the sounds from the mixer
    public void SetSfxLvl(float sfxLvl)
    {
        masterMixer.SetFloat("SoundEffects", sfxLvl);
    }
    //Checking the sounds from the mixer
    public void SetBgmLvl(float bgmLvl)
    {
        masterMixer.SetFloat("BackgroundMusic", bgmLvl);
    }
}
