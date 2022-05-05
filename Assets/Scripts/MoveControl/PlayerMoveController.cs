using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [Header("点之间的离散数量")]
    public int lineSegmentNum = 150;

    [Header("成功时冲刺速度")]
    public float acceratedSpeed = 3f;

    public static PlayerMoveController Instance;

    private List<Vector3> movePoints; //离散化的待移动点
    private float moveDeltaTime; //离散点之间的移动时间
    private float originMoveDeltaTime;
    private bool startMove;
    private float timer;
    private float aTimer;
    private MovePoint nowMovePoint; //当前正在走的
    private float mpSegTotalTime;
    private float aSpeed;
    private int nowPoint;
    private float aveDeltaTime;
    private bool waitForOneFrame;
    private int accelerateNum;

    [SerializeField] private bool accelerated; //是否加速冲刺
    private bool finishedAccelerate;
    private float saveAcceleratedTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        movePoints = new List<Vector3>();
        ResetMoveController();
        accelerated = false;
        timer = 0;
        aTimer = 0;
        aveDeltaTime = Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //根据点和时间移动
        if(startMove && !LevelController.Instance.isPausing)
        {
            MoveByPoints();
        }

        aveDeltaTime = (Time.fixedDeltaTime + aveDeltaTime) / 2;
    }

    private void MoveByPoints()
    {
        if(waitForOneFrame)
        {
            waitForOneFrame = false;
            return;
        }

        if (LevelController.Instance.finishThisLevel) accelerated = false;

        timer += Time.fixedDeltaTime;
        //aTimer += (Time.unscaledDeltaTime * (accelerated ? aSpeed : 1));
        aTimer += Time.fixedDeltaTime * aSpeed;

        float progress = aTimer / moveDeltaTime ;
         
        nowPoint = Mathf.Min(Mathf.FloorToInt(progress), movePoints.Count - 2);
        transform.position = movePoints[nowPoint];


        //动态计算速度
        if(accelerated)
        {
            accelerateNum++;

            aSpeed = aSpeed - 0.1f / nowMovePoint.timeToNextMovePoint;
            Debug.Log("动态速度减小到 " + aSpeed);

            float leftTime = nowMovePoint.nextPoint.timeInTrack - LevelController.Instance.mainMusicPlayingTimer;
            float leftATime = aSpeed * moveDeltaTime * (movePoints.Count - nowPoint);

            //Debug.Log("now left time " + leftTime);
            //Debug.Log("now left A time " + leftATime);

            if ((accelerateNum >= 30) || aSpeed <= 0)
            {
                accelerated = false;
                if(movePoints.Count - nowPoint > 0)
                {
                    //aSpeed = (mpSegTotalTime - timer + saveAcceleratedTime) / aveDeltaTime / (movePoints.Count - nowPoint);
                    //aSpeed = (nowMovePoint.nextPoint.timeInTrack - LevelController.Instance.mainMusicPlayingTimer) / originMoveDeltaTime / (movePoints.Count - nowPoint);
                    aSpeed =   (moveDeltaTime * (movePoints.Count - nowPoint)) / (nowMovePoint.nextPoint.timeInTrack - LevelController.Instance.mainMusicPlayingTimer);
                    Debug.Log("匀速 " + aSpeed + "剩余时间" + (nowMovePoint.nextPoint.timeInTrack - LevelController.Instance.mainMusicPlayingTimer));
                }
                finishedAccelerate = true;
                Debug.Log("恢复匀速");
            }
        }

        if(nowPoint >= movePoints.Count - 2)
        {
            //到最后一个点
            ArrivedLastPoint();
        }
    }

    /// <summary>
    /// 到最后一个点
    /// </summary>
    private void ArrivedLastPoint()
    {
        timer = 0;
        aTimer = 0;
        startMove = false;
        nowPoint = 0;

        if (finishedAccelerate)
        {
            finishedAccelerate = false;
            aSpeed = 1;
            saveAcceleratedTime = 0;
        }

        if (nowMovePoint.nextPoint != null)
        {
            //计算下一段路线
            MoveToPoint(nowMovePoint.GetNextOrBranchPoint());
        }
        else
        {
            PlayerController.Instance.startPlaying = false;
        }

        if(!accelerated)
        {
            if (nowMovePoint.nextPoint != null && (nowMovePoint.nextPoint.timeInTrack - LevelController.Instance.mainMusicPlayingTimer) > 0)
            {
                aSpeed = nowMovePoint.timeToNextMovePoint / (nowMovePoint.nextPoint.timeInTrack - LevelController.Instance.mainMusicPlayingTimer);
                Debug.Log("到下一节点时间 " + (nowMovePoint.nextPoint.timeInTrack - LevelController.Instance.mainMusicPlayingTimer)
                + "固定时间 " + nowMovePoint.timeToNextMovePoint + " 节点动态速度 " + aSpeed);
            }

            
        }
        
    }

    //冲刺
    public void AccerateMove()
    {
        accelerated = true;
        aSpeed = acceratedSpeed;
        finishedAccelerate = false;
        saveAcceleratedTime = 0;
        accelerateNum = 0;

        //在前一个结尾处加速时
        if (nowPoint> movePoints.Count * 0.8)
        {
            //保存时间
            //saveAcceleratedTime = mpSegTotalTime - timer;

            //跳过阶段
            //ArrivedLastPoint();

            //waitForOneFrame = true;
        }

    }

    /// <summary>
    /// 在目标时间里，移动到下一个点
    /// </summary>
    /// <param name="thisMovePoint">目标点</param>
    public void MoveToPoint(MovePoint thisMovePoint)
    {
        nowMovePoint = thisMovePoint;

        // 计算到下一个点的曲线的所有中间点
        movePoints.Clear();
        CalculateAllNextPoints(thisMovePoint.basePoints);

        // 计算每步时间
        mpSegTotalTime = thisMovePoint.timeToNextMovePoint;
        originMoveDeltaTime = thisMovePoint.timeToNextMovePoint / movePoints.Count;
        moveDeltaTime = originMoveDeltaTime;

        // 开始
        startMove = true;
    }

    /// <summary>
    /// 计算到下一个点的曲线的所有中间点
    /// </summary>
    private void CalculateAllNextPoints(Vector3[] basePoints)
    {
        Vector3[] vector3s = PathControlPointGenerator(basePoints);
        int SmoothAmount = basePoints.Length * lineSegmentNum;
        for (int i = 1; i < SmoothAmount; i++)
        {
            float pm = (float)i / SmoothAmount;
            Vector3 currPt = Interp(vector3s, pm);
            movePoints.Add(currPt);
        }
    }

    /// <summary>
    /// 计算所有节点以及控制点坐标
    /// </summary>
    /// <param name="path">所有节点的存储数组</param>
    /// <returns></returns>
    public Vector3[] PathControlPointGenerator(Vector3[] path)
    {
        Vector3[] suppliedPath;
        Vector3[] vector3s;

        suppliedPath = path;
        int offset = 2;
        vector3s = new Vector3[suppliedPath.Length + offset];
        Array.Copy(suppliedPath, 0, vector3s, 1, suppliedPath.Length);
        vector3s[0] = vector3s[1] + (vector3s[1] - vector3s[2]);
        vector3s[vector3s.Length - 1] = vector3s[vector3s.Length - 2] + (vector3s[vector3s.Length - 2] - vector3s[vector3s.Length - 3]);
        if (vector3s[1] == vector3s[vector3s.Length - 2])
        {
            Vector3[] tmpLoopSpline = new Vector3[vector3s.Length];
            Array.Copy(vector3s, tmpLoopSpline, vector3s.Length);
            tmpLoopSpline[0] = tmpLoopSpline[tmpLoopSpline.Length - 3];
            tmpLoopSpline[tmpLoopSpline.Length - 1] = tmpLoopSpline[2];
            vector3s = new Vector3[tmpLoopSpline.Length];
            Array.Copy(tmpLoopSpline, vector3s, tmpLoopSpline.Length);
        }
        return (vector3s);
    }

    /// <summary>
    /// 计算曲线的任意点的位置
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public Vector3 Interp(Vector3[] pos, float t)
    {
        int length = pos.Length - 3;
        int currPt = Mathf.Min(Mathf.FloorToInt(t * length), length - 1);
        float u = t * (float)length - (float)currPt;
        Vector3 a = pos[currPt];
        Vector3 b = pos[currPt + 1];
        Vector3 c = pos[currPt + 2];
        Vector3 d = pos[currPt + 3];
        return .5f * (
           (-a + 3f * b - 3f * c + d) * (u * u * u)
           + (2f * a - 5f * b + 4f * c - d) * (u * u)
           + (-a + c) * u
           + 2f * b
       );
    }


    public void ResetMoveController()
    {
        movePoints.Clear();
        startMove = false;
        timer = 0;
        aTimer = 0;
        startMove = false;
        nowPoint = 0;
        accelerated = false;
        finishedAccelerate = false;
        aSpeed = 1;
        saveAcceleratedTime = 0;
        mpSegTotalTime = 0;
    }
}
