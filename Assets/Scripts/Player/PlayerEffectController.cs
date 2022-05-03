using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    public WavyCirclePointsController[] wavyCircles;

    public static PlayerEffectController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void DragCircleEffect(KeyDirectionType dragDir, bool powerful = false)
    {
        foreach(WavyCirclePointsController wave in wavyCircles)
        {
            wave.DragCircle(dragDir, powerful);
        }
    }

    public void ResetPlayerEffect()
    {

    }

}

public enum SpaceAnimation
{

}