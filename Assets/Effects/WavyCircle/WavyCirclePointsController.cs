using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavyCirclePointsController : MonoBehaviour
{
    [Header("周期波浪")]
    public int pointsNum = 120;
    public float radius = 2f;
    public int waveSegments = 4; // 几个波
    public float speed = 0.5f;
    public float amplitude = 0.05f;

    [Header("不规则波浪")]
    public int randomWaveSeg = 2; // 几个波
    public float randomWaveSpeed = 1f;
    public float randomWaveAmp = 0.1f;

    [Header("旋转")]
    public bool enableRotation = true;
    public float rotateSpeed = 0.5f;
    public float rotateRatio = 30f;

    [Header("拖拽")]
    public MoveDirection dragDrection;
    public float dragShrinkMax = 0.5f; //大力的收缩
    public float dragShrinkMin = 0.75f; //小力的收缩
    public float dragShrinkTime = 0.3f;
    public float dragShrinkSpeed = 1f;
    public float dragWaveSpeed = 2f;
    public float dragPowerAmgMax = 1.5f;
    public float dragPowerAmgMin = 1.2f;
    public float dragRange = 60f;
    public float dragRangePowerful = 50f;
    public float dragTime = 0.5f;
    public float dragSpeed = 2f;

    [Header("渲染")]
    public WaveRenderType renderType = WaveRenderType.Stroke;

    [Header("碰撞")]
    public WayyCollider[] triggers;
    public float colliderRange = 45f;
    public float colliderIntense = 2f;

    private float startAngle = 0f;
    private float startRandomWaveAngle = 0f;
    private float staticWaveAngle = 0f;
    private float randomWaveAngle = 0f;
    private Vector3[] originalPos;
    private Vector3[] afterJellyPos;
    private Vector3[] afterCollider;
    private Vector3[] afterShrink;
    private Vector3[] renderPos;
    private LineRenderer line;
    private WavePointsFilling fillingRenderer;
    private WaveJellyEffect jellyEffect;
    private int segments;
    private float originRadius;
    private float nowRadius;
    private float originRandomSpeed;
    private float dragAngle;
    private float nowDragDis;
    private float timer;

    private bool dragPowerfuly;
    private bool hasStartDragging;
    private bool hasArriveMaxDragDis;

    private void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();


        if(renderType == WaveRenderType.Stroke && line != null)
        {
            line.positionCount = pointsNum;
            line.useWorldSpace = false;
        }

        segments = pointsNum;

        fillingRenderer = GetComponent<WavePointsFilling>();

        jellyEffect = GetComponent<WaveJellyEffect>();

        originalPos = new Vector3[pointsNum];

        afterJellyPos = new Vector3[pointsNum];

        afterCollider = new Vector3[pointsNum];

        afterShrink = new Vector3[pointsNum];

        renderPos = new Vector3[pointsNum];

        originRadius = radius;

        nowRadius = originRadius;

        originRandomSpeed = randomWaveSpeed;

        dragDrection = MoveDirection.Stop;

        InitialRadius();
    }

    private void Update()
    {
        //Test
        if(Input.GetKeyDown(KeyCode.G))
        {
            //DragCircle();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //DragCircle(true);
        }

        //正常波浪
        SimulateWave();
        

        //时刻恢复运转速度
        KeepingWaveSpeed();

        //拖拽
        DragEffect();

        //渲染
        RenderWave();
    }

    private void FixedUpdate()
    {
        //旋转
        if (enableRotation)
            RotateInWaving();

        // 果冻
        if (jellyEffect != null)
            jellyEffect.CalculateJelly(originalPos);

        //碰撞
        if (GetColliderEnter())
        {
            SimulateColliding();
        }
        else
        {
            for (int i = 0; i < pointsNum; i++)
            {
                afterCollider[i] = afterJellyPos[i];
            }
        }

    }


    private bool GetColliderEnter()
    {
        bool enter = false;

        foreach(WayyCollider trigger in triggers)
        {
            enter = enter || trigger.triggerEnter;
        }

        return enter;
    }

    /// <summary>
    /// 模拟碰撞变形
    /// </summary>
    private void SimulateColliding()
    {
        float segAngle = 360f / triggers.Length;

        float ang = (360 - transform.localEulerAngles.z) % 360;

        for (int i = 0; i < pointsNum; i++)
        {
            bool colliderEnter = false;

            for (int t = 0; t < triggers.Length; t++)
            {

                if(triggers[t].triggerEnter)
                {
                    float nowRange = colliderRange;
                    float angMin = t * segAngle - nowRange;
                    float angMax = t * segAngle + nowRange;

                    if (t != 0)
                    {
                        if (ang > angMin && ang < angMax)
                        {
                            float disAngle = Mathf.Deg2Rad * (ang - t * segAngle) * 90 / nowRange;

                            float x = Mathf.Sin(Mathf.Deg2Rad * (ang + transform.localEulerAngles.z)) * triggers[t].colliDis * colliderIntense
                                * Mathf.Sqrt(Mathf.Cos(disAngle));
                            float y = Mathf.Cos(Mathf.Deg2Rad * (ang + transform.localEulerAngles.z)) * triggers[t].colliDis * colliderIntense
                                * Mathf.Sqrt(Mathf.Cos(disAngle));

                            afterCollider[i] = afterJellyPos[i] - new Vector3(x, y, 0);

                            colliderEnter = true;
                        }
                    }
                    else // dragAngle = 0, Up的时候
                    {
                        if (ang < angMax || ang > (angMin + 360) % 360)
                        {
                            float disAngle = 0;

                            if (ang < angMax)
                                disAngle = Mathf.Deg2Rad * (ang) * 90 / nowRange;
                            if (ang > (angMin + 360) % 360)
                                disAngle = Mathf.Deg2Rad * (360 - ang) * 90 / nowRange;

                            float x = Mathf.Sin(Mathf.Deg2Rad * (ang + transform.localEulerAngles.z)) * triggers[t].colliDis * colliderIntense
                                * Mathf.Sqrt(Mathf.Cos(disAngle));
                            float y = Mathf.Cos(Mathf.Deg2Rad * (ang + transform.localEulerAngles.z)) * triggers[t].colliDis * colliderIntense
                                * Mathf.Sqrt(Mathf.Cos(disAngle));

                            afterCollider[i] = afterJellyPos[i] - new Vector3(x, y, 0);

                            colliderEnter = true;
                        }
                    }
                }
            }

            //无碰撞区域重置
            if (!colliderEnter) afterCollider[i] = afterJellyPos[i];

            ang = (ang + (360f / segments)) % 360;
        }

    }

    /// <summary>
    /// 拖拽接口
    /// </summary>
    /// <param name="dragDir"></param>
    /// <param name="powerful"></param>
    public void DragCircle(MoveDirection dragDir, bool powerful = false)
    {
        ResetDragFlags();

        hasStartDragging = true;

        dragDrection = dragDir;
        dragPowerfuly = powerful;
        nowDragDis = originRadius * (dragPowerfuly ? dragPowerAmgMax : dragPowerAmgMin);

    }

    /// <summary>
    /// 画基础圆
    /// </summary>
    private void InitialRadius()
    {
        float x;
        float y;
        float z = 0f;

        float ang = 0f;  // start from 0 degree

        for (int i = 0; i < pointsNum; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * ang) * originRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * ang) * originRadius;

            originalPos[i] = new Vector3(x, y, z);
            afterJellyPos[i] = originalPos[i];
            afterCollider[i] = afterJellyPos[i];

            ang += (360f / segments);
        }
    }

    /// <summary>
    /// 根据相对大小缩放基础圆
    /// </summary>
    /// <param name="radius"></param>
    private void SetRadius(float newRadius)
    {
        float x;
        float y;
        float z = 0f;

        float ang = 0f;  // start from 0 degree

        for (int i = 0; i < pointsNum; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * ang) * (newRadius - originRadius);
            y = Mathf.Cos(Mathf.Deg2Rad * ang) * (newRadius - originRadius);

            afterShrink[i] = afterCollider[i] + new Vector3(x, y, z);

            ang += (360f / segments);
        }

        nowRadius = newRadius;


    }

    /// <summary>
    /// 波浪模拟
    /// </summary>
    private void SimulateWave()
    {
        // 滚动
        startAngle += speed;
        staticWaveAngle = startAngle;
        startRandomWaveAngle += randomWaveSpeed;
        randomWaveAngle = startRandomWaveAngle;

        //基础
        SetRadius(nowRadius);

        // 添加三角函数波动
        for (int i = 0; i < pointsNum; i++)
        {
            float x = Mathf.Cos(staticWaveAngle) * amplitude;
            float y = Mathf.Sin(staticWaveAngle) * amplitude;
            float z = 0;
            Vector3 wave = new Vector3(x, y, z);

            renderPos[i] = afterShrink[i] + wave;
            //renderPos[i] = afterShrink[i];

            staticWaveAngle += (Mathf.Deg2Rad * 360 / segments * waveSegments);
        }

        // 添加随机波动
        for (int i = 0; i < pointsNum; i++)
        {
            float x = Mathf.Cos(randomWaveAngle) * randomWaveAmp;
            float y = Mathf.Sin(randomWaveAngle) * randomWaveAmp;
            float z = 0;
            Vector3 wave = new Vector3(x, y, z);

            renderPos[i] = renderPos[i] + wave;

            randomWaveAngle += (Mathf.Deg2Rad * 360 / segments * randomWaveSeg);
        }
    }

    /// <summary>
    /// 时刻恢复波浪运转速度
    /// </summary>
    private void KeepingWaveSpeed()
    {
        if(Mathf.Abs(randomWaveSpeed) > Mathf.Abs(originRandomSpeed))
        {
            randomWaveSpeed = Mathf.Lerp(randomWaveSpeed, originRandomSpeed * 0.9f, Time.deltaTime * 2);
        }
        else
        {
            randomWaveSpeed = originRandomSpeed;
        }
    }

    /// <summary>
    /// 旋转
    /// </summary>
    private void RotateInWaving()
    {
        Vector3 ro = new Vector3(0, 0, rotateRatio);
        Quaternion roQ = Quaternion.Euler(ro);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * roQ, rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 拖拽特效
    /// </summary>
    private void DragEffect()
    {
        if (!hasStartDragging)
            return;

        timer += Time.deltaTime;

        //直径变化
        if(!hasArriveMaxDragDis) //收缩
        {
            nowRadius -= (originRadius * (1 - (dragPowerfuly ? dragShrinkMax : dragShrinkMin))/(dragShrinkTime / Time.deltaTime) * dragShrinkSpeed);

            SetRadius(nowRadius);

            randomWaveSpeed = originRandomSpeed * dragWaveSpeed * (dragPowerfuly ? 1 : 0.5f); //加速旋转

            if (nowRadius <= originRadius * (dragPowerfuly ? dragShrinkMax : dragShrinkMin))
            {
                hasArriveMaxDragDis = true;
            }
        }
        else // 复原
        {
            nowRadius = Mathf.Lerp(radius, originRadius * 1.2f, Time.deltaTime * dragShrinkSpeed);


            if(nowRadius >= originRadius)
            {
                nowRadius = originRadius;
            }

            SetRadius(nowRadius);
        }

        //拉拽特效
        if(dragDrection != MoveDirection.Stop)
        {
            CalculateDragDerection();

            float ang = (360 - transform.localEulerAngles.z) % 360;

            float nowRange =  dragPowerfuly ? dragRangePowerful : dragRange;
            float angMin = dragAngle - nowRange;
            float angMax = dragAngle + nowRange;

            for (int i = 0; i < pointsNum; i++)
            {

                if(dragAngle != 0)
                {
                    if (ang > angMin && ang < angMax)
                    {
                        float disAngle = Mathf.Deg2Rad * (ang - dragAngle) * 90 / nowRange;

                        //nowDragDis = Mathf.Max(Mathf.Lerp(nowDragDis, 0, Time.deltaTime / 5), 0);

                        float x = Mathf.Sin(Mathf.Deg2Rad * (ang + transform.localEulerAngles.z)) * nowDragDis
                            * Mathf.Pow(Mathf.Cos(disAngle), 4);
                        float y = Mathf.Cos(Mathf.Deg2Rad * (ang + transform.localEulerAngles.z)) * nowDragDis
                            * Mathf.Pow(Mathf.Cos(disAngle), 4);

                        renderPos[i] += new Vector3(x, y, 0);
                    }

                }
                else // dragAngle = 0, Up的时候
                {
                    if (ang < angMax || ang > (angMin + 360) % 360)
                    {
                        float disAngle = 0;

                        if(ang < angMax)
                            disAngle = Mathf.Deg2Rad * (ang + 0.5f) * 90 / nowRange;
                        if(ang > (angMin + 360) % 360)
                            disAngle = Mathf.Deg2Rad * (360 - ang - 0.5f) * 90 / nowRange;


                        float x = Mathf.Sin(Mathf.Deg2Rad * (ang + transform.localEulerAngles.z)) * nowDragDis
                            * Mathf.Pow(Mathf.Cos(disAngle), 4);
                        float y = Mathf.Cos(Mathf.Deg2Rad * (ang + transform.localEulerAngles.z)) * nowDragDis
                            * Mathf.Pow(Mathf.Cos(disAngle), 4);

                        renderPos[i] += new Vector3(x, y, 0);
                    }

                }

                ang = (ang + (360f / segments)) % 360;
            }

            nowDragDis = Mathf.Max(Mathf.Lerp(nowDragDis, 0, Time.deltaTime * dragSpeed), 0);

        }

        //结束
        if (timer >= dragTime)
        {
            ResetDragFlags();
        }
    }

    private void ResetDragFlags()
    {
        timer = 0;
        hasStartDragging = false;
        hasArriveMaxDragDis = false;
        dragDrection = MoveDirection.Stop;
        nowDragDis = originRadius * dragPowerAmgMin;
    }

    /// <summary>
    /// 设定拖拽角度
    /// </summary>
    private void CalculateDragDerection()
    {
        switch (dragDrection)
        {
            case MoveDirection.Up:
                dragAngle = 0;
                break;
            case MoveDirection.UpRight:
                dragAngle = 45;
                break;
            case MoveDirection.Right:
                dragAngle = 90;
                break;
            case MoveDirection.DownRight:
                dragAngle = 135;
                break;
            case MoveDirection.Down:
                dragAngle = 180;
                break;
            case MoveDirection.DownLeft:
                dragAngle = 225;
                break;
            case MoveDirection.Left:
                dragAngle = 270;
                break;
            case MoveDirection.UpLeft:
                dragAngle = 315;
                break;
        }

    }

    private void RenderWave()
    {
        if(renderType == WaveRenderType.Stroke)
        {
            if(line==null)
                Debug.LogError("No Wave LineRenderer!");

            for (int i = 0; i < pointsNum; i++)
            {
                line.SetPosition(i,renderPos[i]);
            }
        }
        else if(renderType == WaveRenderType.Filling)
        {
            if (fillingRenderer == null)
                Debug.LogError("No Wave Filling Renderer!");

            fillingRenderer.FillingPoints(renderPos);
        }
    }
}

public enum WaveRenderType
{
    Stroke,
    Filling,
}

public enum MoveDirection
{
    Stop,
    Up,
    UpLeft,
    Left,
    DownLeft,
    Down,
    DownRight,
    Right,
    UpRight,
}
