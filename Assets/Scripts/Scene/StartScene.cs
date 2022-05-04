using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    public AkEvent PlayTheme;

    private void Start()
    {
        PlayTheme.HandleEvent(WwiseManager.Instance.gameObject);
    }
}
