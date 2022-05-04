using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    public AkEvent PlayThemeMusic;
    public AkEvent StopThemeMusic;

    public AkEvent StopLeve_1_Music;

    public void MainMenu_Enter()
    {
        PlayThemeMusic.HandleEvent(WwiseManager.Instance.gameObject);


        Debug.Log("Play Theme Music");
    }


    public void MainMenu_Leave()
    {
        StopThemeMusic.HandleEvent(WwiseManager.Instance.gameObject);

        Debug.Log("Stop Theme Music");
    }

    public void Level_1_Enter()
    {
        //StartCoroutine(WaitStopThemeMusic());

        //Debug.Log("Play Theme Music");
    }

    public void Level_1_Leave()
    {
        StopLeve_1_Music.HandleEvent(WwiseManager.Instance.gameObject);
    }

    private IEnumerator WaitStopThemeMusic()
    {
        yield return new WaitForSeconds(0.05f);

        StopThemeMusic.HandleEvent(gameObject);

    }

    public void Level_5_Enter()
    {
        LevelController.Instance.StartLevel();
    }
}
