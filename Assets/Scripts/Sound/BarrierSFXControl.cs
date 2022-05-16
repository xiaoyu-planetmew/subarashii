using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSFXControl : MonoBehaviour
{
    void BarrierOpenSFX()
    {
        SoundController.Instance.Food_Barriage.HandleEvent(gameObject);
    }
}
