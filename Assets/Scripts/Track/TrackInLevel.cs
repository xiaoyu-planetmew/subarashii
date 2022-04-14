using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelController))]
public class TrackInLevel : MonoBehaviour
{
    [Header("���ֿ�ʼ�ڵ�")]
    public MovePoint startPoint;

    [Header("���ֽ����ڵ�")]
    public MovePoint endPoint;

    [Header("�����ļ���")]
    public string trackFile;
}
