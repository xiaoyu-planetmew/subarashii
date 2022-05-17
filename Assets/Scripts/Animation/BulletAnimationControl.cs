using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimationControl : MonoBehaviour
{
    public GameObject bullet;

    void bulletOn()
    {
        bullet.SetActive(true);
    }

    void bulletOff()
    {
        bullet.SetActive(false);
    }
}
