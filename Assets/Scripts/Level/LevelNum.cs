using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNum : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Text>().text = ((int) LevelController.Instance.level).ToString();
    }
}
