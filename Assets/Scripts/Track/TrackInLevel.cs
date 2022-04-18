using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelController))]
public class TrackInLevel : MonoBehaviour
{
    [Header("���ֿ�ʼ�ڵ�(ʱ��Ϊ0�ĵ�)")]
    public MovePoint startPoint;

    [Header("���ֽ����ڵ㣨�����ڶ����㣩")]
    public MovePoint endPoint;

    [Header("�����ļ���")]
    public string trackFile;
}
