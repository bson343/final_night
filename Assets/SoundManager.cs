using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public AudioSource musicsource;

    public void SetMusicVolume(float volume){
        musicsource.volume = volume;
    }
    

    // Update is called once per frame
   
}
