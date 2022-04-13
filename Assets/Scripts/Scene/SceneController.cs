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
        }
    }

    private void Start()
    {
        //test
        TrackManager.Instance.InitiateLevelTrack();
    }

    /// <summary>
    /// �л��ؿ�
    /// </summary>
    /// <param name="toLevel"></param>
    public void ChangeScene(Level toLevel)
    {
        // �л��ؿ���Ч

        //����

        //ǰ�����뿪�¼�
        if (LevelController.Instance != null)
            LevelController.Instance.scenLeaveEvents.Invoke();

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

        //����

        //���������¼�
        LevelController.Instance.sceneChangeEvents.Invoke();

        //�ر���Ч

        //�Զ��浵

        //����Track
        TrackManager.Instance.InitiateLevelTrack();
    }
}
