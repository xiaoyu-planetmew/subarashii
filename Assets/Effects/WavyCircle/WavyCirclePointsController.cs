using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavyCirclePointsController : MonoBehaviour
{
    [Header("���ڲ���")]
    public int pointsNum = 120;
    public float radius = 2f;
    public int waveSegments = 4; // ������
    public float speed = 0.5f;
    public float amplitude = 0.05f;

    [Header("��������")]
    public int randomWaveSeg = 2; // ������
    public float randomWaveSpeed = 1f;
    public float randomWaveAmp = 0.1f;

    [Header("��ת")]
    public bool enableRotation = true;
    public float rotateSpeed = 0.5f;
    public float rotateRatio = 30f;

    [Header("��ק")]
    public MoveDirection dragDrection;
    public float dragShrinkMax = 0.5f; //����������
    public float dragShrinkMin = 0.75f; //С��������
    public float dragShrinkTime = 0.3f;
    public float dragShrinkSpeed = 1f;
    public float dragWaveSpeed = 2f;
    public float dragPowerAmgMax = 1.5f;
    public float dragPowerAmgMin = 1.2f;
    public float dragRange = 60f;
    public float dragRangePowerful = 50f;
    public float dragTime = 0.5f;
    public float dragSpeed = 2f;

    [Header("��Ⱦ")]
    public WaveRenderType renderType = WaveRenderType.Stroke;

    [Header("��ײ")]
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

        //��������
        SimulateWave();
        

        //ʱ�ָ̻���ת�ٶ�
        KeepingWaveSpeed();

        //��ק
        DragEffect();

        //��Ⱦ
        RenderWave();
    }

    private void FixedUpdate()
    {
        //��ת
        if (enableRotation)
            RotateInWaving();

        // ����
        if (jellyEffect != null)
            jellyEffect.CalculateJelly(originalPos);

        //��ײ
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
    /// ģ����ײ����
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
                    else // dragAngle = 0, Up��ʱ��
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

            //����ײ��������
            if (!colliderEnter) afterCollider[i] = afterJellyPos[i];

            ang = (ang + (360f / segments)) % 360;
        }

    }

    /// <summary>
    /// ��ק�ӿ�
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
    /// ������Բ
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
    /// ������Դ�С���Ż���Բ
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
    /// ����ģ��
    /// </summary>
    private void SimulateWave()
    {
        // ����
        startAngle += speed;
        staticWaveAngle = startAngle;
        startRandomWaveAngle += randomWaveSpeed;
        randomWaveAngle = startRandomWaveAngle;

        //����
        SetRadius(nowRadius);

        // ������Ǻ�������
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

        // ����������
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
    /// ʱ�ָ̻�������ת�ٶ�
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
    /// ��ת
    /// </summary>
    private void RotateInWaving()
    {
        Vector3 ro = new Vector3(0, 0, rotateRatio);
        Quaternion roQ = Quaternion.Euler(ro);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, transform.localRotation * roQ, rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// ��ק��Ч
    /// </summary>
    private void DragEffect()
    {
        if (!hasStartDragging)
            return;

        timer += Time.deltaTime;

        //ֱ���仯
        if(!hasArriveMaxDragDis) //����
        {
            nowRadius -= (originRadius * (1 - (dragPowerfuly ? dragShrinkMax : dragShrinkMin))/(dragShrinkTime / Time.deltaTime) * dragShrinkSpeed);

            SetRadius(nowRadius);

            randomWaveSpeed = originRandomSpeed * dragWaveSpeed * (dragPowerfuly ? 1 : 0.5f); //������ת

            if (nowRadius <= originRadius * (dragPowerfuly ? dragShrinkMax : dragShrinkMin))
            {
                hasArriveMaxDragDis = true;
            }
        }
        else // ��ԭ
        {
            nowRadius = Mathf.Lerp(radius, originRadius * 1.2f, Time.deltaTime * dragShrinkSpeed);


            if(nowRadius >= originRadius)
            {
                nowRadius = originRadius;
            }

            SetRadius(nowRadius);
        }

        //��ק��Ч
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
                else // dragAngle = 0, Up��ʱ��
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

        //����
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
    /// �趨��ק�Ƕ�
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
