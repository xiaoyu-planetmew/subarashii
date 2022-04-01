using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayyCollider : MonoBehaviour
{
    public WaveColliderType triggerType;
    public bool triggerEnter;
    [HideInInspector] public float colliDis;

    private Vector3 colliStartPos;

    private void Start()
    {
        colliDis = 0;
    }

    private void FixedUpdate()
    {
        if(triggerEnter)
        {
            if(triggerType==WaveColliderType.Up || triggerType == WaveColliderType.Down)
            {
                colliDis = Mathf.Abs(colliStartPos.y - transform.position.y);
            }
            else
            {
                colliDis = Mathf.Abs(colliStartPos.x - transform.position.x);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            triggerEnter = true;
            colliStartPos = transform.position;
            colliDis = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            triggerEnter = false;
            colliDis = 0;
        }
    }
}

public enum WaveColliderType
{
    Up,
    Right,
    Down,
    Left,
}
