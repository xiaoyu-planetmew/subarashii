using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{

    public void Level_5_Enter()
    {
        LevelController.Instance.StartLevel();
    }
}
