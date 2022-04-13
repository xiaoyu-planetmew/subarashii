using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换管理器
/// 采用单例
/// </summary>
public class ScenesMgr : BaseManager<ScenesMgr>
{
    /// <summary>
    /// 同步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// /// <param name="fun">委托方法</param>
    public void LoadScene(string name,UnityAction fun)
    {
        SceneManager.LoadScene(name);
        fun();
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun">委托方法</param>
    public void LoadSceneAsyn(string name, UnityAction fun)
    {
        MonoManager.GetInstance().StartCoroutine(ReallyLoadSceneAsyn(name,fun));
    }

    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            yield return ao.progress;
        }
        yield return ao;

        fun();
    }
    // Enum.GetName(typeof(UserRoleEnum),1)
}
