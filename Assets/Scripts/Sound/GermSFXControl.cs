using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermSFXControl : MonoBehaviour
{
    void GermSuccesSFX()
    {
        SoundController.Instance.Space_Hinder.HandleEvent(gameObject);
    }

    void GermCatchSFX()
    {
        SoundController.Instance.Door_Impacting.HandleEvent(gameObject);
    }
}
