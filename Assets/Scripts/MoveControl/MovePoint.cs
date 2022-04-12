using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    [Header("�ƶ��ڵ����")]

    [Tooltip("�ƶ����˵��ʱ��")] 
    public float timeToNextMovePoint = 1;

    [Tooltip("���ӵ���һ���ڵ�")] 
    public MovePoint nextPoint;

    [Tooltip("�ƶ�����һ���ڵ���м䱴������, ���ƶ�˳������")] 
    public Transform[] besizerControlPoints;


    [HideInInspector] public Vector3[] basePoints;

    private void Start()
    {
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
}







