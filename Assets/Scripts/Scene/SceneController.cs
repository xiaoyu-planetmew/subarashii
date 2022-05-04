using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void ChangeScene(string toLevel)
    {
        ChangeScene(LevelController.Instance.GetLevelType(toLevel));
    }

    /// <summary>
    /// �л��ؿ�
    /// </summary>
    /// <param name="toLevel"></param>
    public void ChangeScene(Level toLevel)
    {
        // �л��ؿ���Ч
        SceneTransition.Instance.EffectStart();

        //����
        WwiseManager.Instance.FadeOutAll(1.5f);

        StartCoroutine(WaitEffectComplete(toLevel));
    }

    private IEnumerator WaitEffectComplete(Level toLevel)
    {
        yield return new WaitForSeconds(2f);

        //ǰ�����뿪�¼�
        if (LevelController.Instance != null)
            LevelController.Instance.scenLeaveEvents.Invoke();

        StartCoroutine(WaitLeaveEvent(toLevel));
    }

    private IEnumerator WaitLeaveEvent(Level toLevel)
    {
        yield return new WaitForSeconds(0.1f);

        //�첽���أ��ȴ��������
        StartCoroutine(LateLoadNewScene(toLevel));
    }

    /// <summary>
    /// �첽����
    /// </summary>
    /// <param name="toLevel"></param>
    /// <returns></returns>
    private IEnumerator LateLoadNewScene(Level toLevel)
    {
        yield return new WaitForSeconds(0.1f);

        ScenesMgr.GetInstance().LoadSceneAsyn(toLevel.ToString(), () =>
        {
            AfterLoadScene(toLevel);
        });
    }

    /// <summary>
    /// ������Ϻ�
    /// </summary>
    /// <param name="toLevel"></param>
    private void AfterLoadScene(Level toLevel)
    {
        Debug.Log("Change scene to " + toLevel.ToString());
        
        // ����
        WwiseManager.Instance.FadeInAll(1.5f);

        StartCoroutine(WaitAfterLoad());
    }

    private IEnumerator WaitAfterLoad()
    {
        yield return new WaitForSeconds(0.1f);

        //���������¼�
        LevelController.Instance.sceneChangeEvents.Invoke();

        //�ر���Ч
        SceneTransition.Instance.EffectClose();

        //�Զ��浵

        //����Track
        //TrackManager.Instance.InitiateLevelTrack();
    }
}
