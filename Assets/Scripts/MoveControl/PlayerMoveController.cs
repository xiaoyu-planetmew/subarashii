using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    //[Header("��֮�����ɢ����")]
    public int lineSegmentNum = 150;

    //[Header("�ɹ�ʱ����ٶ�")]
    public float acceratedSpeed = 3f;

    public static PlayerMoveController Instance;

    private List<Vector3> movePoints; //��ɢ���Ĵ��ƶ���
    private float moveDeltaTime; //��ɢ��֮����ƶ�ʱ��
    private float originMoveDeltaTime;
    private bool startMove;
    private float timer;
    private float aTimer;
    private MovePoint nowMovePoint; //��ǰ�����ߵ�
    private float mpSegTotalTime;
    private float aSpeed;
    private int nowPoint;
    private float aveDeltaTime;
    private bool waitForOneFrame;
    private int accelerateNum;

    [SerializeField] private bool accelerated; //�Ƿ���ٳ��
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
        //���ݵ��ʱ���ƶ�
        if(startMove && !LevelController.Instance.isPausing && PlayerController.Instance.startPlaying)
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


        //��̬�����ٶ�
        if(accelerated)
        {
            accelerateNum++;

            aSpeed = aSpeed - 0.1f / nowMovePoint.timeToNextMovePoint;
            Debug.Log("��̬�ٶȼ�С�� " + aSpeed);

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
                    
                    if (aSpeed <= 0) aSpeed = 1;

                    Debug.Log("���� " + aSpeed + "ʣ��ʱ��" + (nowMovePoint.nextPoint.timeInTrack - LevelController.Instance.mainMusicPlayingTimer));
                }
                finishedAccelerate = true;
                Debug.Log("�ָ�����");
            }
        }

        if(nowPoint >= movePoints.Count - 2)
        {
            //�����һ����
            ArrivedLastPoint();
        }
    }

    /// <summary>
    /// �����һ����
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
            //������һ��·��
            MoveToPoint(nowMovePoint.nextPoint);
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
                Debug.Log("����һ�ڵ�ʱ�� " + (nowMovePoint.nextPoint.timeInTrack - LevelController.Instance.mainMusicPlayingTimer)
                + "�̶�ʱ�� " + nowMovePoint.timeToNextMovePoint + " �ڵ㶯̬�ٶ� " + aSpeed);
            }

            
        }
        
    }

    //���
    public void AccerateMove()
    {
        accelerated = true;
        aSpeed = acceratedSpeed;
        finishedAccelerate = false;
        saveAcceleratedTime = 0;
        accelerateNum = 0;

        //��ǰһ����β������ʱ
        if (nowPoint> movePoints.Count * 0.8)
        {
            //����ʱ��
            //saveAcceleratedTime = mpSegTotalTime - timer;

            //�����׶�
            //ArrivedLastPoint();

            //waitForOneFrame = true;
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
        mpSegTotalTime = thisMovePoint.timeToNextMovePoint;
        originMoveDeltaTime = thisMovePoint.timeToNextMovePoint / movePoints.Count;
        moveDeltaTime = originMoveDeltaTime;

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
