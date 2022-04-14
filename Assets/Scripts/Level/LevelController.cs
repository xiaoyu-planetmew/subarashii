using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [Header("当前关卡")]
    public Level level = Level.Level_1;

    [Header("关卡进入事件")]
    public UnityEvent sceneChangeEvents;

    [Header("关卡结束事件")]
    public UnityEvent scenLeaveEvents;

    public static LevelController Instance;
    [HideInInspector] public TrackInLevel[] trackController;

    private void Awake()
    {
        Instance = this;

        InitiateLevel();
    }

    private void InitiateLevel()
    {
        trackController = GetComponents<TrackInLevel>();
    }

}

public enum Level
{
    Start,
    Level_1,
    Level_2,
    Level_3,
    Level_4,
    Level_5,

}

