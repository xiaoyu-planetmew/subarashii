using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TestCircle : MonoBehaviour
{
    [Header("周期波浪")]
    public int pointsNum = 120;
    public float radius = 2f;
    public int waveSegments = 4; // 几个波
    public float speed = 0.5f;
    public float amplitude = 0.05f;
    public float rotateSpeed = 0.5f;
    public float rotateRatio = 30f;

    [Header("不规则波浪")]
    public int randomWaveSeg = 2; // 几个波
    public float randomWaveSpeed = 1f;
    public float randomWaveAmp = 0.1f;


    private float startAngle = 0f;
    private float startRandomWaveAngle = 0f;
    private float staticWaveAngle = 0f;
    private float randomWaveAngle = 0f;
    private Vector3[] originalPos;
    private LineRenderer line;
    private int segments;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        segments = pointsNum;

        line.positionCount = pointsNum;

        line.useWorldSpace = false;

        originalPos = new Vector3[pointsNum];

        CreatePoints();
    }

    void Update()
    {
        SimulateWave();

    }

    /// <summary>
    /// 画圆
    /// </summary>
    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;

        float ang = 0f;  // start from 0 degree

        for (int i = 0; i < pointsNum; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * ang) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * ang) * radius;

            line.SetPosition(i, new Vector3(x, y, z));

            originalPos[i] = line.GetPosition(i);

            ang += (360f / segments);
        }
    }

    void SimulateWave()
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

            line.SetPosition(i, originalPos[i] + wave);

            staticWaveAngle += (Mathf.Deg2Rad * 360 / segments * waveSegments);
        }

        // 添加随机波动
        for (int i = 0; i < pointsNum; i++)
        {
            float x = Mathf.Cos(randomWaveAngle) * randomWaveAmp;
            float y = Mathf.Sin(randomWaveAngle) * randomWaveAmp;
            float z = 0;
            Vector3 wave = new Vector3(x, y, z);

            line.SetPosition(i, line.GetPosition(i) + wave);

            randomWaveAngle += (Mathf.Deg2Rad * 360 / segments * randomWaveSeg);
        }

        // 本身旋转
        Vector3 ro = new Vector3(0, 0, rotateRatio);
        Quaternion roQ = Quaternion.Euler(ro);
        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * roQ, rotateSpeed * Time.deltaTime);
        //Debug.Log(AudioData.amplitudeBuffer * rotateRatio);

    }
}
