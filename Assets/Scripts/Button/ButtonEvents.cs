using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
     public void ChangeScene(string toLevel)
    {
        SceneController.Instance.ChangeScene(toLevel);
        Time.timeScale = 1;
    }

    public void PlayAgain()
    {
        LevelController.Instance.PlayAgain();
        Time.timeScale = 1;
    }

    public void Resume()
    {
        LevelController.Instance.Resume();
    }

    public void ResumeMusic()
    {
        SoundController.Instance.ResumeAll.HandleEvent(WwiseManager.Instance.gameObject);
    }

}
