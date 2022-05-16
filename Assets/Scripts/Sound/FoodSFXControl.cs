using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSFXControl : MonoBehaviour
{
    void FoodAbsorbSFX()
    {
        SoundController.Instance.Food_Aborb.HandleEvent(gameObject);
    }

    void FoodHappySFX()
    {
        SoundController.Instance.Food_Happy.HandleEvent(gameObject);
    }
}
