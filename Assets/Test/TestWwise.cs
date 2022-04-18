using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWwise : MonoBehaviour
{
    public AkEvent StartMusic;
    public AkEvent SwitchToMain;
    public AkEvent SwitchToTutorial;
    public AkEvent PlayMainMuic;

    public static TestWwise Instance;

    private void Awake()
    {
        Instance = this;
    }

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

    public void PlayMainMusic()
    {
        PlayMainMuic.HandleEvent(gameObject);
    }
}
