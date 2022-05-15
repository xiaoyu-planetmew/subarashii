using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5WallControl : MonoBehaviour
{
    public GameObject invisibleWall;

    private void Start()
    {
        invisibleWall.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("On trigger");
        if(collision.tag == "Player")
        {
            Debug.Log("Set Wall True");
            invisibleWall.SetActive(true);
        }
    }

    public void ResetWall()
    {
        invisibleWall.SetActive(false);
    }
}

