using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
     public void ChangeScene(string toLevel)
    {
        SceneController.Instance.ChangeScene(toLevel);
    }
    
    public void PlayAgain()
    {
        LevelController.Instance.PlayAgain();
    }

}
