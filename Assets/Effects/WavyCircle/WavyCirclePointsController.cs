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
    public DragDrection dragDrection;
    public float dragShrinkMax = 0.5f; //大力的收缩
    public float dragShrinkMin = 0.75f; //小力的收缩
    public float dragShrinkTime = 0.3f;
    public float dragShrinkSpeed = 1f;
    public float dragWaveSpeed = 2f;
    public float dragPowerAmgMax = 1.5f;
    public float dragPowerAmgMin = 1.2f;
    public float dragRange = 30f;
    public float dragTime = 0.5f;

    [Header("渲染")]
    public WaveRenderType renderType = WaveRenderType.Stroke;

    private float startAngle = 0f;
    private float startRandomWaveAngle = 0f;
    private float staticWaveAngle = 0f;
    private float randomWaveAngle = 0f;
    private Vector3[] originalPos;
    private Vector3[] renderPos;
    private LineRenderer line;
    WavePointsFilling fillingRenderer;
    private int segments;
    private float originRadius;
    private float originRandomSpeed;
    private float dragAngle;
    private float nowDragDis;
    private float timer;

    [SerializeField] private bool dragPowerfuly;
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

        originalPos = new Vector3[pointsNum];

        renderPos = new Vector3[pointsNum];

        originRadius = radius;

        originRandomSpeed = randomWaveSpeed;

        dragDrection = DragDrection.Stop;

        SetRadius(radius);
    }

    private void Update()
    {
        //Test
        if(Input.GetKeyDown(KeyCode.G))
        {
            DragCircle();
        }

        

        //正常波浪
        SimulateWave();

        //旋转
        if(enableRotation)
            RotateInWaving();

        //时刻恢复运转速度
        KeepingWaveSpeed();

        //拖拽
        DragEffect();

        //渲染
        RenderWave();
    }

    public void DragCircle(bool powerful = false)
    {
        hasStartDragging = true;
        nowDragDis = originRadius * (dragPowerfuly ? dragPowerAmgMax : dragPowerAmgMin);
        dragPowerfuly = powerful;
    }

    /// <summary>
    /// 画圆
    /// </summary>
    private void SetRadius(float radius)
    {
        float x;
        float y;
        float z = 0f;

        float ang = 0f;  // start from 0 degree

        for (int i = 0; i < pointsNum; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * ang) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * ang) * radius;

            originalPos[i] = new Vector3(x, y, z);

            ang += (360f / segments);
        }
    }

    private void SimulateWave()
    {
        // 滚动
        startAngle += speed;
        staticWaveAngle = startAngle;
        startRandomWaveAngle += randomWaveSpeed;
        randomWaveAngle = startRandomWaveAngle;

        // 添加三角函数波动
        for (int i = 0; i < pointsNum; i++)
        {
            float x = Mathf.Cos(staticWaveAngle) * amplitude;
            float y = Mathf.Sin(staticWaveAngle) * amplitude;
            float z = 0;
            Vector3 wave = new Vector3(x, y, z);

            renderPos[i] = originalPos[i] + wave;

            //line.SetPosition(i, originalPos[i] + wave);

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

            //line.SetPosition(i, line.GetPosition(i) + wave);

            randomWaveAngle += (Mathf.Deg2Rad * 360 / segments * randomWaveSeg);
        }
    }

    /// <summary>
    /// 时刻恢复运转速度
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

    private void DragEffect()
    {
        if (!hasStartDragging)
            return;

        timer += Time.deltaTime;

        //直径变化
        if(!hasArriveMaxDragDis) //收缩
        {
            radius -= (originRadius * (1 - (dragPowerfuly ? dragShrinkMax : dragShrinkMin))/(dragShrinkTime / Time.deltaTime) * dragShrinkSpeed);

            SetRadius(radius);

            randomWaveSpeed = originRandomSpeed * dragWaveSpeed * (dragPowerfuly ? 1 : 0.5f); //加速旋转

            if (radius <= originRadius * (dragPowerfuly ? dragShrinkMax : dragShrinkMin))
            {
                hasArriveMaxDragDis = true;
            }
        }
        else // 复原
        {
            radius = Mathf.Lerp(radius, originRadius * 1.2f, Time.deltaTime * dragShrinkSpeed);


            if(radius >= originRadius)
            {
                radius = originRadius;
            }

            SetRadius(radius);
        }

        //拉拽特效
        if(dragDrection != DragDrection.Stop)
        {
            CalculateDragDerection();

            float ang = - transform.localEulerAngles.z;

            for (int i = 0; i < pointsNum; i++)
            {
                float nowRange = dragRange * (dragPowerfuly ? 0.75f : 1);

                if (ang > (dragAngle- nowRange) && ang < (dragAngle + nowRange))
                {
                    float disAngle = Mathf.Deg2Rad * (ang - dragAngle) * 90 / nowRange;

                    nowDragDis = Mathf.Max(Mathf.Lerp(nowDragDis, 0, Time.deltaTime/10), 0);

                    float x = Mathf.Sin(Mathf.Deg2Rad * (ang + transform.localEulerAngles.z)) * nowDragDis
                        * Mathf.Pow(Mathf.Cos(disAngle), 4);
                    float y = Mathf.Cos(Mathf.Deg2Rad * (ang + transform.localEulerAngles.z)) * nowDragDis
                        * Mathf.Pow(Mathf.Cos(disAngle), 4);

                    renderPos[i] += new Vector3(x, y, 0);
                        
                }

                ang += (360f / segments);
            }
        }

        //结束
        if(timer >= dragTime)
        {
            timer = 0;
            hasStartDragging = false;
            hasArriveMaxDragDis = false;
            //dragDrection = DragDrection.Stop;
        }
    }

    /// <summary>
    /// 设定拖拽角度
    /// </summary>
    private void CalculateDragDerection()
    {
        switch (dragDrection)
        {
            case DragDrection.Up:
                dragAngle = 0;
                break;
            case DragDrection.UpLeft:
                dragAngle = 45;
                break;
            case DragDrection.Left:
                dragAngle = 90;
                break;
            case DragDrection.DownLeft:
                dragAngle = 135;
                break;
            case DragDrection.Down:
                dragAngle = 180;
                break;
            case DragDrection.DownRight:
                dragAngle = 225;
                break;
            case DragDrection.Right:
                dragAngle = 270;
                break;
            case DragDrection.UpRight:
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

    public enum WaveRenderType
    {
        Stroke,
        Filling,
    }

    public enum DragDrection
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

}



