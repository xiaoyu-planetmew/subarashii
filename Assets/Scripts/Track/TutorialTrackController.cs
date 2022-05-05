using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrackController : MonoBehaviour
{
    public AkEvent StartMusic;
    public AkEvent SwitchToMain;
    public AkEvent SwitchToTutorial;
    public float timeOfOneBar = 1f;


    public static TutorialTrackController Instance;
    private bool startPlayingTutorial;
    public float timer;
    [SerializeField] private int nowBeat;

    private void Awake()
    {
        startPlayingTutorial = false;
        Instance = this;

    }

    private void Start()
    {
        StartCoroutine(LateStartPlayMusic());
    }

    private void Update()
    {
        if (startPlayingTutorial && !LevelController.Instance.isPausing)
            timer += Time.unscaledDeltaTime;

        // test
        if (startPlayingTutorial)
            nowBeat = (int) Mathf.Floor(timer / timeOfOneBar * 4);
    }

    public void StartPlayingMusicFromTutorial()
    {
        if(startPlayingTutorial == false)
        {
            startPlayingTutorial = true;
            StartMusic.HandleEvent(WwiseManager.Instance.gameObject);
            timer = 0;
        }
        
    }

    private IEnumerator LateStartPlayMusic()
    {
        yield return new WaitForSeconds(2f);

        StartPlayingMusicFromTutorial();
    }

    /// <summary>
    /// 获取教学部分BGM一个小节的剩余播放时间
    /// </summary>
    /// <returns></returns>
    private float GetLastOneBarPlayingTime()
    {
        //获取剩余小节时间
        float leftTime = timeOfOneBar - (timer - Mathf.Floor(timer / timeOfOneBar) * timeOfOneBar);


        Debug.Log("推迟时间" + leftTime + " 已播小节 "+ Mathf.Floor(timer / timeOfOneBar) + " 时间" + (timer - Mathf.Floor(timer / timeOfOneBar) * timeOfOneBar));


        return leftTime;
    }

    /// <summary>
    /// 结束教学环节并开始游戏
    /// </summary>
    public void FinishTutorial()
    {
        //切换音乐
        SwitchToMain.HandleEvent(WwiseManager.Instance.gameObject);

        // 同步游戏开始
        StartCoroutine(SynGameStart(GetLastOneBarPlayingTime()));
    }

    /// <summary>
    /// 同步开始
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator SynGameStart(float time)
    {

        yield return new WaitForSeconds(time);

        LevelController.Instance.StartLevel(); //从教学起始点开始


    }


}
