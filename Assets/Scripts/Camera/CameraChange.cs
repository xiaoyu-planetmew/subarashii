using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public CameraStatus changeToStatus = CameraStatus.Normal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CameraController.Instance.ChangeCameraStatus(changeToStatus);
        }
    }
}
