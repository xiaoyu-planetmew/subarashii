using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            AkSoundEngine.SetRTPCValue("MasterVolume",10);
            Debug.Log("P!");
        }
    }
}
