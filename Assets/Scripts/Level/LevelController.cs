using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [Header("��ǰ�ؿ�")]
    public Level level = Level.Level_1;

    [Header("��ʼMovePoint")]
    public MovePoint startMovePoint;

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
    /// ��ʼ���ؿ���Ϸ
    /// </summary>
    public void StartLevel()
    {
        PlayerController.Instance.startPlaying = true;
        if (startMovePoint != null)
        {
            PlayerMoveController.Instance.MoveToPoint(startMovePoint);
            WwiseManager.Instance.Init();
            WwiseManager.Instance.LoadBank("SoundBank");
            WwiseManager.Instance.Play("Play_Music_level1_BPM100_32bit48khz");
        }
        else
            Debug.LogError("Miss Start MovePoint in LevelController!");
        
    }

    public void ResetLevel()
    {
        // ��������MovePoint
        MovePoint[] mps = FindObjectsOfType<MovePoint>();
        foreach(MovePoint mp in mps)
        {
            mp.ResetMovePoint();
        }

        // ���ý�ɫ��λ�á�������Ѫ����
        PlayerController.Instance.ResetPlayer();
        PlayerEffectController.Instance.ResetPlayerEffect();
        PlayerMoveController.Instance.ResetMoveController();
    }

    public void GameOver()
    {
        // ֹͣ����
        PlayerController.Instance.startPlaying = false;

        // ��ɫ������Ч�Ͷ���

        // �л���Ч��

        // ������������

        // �л���Ч��
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

