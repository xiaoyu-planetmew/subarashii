using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [Header("当前关卡")]
    public Level level = Level.Level_1_ver4;

    [Header("起始MovePoint(谱面时间0点)")]
    public MovePoint startMovePoint;

    [Header("关卡进入事件")]
    public UnityEvent sceneChangeEvents;

    [Header("关卡结束事件")]
    public UnityEvent scenLeaveEvents;

    public static LevelController Instance;
    [HideInInspector] public TrackInLevel[] trackController;
     public float mainMusicPlayingTimer;
    private bool startPlayingMainMusic;
    public GameObject pauseMenu;
    public bool finishThisLevel = false;
    public bool isPausing = false;

    private void Awake()
    {
        Instance = this;

        if(level!=Level.MainMenu)
            InitiateLevel();
    }

    private void Start()
    {
        if (level == Level.MainMenu) 
        {
            sceneChangeEvents.Invoke();

            Debug.Log("初始化 MainMenu");
            return; 
        }

        TrackManager.Instance.InitiateLevelTrack();
        startPlayingMainMusic = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !SceneController.Instance.sceneChanging)
        {
            if (!isPausing)
            {
                Pause();
                pauseMenu.SetActive(true);
            }
            else
            {
                Resume();
                pauseMenu.SetActive(false);
            }
        }

        if (startPlayingMainMusic)
            mainMusicPlayingTimer += Time.unscaledDeltaTime;
    }



    private void InitiateLevel()
    {
        trackController = GetComponents<TrackInLevel>();
    }

    public Level GetLevelType(string levelName)
    {
        foreach (Level level in Level.GetValues(typeof(Level)))
        {
            if (level.ToString() == levelName) return level;
        }

        return Level.MainMenu;

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

            startPlayingMainMusic = true;
        }
        else
            Debug.LogError("Miss Start MovePoint in LevelController!");
        
    }

    public void ResetLevel()
    {
        // 重置主音乐时间
        mainMusicPlayingTimer = 0;
        startPlayingMainMusic = false;

        // 重置所有MovePoint/ reset all MovePoints
        MovePoint[] mps = FindObjectsOfType<MovePoint>();
        foreach(MovePoint mp in mps)
        {
            mp.ResetMovePoint();
        }
        TrackInLevel.Instance.InitTrackMovePoint();

        //食物reset
        InteractiveAnimation[] interAnims = FindObjectsOfType<InteractiveAnimation>();
        foreach(InteractiveAnimation interAnim in interAnims)
        {
            interAnim.ResetAnimation();
        }

        // 重置角色（位置、动画、血量） / reset Player(posisiton, display, blood)
        PlayerController.Instance.ResetPlayer();
        PlayerEffectController.Instance.ResetPlayerEffect();
        PlayerMoveController.Instance.ResetMoveController();

        //重置相机
        CameraController.Instance.ResetCam();

        finishThisLevel = false;
    }

    public void GameOver()
    {
        // 停止交互 / halt interacting
        PlayerController.Instance.startPlaying = false;
        finishThisLevel = true;

        // 角色死亡特效和动画 / death effect and animation
        // 主角动画
        CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.GameOver);

        // 声音
        SoundController.Instance.GameOver.HandleEvent(WwiseManager.Instance.gameObject);
        
        //等待特效和动画播完 wait for playing animation and effect
        StartCoroutine(WaitAnimation());
        
    }

    private IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(1f);

        // 切换特效开 / scene changing effect ON
        SceneTransition.Instance.EffectStart();

        // 等待特效全开，重置整个场景 / reset all level after effect been completed
        StartCoroutine(WaitEffect());
    }

    
    private IEnumerator WaitEffect(float waitTime = 2f)
    {
        yield return new WaitForSeconds(waitTime);

        // 主音乐切换 / main BGM change to tutorial BGM
        TutorialTrackController.Instance.SwitchToTutorial.HandleEvent(WwiseManager.Instance.gameObject);
        //TutorialTrackController.Instance.timer = 0;

        // 重置 reset level
        ResetLevel();
        FinishManager.Instance.FinishCanvas.SetActive(false);

        StartCoroutine(WaitReset());
    }

    private IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(0.1f);

        // 切换特效关  / turn scene changing effect OFF
        SceneTransition.Instance.EffectClose();

        // 重开操作
        PlayerController.Instance.startPlaying = true;
    }
    public void Pause()
    {
        isPausing = true;
        startPlayingMainMusic = false;
        PlayerController.Instance.startPlaying = false;
        SoundController.Instance.PauseAll.HandleEvent(WwiseManager.Instance.gameObject);
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isPausing = false;
        startPlayingMainMusic = true;
        PlayerController.Instance.startPlaying = true;
        SoundController.Instance.ResumeAll.HandleEvent(WwiseManager.Instance.gameObject);
        Time.timeScale = 1;
    }

    public void PlayAgain()
    {
        // 切换特效开 / scene changing effect ON
        SceneTransition.Instance.EffectStart();

        // 等待特效全开，重置整个场景 / reset all level after effect been completed
        StartCoroutine(WaitEffect());
    }
}

public enum Level
{
    MainMenu = 0,
    Level_1_ver4 = 1,
    Level_2_ver1 = 2,
    Level_3_ver1 = 3,
    Level_4_ver1 = 4,
    Level_5_ver1 = 5,
    Level_1_Start = 6,
}

