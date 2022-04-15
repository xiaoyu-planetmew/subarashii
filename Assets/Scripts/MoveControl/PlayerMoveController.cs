using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [Header("点之间的离散数量")]
    public int lineSegmentNum = 150;

    public static PlayerMoveController Instance;

    private List<Vector3> movePoints; //离散化的待移动点
    private float moveDeltaTime; //离散点之间的移动时间
    private bool startMove;
    private float timer;
    private MovePoint nowMovePoint; //当前正在走的

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        movePoints = new List<Vector3>();
        ResetMoveController();
    }

    private void Update()
    {
        //根据点和时间移动
        if(startMove)
        {
            MoveByPoints();
        }
    }

    private void MoveByPoints()
    {
        timer += Time.deltaTime;
        int nowPoint = Mathf.Min(Mathf.FloorToInt(timer/moveDeltaTime), movePoints.Count - 2);
        transform.position = movePoints[nowPoint];
        
        if(nowPoint >= movePoints.Count - 2)
        {
            //到最后一个点
            timer = 0;
            startMove = false;

            if(nowMovePoint.nextPoint != null)
            {
                //计算下一段路线
                MoveToPoint(nowMovePoint.GetNextOrBranchPoint());
            }
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
        moveDeltaTime = thisMovePoint.timeToNextMovePoint / movePoints.Count;

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
    }
}
