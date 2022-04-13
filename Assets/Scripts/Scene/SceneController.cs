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
    /// 切换关卡
    /// </summary>
    /// <param name="toLevel"></param>
    public void ChangeScene(Level toLevel)
    {
        // 切换关卡特效

        //音乐

        //前场景离开事件
        if (LevelController.Instance != null)
            LevelController.Instance.scenLeaveEvents.Invoke();

        //异步加载，等待加载完毕
        StartCoroutine(LateLoadNewScene(toLevel));
    }

    /// <summary>
    /// 异步加载
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
    /// 加载完毕后
    /// </summary>
    /// <param name="toLevel"></param>
    private void AfterLoadScene(Level toLevel)
    {
        Debug.Log("Change scene to " + toLevel.ToString());

        //音乐

        //场景进入事件
        LevelController.Instance.sceneChangeEvents.Invoke();

        //关闭特效

        //自动存档

        //加载Track
        TrackManager.Instance.InitiateLevelTrack();
    }
}
