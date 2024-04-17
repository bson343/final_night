using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class volume : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider audioSlider;
    public void AudioControl()
    {
        float sound = audioSlider.value;
        if(sound == - 40f) masterMixer.SetFloat("Master", -80);
        else masterMixer.SetFloat("Master", sound);
    }
    // Start is called before the first frame update
    
    
}
