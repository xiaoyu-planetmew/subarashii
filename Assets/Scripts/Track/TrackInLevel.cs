using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelController))]
public class TrackInLevel : MonoBehaviour
{
    [Header("音乐开始节点")]
    public MovePoint startPoint;

    [Header("音乐结束节点")]
    public MovePoint endPoint;

    [Header("音轨文件名")]
    public string trackFile;
}
