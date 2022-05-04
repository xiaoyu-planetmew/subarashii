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
        AkSoundEngine.SetRTPCValue("MasterVolume",v * 100);

        Debug.Log("master"+v);

        WwiseManager.Instance.masterVol = v * 100;
    }
    public void MusicVolume(float v)
    {
        AkSoundEngine.SetRTPCValue("MusicVolume", v * 100);
    }
    public void SFXVolume(float v)
    {
        AkSoundEngine.SetRTPCValue("SFXVolume", v * 100);
    }
}
