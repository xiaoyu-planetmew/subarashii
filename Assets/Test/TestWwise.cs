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
    private uint testId;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //test
        TutorialTrackController.Instance.StartPlayingMusicFromTutorial();
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchToTutorial.HandleEvent(gameObject);
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            TutorialTrackController.Instance.FinishTutorial();
        }
        
    }

    public void PlayMainMusic()
    {
        PlayMainMuic.HandleEvent(gameObject);
    }

    private int GetNowPlayingPosition(uint playingId)
    {
        int _prevSample = 0;

        AKRESULT akResult = AkSoundEngine.GetSourcePlayPosition(playingId, out int currentSourcePos, true);

        if (akResult == AKRESULT.AK_Success)
        {
            if (_prevSample <= 0)
            {
                // Do nothing, the song just started, looped, or doesn't support SourcePlayPosition
            }
            else if (_prevSample > currentSourcePos)
            {
                Debug.LogError("It went backwards? " + currentSourcePos + " @ " + Time.time);
            }

            _prevSample = currentSourcePos;
        }

        return 0;
    }



}
