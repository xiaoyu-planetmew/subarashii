using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrackController : MonoBehaviour
{
    [Header("��ѧ����BGM")]
    public AudioClip TutorialBGM;

    [Header("һ��ѭ��С��ʱ��")]
    public float timeOfOneBar = 1f;


    /// <summary>
    /// ��ȡ��ѧ����BGMһ��С�ڵ�ʣ�ಥ��ʱ��
    /// </summary>
    /// <returns></returns>
    private float GetLastOneBarPlayingTime()
    {
        //��ȡ

        return 0;
    }

    /// <summary>
    /// ������ѧ���ڲ���ʼ��Ϸ
    /// </summary>
    public void FinishTutorial()
    {
        float time = GetLastOneBarPlayingTime();

        LevelController.Instance.startMovePoint.timeToNextMovePoint = time + timeOfOneBar;

        PlayerMoveController.Instance.MoveToPoint(LevelController.Instance.startMovePoint);

        StartCoroutine(LateChangeBGM(time));
    }

    private IEnumerator LateChangeBGM(float time)
    {
        yield return new WaitForSeconds(time);

        // �л�������
    }


}
