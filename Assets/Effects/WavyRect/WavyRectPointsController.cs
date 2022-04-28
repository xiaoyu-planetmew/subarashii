using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavyRectPointsController : MonoBehaviour
{
    public Vector2 size = new Vector2(100f,100f);
    public int segmentNum = 40;
    public float cornerRounded = 0.1f;

    [Header("周期波动")]
    public int waveSegments = 4; // 几个波
    public float speed = 0.5f;
    public float amplitude = 0.05f;

    [Header("不规则波浪")]
    public int randomWaveSeg = 2; // 几个波
    public float randomWaveSpeed = 1f;
    public float randomWaveAmp = 0.1f;

    public WaveRenderType renderType = WaveRenderType.Stroke;
    private int seg;
    private Vector3[] originPoints;
    private Vector3[] afterWave;

    private WavyRectLineRenderer lineRenderer;
    private WavyRectSpriteRenderer spriteRenderer;

    private float startAngle = 0f;
    private float startRandomWaveAngle = 0f;
    private float staticWaveAngle = 0f;
    private float randomWaveAngle = 0f;

    private void Start()
    {
        seg = Mathf.Abs(segmentNum / 4) * 4;

        originPoints = new Vector3[seg];

        afterWave = new Vector3[seg];

        InitialateRect();

        lineRenderer = GetComponent<WavyRectLineRenderer>();
        spriteRenderer = GetComponent<WavyRectSpriteRenderer>();



    }

    private void Update()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) > 30) return;

        SimulateWave();

        if (renderType==WaveRenderType.Stroke && lineRenderer!=null)
        {
            lineRenderer.RenderLine(afterWave);
        }
        if(renderType==WaveRenderType.Filling && spriteRenderer!=null)
        {
            spriteRenderer.RenderShape(afterWave);
        }
    }

    /// <summary>
    /// 画初始矩形
    /// </summary>
    private void InitialateRect()
    {
        for(int i = 0; i < 4;i++)
        {
            for(int j = 0; j < seg/4; j ++)
            {
                float x = 0;
                float y = 0;

                if(i==0)
                {
                    x = j * size[0] / (seg/4) - size[0] / 2;
                    y = size[1] / 2;
                }
                else if(i==1)
                {
                    x = size[0] / 2;
                    y = size[1] / 2 - j * size[1] / (seg / 4);
                }
                else if(i==2)
                {
                    x = size[0] / 2 - j * size[0] / (seg / 4);
                    y = -size[1] / 2;
                }
                else if(i==3)
                {
                    x = - size[0] / 2;
                    y = - size[1] / 2 + j * size[1] / (seg / 4);
                }

                originPoints[i * seg / 4 + j] = new Vector2(x, y);
            }
        }

        //圆角
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < seg / 4; j++)
            {
                if (i == 0)
                {
                    if (originPoints[i * seg / 4 + j][0] <= -size[0] / 2 * (1-cornerRounded))
                    {
                        float disAngle = Mathf.Deg2Rad * 45 * (-size[0] / 2 * (1 - cornerRounded) - originPoints[i * seg / 4 + j][0]) / (size[0] / 2) / cornerRounded;
                        originPoints[i * seg / 4 + j][0] = -size[0] / 2 * (1-cornerRounded) - Mathf.Sin(disAngle)  * size[0] / 2 *cornerRounded;
                        originPoints[i * seg / 4 + j][1] =  size[1] / 2 * (1-cornerRounded) + Mathf.Cos(disAngle) * size[0] / 2 * cornerRounded;
                        
                    }
                    else if(originPoints[i * seg / 4 + j][0] >= size[0] / 2 * (1 - cornerRounded))
                    {
                        float disAngle = Mathf.Deg2Rad * 45 * (originPoints[i * seg / 4 + j][0] - size[0] / 2 * (1 - cornerRounded)) / (size[0] / 2) / cornerRounded;
                        originPoints[i * seg / 4 + j][0] = size[0] / 2 * (1 - cornerRounded) + Mathf.Sin(disAngle)  * size[0] / 2 * cornerRounded;
                        originPoints[i * seg / 4 + j][1] = size[0] / 2 * (1 - cornerRounded) + Mathf.Cos(disAngle)  * size[0] / 2 * cornerRounded;
                    }
                }
                else if(i ==1)
                {
                    if (originPoints[i * seg / 4 + j][1] >= size[0] / 2 * (1 - cornerRounded))
                    {
                        float disAngle = Mathf.Deg2Rad * 45 * ( originPoints[i * seg / 4 + j][1] - size[1] / 2 * (1 - cornerRounded) ) / (size[0] / 2) / cornerRounded;
                        originPoints[i * seg / 4 + j][0] = size[0] / 2 * (1 - cornerRounded) + Mathf.Cos(disAngle)  * size[0] / 2 * cornerRounded;
                        originPoints[i * seg / 4 + j][1] = size[1] / 2 * (1 - cornerRounded) + Mathf.Sin(disAngle)  * size[0] / 2 * cornerRounded;
                    }
                    else if (originPoints[i * seg / 4 + j][1] <= -size[0] / 2 * (1 - cornerRounded))
                    {
                        float disAngle = Mathf.Deg2Rad * 45 * (-size[0] / 2 * (1 - cornerRounded) - originPoints[i * seg / 4 + j][1]) / (size[0] / 2) / cornerRounded;
                        originPoints[i * seg / 4 + j][0] =  size[0] / 2 * (1 - cornerRounded) + Mathf.Cos(disAngle)  * size[0] / 2 * cornerRounded;
                        originPoints[i * seg / 4 + j][1] = -size[0] / 2 * (1 - cornerRounded) - Mathf.Sin(disAngle)  * size[0] / 2 * cornerRounded;
                    }
                }
                else if(i == 2)
                {
                    if (originPoints[i * seg / 4 + j][0] >= size[0] / 2 * (1 - cornerRounded))
                    {
                        float disAngle = Mathf.Deg2Rad * 45 * (originPoints[i * seg / 4 + j][0] - size[0] / 2 * (1 - cornerRounded)) / (size[0] / 2) / cornerRounded;
                        originPoints[i * seg / 4 + j][0] =  size[0] / 2 * (1 - cornerRounded) + Mathf.Sin(disAngle)  * size[0] / 2 * cornerRounded;
                        originPoints[i * seg / 4 + j][1] = -size[1] / 2 * (1 - cornerRounded) - Mathf.Cos(disAngle)  * size[0] / 2 * cornerRounded;
                    }
                    else if (originPoints[i * seg / 4 + j][0] <= -size[0] / 2 * (1 - cornerRounded))
                    {
                        float disAngle = Mathf.Deg2Rad * 45 * (-size[0] / 2 * (1 - cornerRounded) - originPoints[i * seg / 4 + j][0]) / (size[0] / 2) / cornerRounded;
                        originPoints[i * seg / 4 + j][0] = -size[0] / 2 * (1 - cornerRounded) - Mathf.Sin(disAngle)  * size[0] / 2 * cornerRounded;
                        originPoints[i * seg / 4 + j][1] = -size[0] / 2 * (1 - cornerRounded) - Mathf.Cos(disAngle)  * size[0] / 2 * cornerRounded;
                    }
                }
                else if (i == 3)
                {
                    if (originPoints[i * seg / 4 + j][1] <= -size[0] / 2 * (1 - cornerRounded))
                    {
                        float disAngle = Mathf.Deg2Rad * 45 * (-size[0] / 2 * (1 - cornerRounded) - originPoints[i * seg / 4 + j][1]) / (size[0] / 2) / cornerRounded;
                        originPoints[i * seg / 4 + j][0] = -size[0] / 2 * (1 - cornerRounded) - Mathf.Cos(disAngle) * size[0] / 2 * cornerRounded;
                        originPoints[i * seg / 4 + j][1] = -size[1] / 2 * (1 - cornerRounded) - Mathf.Sin(disAngle) * size[0] / 2 * cornerRounded;
                    }
                    else if (originPoints[i * seg / 4 + j][1] >= size[0] / 2 * (1 - cornerRounded))
                    {
                        float disAngle = Mathf.Deg2Rad * 45 * (originPoints[i * seg / 4 + j][1] - size[0] / 2 * (1 - cornerRounded)) / (size[0] / 2) / cornerRounded;
                        originPoints[i * seg / 4 + j][0] = -size[0] / 2 * (1 - cornerRounded) - Mathf.Cos(disAngle) * size[0] / 2 * cornerRounded;
                        originPoints[i * seg / 4 + j][1] =  size[0] / 2 * (1 - cornerRounded) + Mathf.Sin(disAngle) * size[0] / 2 * cornerRounded;
                    }
                }
            }
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
        for (int i = 0; i < seg; i++)
        {
            float x = Mathf.Cos(staticWaveAngle) * amplitude;
            float y = Mathf.Sin(staticWaveAngle) * amplitude;
            float z = 0;
            Vector3 wave = new Vector3(x, y, z);

            afterWave[i] = originPoints[i] + wave;
            //renderPos[i] = afterShrink[i];

            staticWaveAngle += (Mathf.Deg2Rad * 360 / seg * waveSegments);
        }

        // 添加随机波动
        for (int i = 0; i < seg; i++)
        {
            float x = Mathf.Cos(randomWaveAngle) * randomWaveAmp;
            float y = Mathf.Sin(randomWaveAngle) * randomWaveAmp;
            float z = 0;
            Vector3 wave = new Vector3(x, y, z);

            afterWave[i] += wave;

            randomWaveAngle += (Mathf.Deg2Rad * 360 / seg * randomWaveSeg);
        }
    }
}
