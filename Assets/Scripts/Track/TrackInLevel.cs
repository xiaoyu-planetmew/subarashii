using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelController))]
public class TrackInLevel : MonoBehaviour
{
    [Header("音乐开始节点(时间为0的点)")]
    public MovePoint startPoint;

    [Header("音乐结束节点（倒数第二个点）")]
    public MovePoint endPoint;

    [Header("音轨文件名")]
    public string trackFile;

    public static TrackInLevel Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (endPoint.nextPoint != null)
            endPoint.nextPoint.gameObject.AddComponent<FinalCheckPoint>();
        else
            Debug.LogError("Teack In Level的倒数第二个点缺最后一个点");

        InitTrackMovePoint();
    }

    public void InitTrackMovePoint()
    {
        startPoint.active = true;
        startPoint.nextPoint.active = true;
        startPoint.nextPoint.nextPoint.active = true;
    }
}
