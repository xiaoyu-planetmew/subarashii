using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [Header("��ǰ�ؿ�")]
    public Level level = Level.Level_1;

    [Header("�ؿ������¼�")]
    public UnityEvent sceneChangeEvents;

    [Header("�ؿ������¼�")]
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

