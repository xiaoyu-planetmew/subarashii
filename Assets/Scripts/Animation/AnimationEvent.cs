using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public GameObject showGameObject;

    public void SetDisableAndShowNextObject()
    {
        gameObject.SetActive(false);
        showGameObject.SetActive(true);
    }
}
