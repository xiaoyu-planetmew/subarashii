using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    float master;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MasterVolume(float v)
    {
        AkSoundEngine.SetRTPCValue("MasterVolume",v);
        master = v/100;
    }
    public void MusicVolume(float v)
    {
        AkSoundEngine.SetRTPCValue("MusicVolume", v*master);
    }
    public void SFXVolume(float v)
    {
        AkSoundEngine.SetRTPCValue("SFXVolume", v*master);
    }
}
