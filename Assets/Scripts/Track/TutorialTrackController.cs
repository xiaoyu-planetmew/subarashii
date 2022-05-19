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
    /// ��ȡ��ѧ����BGMһ��С�ڵ�ʣ�ಥ��ʱ��
    /// </summary>
    /// <returns></returns>
    private float GetLastOneBarPlayingTime()
    {
        //��ȡʣ��С��ʱ��
        float leftTime = timeOfOneBar - (timer - Mathf.Floor(timer / timeOfOneBar) * timeOfOneBar);


        Debug.Log("�Ƴ�ʱ��" + leftTime + " �Ѳ�С�� "+ Mathf.Floor(timer / timeOfOneBar) + " ʱ��" + (timer - Mathf.Floor(timer / timeOfOneBar) * timeOfOneBar));


        return leftTime;
    }

    /// <summary>
    /// ������ѧ���ڲ���ʼ��Ϸ
    /// </summary>
    public void FinishTutorial()
    {
        //�л�����
        SwitchToMain.HandleEvent(WwiseManager.Instance.gameObject);

        // ͬ����Ϸ��ʼ
        StartCoroutine(SynGameStart(GetLastOneBarPlayingTime()));
    }

    /// <summary>
    /// ͬ����ʼ
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator SynGameStart(float time)
    {

        yield return new WaitForSeconds(time);

        LevelController.Instance.StartLevel(); //�ӽ�ѧ��ʼ�㿪ʼ


    }


}
