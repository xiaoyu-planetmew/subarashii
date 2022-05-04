using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public GameObject showGameObject;

    public AkEvent[] SFXs;


    public void SetDisableAndShowNextObject()
    {
        gameObject.SetActive(false);
        showGameObject.SetActive(true);
    }

    public void SetDisable()
    {
        gameObject.SetActive(false);
    }

    public void PlaySound(int index)
    {
        SFXs[index].HandleEvent(WwiseManager.Instance.gameObject);
    }
}
