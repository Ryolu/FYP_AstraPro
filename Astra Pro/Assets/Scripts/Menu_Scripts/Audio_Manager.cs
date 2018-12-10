using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Audio_Manager : MonoBehaviour {

    public Slider BGM, SFX;
    public AudioSource BGMSet, SFXSet;

    public void BGMMaxAudio(float Volume)
    {
        BGM.value = Volume;
        BGMSet.GetComponent<AudioSource>().volume = Volume;
    }
    public void BGMMinAudio()
    {
        BGM.value = 0;
        BGMSet.GetComponent<AudioSource>().volume = 0;
    }
    public void SFXMaxAudio(float Volume)
    {
        SFX.value = Volume;
        SFXSet.GetComponent<AudioSource>().volume = Volume;
    }
    public void SFXMinAudio()
    {
        SFX.value = 0;
        SFXSet.GetComponent<AudioSource>().volume = 0;
    }
}
