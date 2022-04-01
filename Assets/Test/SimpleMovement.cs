using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float inputDelayTime = 0.1f;
    public float force = 1f;
    public float powerfulRate = 1.2f;
    public float accelationTime = 0.1f;
    public float decelation = 2f;

    private WavyCirclePointsController waveController;
    private Rigidbody2D rigid;
    private float keyTimer;
    private bool getKeyDown;
    private float axisX;
    private float axisY;
    private Vector2 dirAngle;
    private Vector2 impulseVelocity;
    private float accelatingTimer;
    private bool startAccelating;

    private void Start()
    {
        waveController = GetComponentInChildren<WavyCirclePointsController>();
        rigid = GetComponent<Rigidbody2D>();
        keyTimer = 0;
        axisX = 0;
        axisY = 0;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) ||
           Input.GetKeyDown(KeyCode.DownArrow) ||
           Input.GetKeyDown(KeyCode.LeftArrow) ||
           Input.GetKeyDown(KeyCode.RightArrow))
        {
            getKeyDown = true;
        }

        if(getKeyDown)
        {
            keyTimer += Time.deltaTime;

            GetKeyToAngle();

            if (keyTimer >= inputDelayTime)
            {
                keyTimer = 0;
                getKeyDown = false;

                dirAngle = new Vector2(axisX, axisY);
                MoveDirection dir = GetInputDir(dirAngle);
                if(waveController!=null)
                    waveController.DragCircle(dir, Input.GetKey(KeyCode.LeftShift));

                rigid.AddForce(dirAngle.normalized * force * (Input.GetKey(KeyCode.LeftShift) ? powerfulRate : 1f), ForceMode2D.Impulse);
                impulseVelocity = rigid.velocity;
                startAccelating = true;

                Debug.Log(dir);

                axisX = 0;
                axisY = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if(startAccelating)
        {
            accelatingTimer += Time.fixedDeltaTime;

            if (accelatingTimer >= accelationTime)
            {
                startAccelating = false;
                accelatingTimer = 0;

            }

            return;
        }

        rigid.velocity = Vector2.Lerp(rigid.velocity, -2 * impulseVelocity, Time.fixedDeltaTime * decelation);
        if (Vector2.Dot(rigid.velocity, impulseVelocity) <= 0)
            rigid.velocity = Vector2.zero;
    }

    private void GetKeyToAngle()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            if(Input.GetKey(KeyCode.DownArrow))
            {
                axisY = 0;
            }
            else
            {
                axisY = 1f;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                axisY = 0;
            }
            else
            {
                axisY = -1f;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                axisX = 0;
            }
            else
            {
                axisX = 1f;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                axisX = 0;
            }
            else
            {
                axisX = -1f;
            }
        }
    }

    private MoveDirection GetInputDir(Vector2 inputAxisVec)
    {

        if(inputAxisVec.magnitude < 0.001f) //输入太小时
        {
            Debug.Log("太小" + inputAxisVec);
            return MoveDirection.Stop;
        }

        if(Vector2.Dot(inputAxisVec, Vector2.left)>=0)
        {
            float ang = Vector2.Angle(inputAxisVec, Vector2.up);

            if (ang < 30)
            {
                return MoveDirection.Up;
            }
            else if(ang < 60)
            {
                return MoveDirection.UpLeft;
            }
            else if(ang < 120)
            {
                return MoveDirection.Left;
            }
            else if(ang < 150)
            {
                return MoveDirection.DownLeft;
            }
            else
            {
                return MoveDirection.Down;
            }
        }
        else
        {
            float ang = Vector2.Angle(inputAxisVec, Vector2.up);

            if (ang < 30)
            {
                return MoveDirection.Up;
            }
            else if (ang < 60)
            {
                return MoveDirection.UpRight;
            }
            else if (ang < 120)
            {
                return MoveDirection.Right;
            }
            else if (ang < 150)
            {
                return MoveDirection.DownRight;
            }
            else
            {
                return MoveDirection.Down;
            }
        }
    }
}
