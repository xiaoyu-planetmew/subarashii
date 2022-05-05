using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    private void Start()
    {
        if(LevelController.Instance.level == Level.Level_5_ver1)
        {
#if UNITY_EDITOR
            StartCoroutine(WaitEnter());
#endif
        }
    }

    private IEnumerator WaitEnter()
    {
        yield return new WaitForSeconds(0.05f);

        LevelController.Instance.StartLevel();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {

        }
    }
}
