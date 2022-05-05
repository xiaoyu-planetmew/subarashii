using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    [Header("移动节点参数")]

    [Tooltip("链接到下一个节点")] 
    public MovePoint nextPoint;

    [Tooltip("支线节点")]
    public MovePoint branchPoint;

    [Tooltip("移动到下一个节点的中间贝塞尔点, 按移动顺序排列")] 
    public Transform[] besizerControlPoints;

    public float timeToNextMovePoint = 1;
    public float timeInTrack = 1;
    [HideInInspector] public Vector3[] basePoints;
    [HideInInspector] public bool toBranch;
    [HideInInspector] public MovePointDisplay displayController;
    [HideInInspector] public MovePointInputController inputController;
    [HideInInspector] public bool active = false;

    private void Awake()
    {
        displayController = GetComponent<MovePointDisplay>();

        inputController = GetComponent<MovePointInputController>();
    }

    private void Start()
    {
        toBranch = false;

        //初始化基础点
        InitiateBasePoints();

        //控制节点显示图案

    }

    private void InitiateBasePoints()
    {
        basePoints = new Vector3[besizerControlPoints.Length + 2];
        basePoints[0] = transform.position;
        if(besizerControlPoints.Length!=0)
        {
            for (int i = 1; i < besizerControlPoints.Length + 1 ; i++)
            {
                basePoints[i] = besizerControlPoints[i - 1].position;
            }
        }

        if(nextPoint!=null)
            basePoints[besizerControlPoints.Length + 1] = nextPoint.transform.position;
    }

    public MovePoint GetNextOrBranchPoint()
    {
        if (!toBranch || branchPoint != null)
            return nextPoint;
        else
        {
            ChangeToBranch();
            return branchPoint;
        }
    }

    public void ChangeToBranch()
    {
        if (branchPoint != null)
            basePoints[besizerControlPoints.Length + 1] = branchPoint.transform.position;
    }

    public void ResetMovePoint()
    {
        toBranch = false;
        InitiateBasePoints();

        //重置输入
        if(inputController != null)
        {
            inputController.ResetMovePointInput();
        }

        // 重置MovePoint显示和特效
        if(displayController != null)
        {
            displayController.ResetMovePointDisplay();
        }
    }
}







