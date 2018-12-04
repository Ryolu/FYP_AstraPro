using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Audio_Manager : MonoBehaviour {

    public Slider BGM, SFX;

    public void BGMMaxAudio(float Volume)
    {
        BGM.value = Volume;
    }
    public void BGMMinAudio()
    {
        BGM.value = 0;
    }
    public void SFXMaxAudio(float Volume)
    {
        SFX.value = Volume;
    }
    public void SFXMinAudio()
    {
        SFX.value = 0;
    }
}
