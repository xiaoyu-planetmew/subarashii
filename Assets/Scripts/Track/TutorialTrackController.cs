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
    private float timer;

    private void Awake()
    {
        startPlayingTutorial = false;
        Instance = this;

    }


    private void Update()
    {
        if(startPlayingTutorial)
            timer += Time.deltaTime;
    }

    public void StartPlayingMusicFromTutorial()
    {
        if(startPlayingTutorial == false)
        {
            startPlayingTutorial = true;
            StartMusic.HandleEvent(gameObject);
        }
        
    }

    /// <summary>
    /// ��ȡ��ѧ����BGMһ��С�ڵ�ʣ�ಥ��ʱ��
    /// </summary>
    /// <returns></returns>
    private float GetLastOneBarPlayingTime()
    {
        //��ȡʣ��С��ʱ��
        float leftTime = timer % timeOfOneBar;

        Debug.Log("ʣ��С��ʱ�� "+ leftTime);

        return leftTime;
    }

    /// <summary>
    /// ������ѧ���ڲ���ʼ��Ϸ
    /// </summary>
    public void FinishTutorial()
    {
        //�л�����
        SwitchToMain.HandleEvent(gameObject);

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
