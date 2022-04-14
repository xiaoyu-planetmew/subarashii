using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 公共Mono管理器
/// 1、提供帧更新方法
/// 2、提供协程方法
/// 采用单例
/// </summary>
public class MonoManager : BaseManager<MonoManager>
{
    private MonoController controller;

    public MonoManager()
    {
        GameObject obj = new GameObject("MonoController");
        obj.AddComponent<MonoController>();
        controller = obj.GetComponent<MonoController>();
    }

    /// <summary>
    /// 添加帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }

    /// <summary>
    /// 移除帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }


    /// 添加帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void AddFixedUpdateListener(UnityAction fun)
    {
        controller.AddFixedUpdateListener(fun);
    }

    /// <summary>
    /// 移除帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveFixedUpdateListener(UnityAction fun)
    {
        controller.RemoveFixedUpdateListener(fun);
    }
    
    
    //协程方法(与MonoBehavior协程相同)
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }
    
    public Coroutine StartCoroutine(UnityAction u1 = null,float time = 0,UnityAction u2 =null)
    {
        
        return controller.StartCoroutine(coroEnumerator(u1,time,u2));
    }
    
    public void StopAllCoroutines()
    {
        controller.StopAllCoroutines();
    }

    public void StopCoroutine(IEnumerator routine)
    {
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine)
    {
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(string methodName)
    {
        controller.StopCoroutine(methodName);
    }

    IEnumerator coroEnumerator(UnityAction u1 = null,float time = 0,UnityAction u2 =null)
    {
        if (u1 != null)
            u1();
        yield return new WaitForSeconds(time);
        if (u2 != null)
            u2();
    }
}
