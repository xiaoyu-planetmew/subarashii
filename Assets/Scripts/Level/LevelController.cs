using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [Header("当前关卡")]
    public Level level = Level.Level_1;

    [Header("起始MovePoint(复活点)")]
    public MovePoint startMovePoint;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            StartLevel();
        }
    }

    private void InitiateLevel()
    {
        trackController = GetComponents<TrackInLevel>();
    }

    /// <summary>
    /// 开始本关卡游戏
    /// </summary>
    public void StartLevel()
    {
        PlayerController.Instance.startPlaying = true;
        if (startMovePoint != null)
        {
            PlayerMoveController.Instance.MoveToPoint(startMovePoint);
            //WwiseManager.Instance.Init();
            //WwiseManager.Instance.LoadBank("SoundBank");
            //WwiseManager.Instance.Play("Play_Music_level1_BPM100_32bit48khz");
            TestWwise.Instance.PlayMainMusic();
        }
        else
            Debug.LogError("Miss Start MovePoint in LevelController!");
        
    }

    public void ResetLevel()
    {
        // 重置所有MovePoint/ reset all MovePoints
        MovePoint[] mps = FindObjectsOfType<MovePoint>();
        foreach(MovePoint mp in mps)
        {
            mp.ResetMovePoint();
        }

        // 重置角色（位置、动画、血量） / reset Player(posisiton, display, blood)
        PlayerController.Instance.ResetPlayer();
        PlayerEffectController.Instance.ResetPlayerEffect();
        PlayerMoveController.Instance.ResetMoveController();
    }

    public void GameOver()
    {
        // 停止交互 / halt interacting
        PlayerController.Instance.startPlaying = false;

        // 角色死亡特效和动画 / death effect and animation

        // 主音乐切换 / main BGM change to tutorial BGM

        // 切换特效开 / scene changing effect ON

        // 等待特效全开，重置整个场景 / reset all level after effect been completed

        // 切换特效关  / turn scene changing effect OFF

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

