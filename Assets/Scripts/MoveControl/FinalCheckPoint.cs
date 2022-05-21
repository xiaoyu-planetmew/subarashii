using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && PlayerController.Instance.startPlaying)
        {
            PlayerController.Instance.startPlaying = false;
            LevelController.Instance.finishThisLevel = true;

            if (LevelController.Instance.level!=Level.Level_5_ver1)
                FinishManager.Instance.FinishPlaying((int)LevelController.Instance.level - 1, PlayerController.Instance.blood, PlayerController.Instance.successPoints, PlayerController.Instance.totalPoints);
            else
            {
                LevelController.Instance.FinishLevel();
                SceneController.Instance.ChangeScene("Level_Finished");
                Time.timeScale = 1;
            }
            
        }
    }
}
