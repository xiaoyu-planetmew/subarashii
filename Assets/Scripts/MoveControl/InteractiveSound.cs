using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSound : MonoBehaviour
{
    public AkEvent[] PlaySounds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            foreach(AkEvent sound in PlaySounds)
            {
                sound.HandleEvent(gameObject);
            }
        }
    }
}
