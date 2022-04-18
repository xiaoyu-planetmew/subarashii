using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWwise : MonoBehaviour
{
    public AkEvent StartMusic;
    public AkEvent SwitchToMain;
    public AkEvent SwitchToTutorial;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            StartMusic.HandleEvent(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SwitchToMain.HandleEvent(gameObject);
        }
    }
}
