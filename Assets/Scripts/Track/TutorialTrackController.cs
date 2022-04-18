using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrackController : MonoBehaviour
{
    [Header("教学环节MovePoint")]
    public MovePoint TutorialMovePoint;

    [Header("教学部分BGM")]
    public AudioClip TutorialBGM;

    [Header("一个循环小节时间")]
    public float timeOfOneBar = 1f;


    /// <summary>
    /// 获取教学部分BGM一个小节的剩余播放时间
    /// </summary>
    /// <returns></returns>
    private float GetLastOneBarPlayingTime()
    {
        //获取

        return 0;
    }

    /// <summary>
    /// 结束教学环节并开始游戏
    /// </summary>
    public void FinishTutorial()
    {
        float time = GetLastOneBarPlayingTime();

        TutorialMovePoint.timeToNextMovePoint = time + timeOfOneBar;

        LevelController.Instance.StartLevel(); //从教学起始点开始

        StartCoroutine(LateChangeBGM(time));
    }

    private IEnumerator LateChangeBGM(float time)
    {
        yield return new WaitForSeconds(time);

        // 切换主音乐

    }


}
