using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    //[Header("�ƶ��ڵ����")]

    //[Tooltip("���ӵ���һ���ڵ�")] 
    public MovePoint nextPoint;

    //[Tooltip("֧�߽ڵ�")]
    public MovePoint branchPoint;

    //[Tooltip("�ƶ�����һ���ڵ���м䱴������, ���ƶ�˳������")] 
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

        //��ʼ��������
        InitiateBasePoints();

        //���ƽڵ���ʾͼ��

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

        active = false;

        InitiateBasePoints();

        //��������
        if(inputController != null)
        {
            inputController.ResetMovePointInput();
        }

        // ����MovePoint��ʾ����Ч
        if(displayController != null)
        {
            displayController.ResetMovePointDisplay();
        }
    }
}







