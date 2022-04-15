using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [Header("��֮�����ɢ����")]
    public int lineSegmentNum = 150;

    public static PlayerMoveController Instance;

    private List<Vector3> movePoints; //��ɢ���Ĵ��ƶ���
    private float moveDeltaTime; //��ɢ��֮����ƶ�ʱ��
    private bool startMove;
    private float timer;
    private MovePoint nowMovePoint; //��ǰ�����ߵ�

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
        //���ݵ��ʱ���ƶ�
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
            //�����һ����
            timer = 0;
            startMove = false;

            if(nowMovePoint.nextPoint != null)
            {
                //������һ��·��
                MoveToPoint(nowMovePoint.GetNextOrBranchPoint());
            }
        }
    }

    /// <summary>
    /// ��Ŀ��ʱ����ƶ�����һ����
    /// </summary>
    /// <param name="thisMovePoint">Ŀ���</param>
    public void MoveToPoint(MovePoint thisMovePoint)
    {
        nowMovePoint = thisMovePoint;

        // ���㵽��һ��������ߵ������м��
        movePoints.Clear();
        CalculateAllNextPoints(thisMovePoint.basePoints);

        // ����ÿ��ʱ��
        moveDeltaTime = thisMovePoint.timeToNextMovePoint / movePoints.Count;

        // ��ʼ
        startMove = true;
    }

    /// <summary>
    /// ���㵽��һ��������ߵ������м��
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
    /// �������нڵ��Լ����Ƶ�����
    /// </summary>
    /// <param name="path">���нڵ�Ĵ洢����</param>
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
    /// �������ߵ�������λ��
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
