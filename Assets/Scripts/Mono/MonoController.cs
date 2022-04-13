using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 公共Mono
/// 请使用MonoManager
/// </summary>
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;
    private event UnityAction fixedUpdateEvent;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(updateEvent != null)
        {
            updateEvent();
        }
    }
    void FixedUpdate()
    {
        if(fixedUpdateEvent != null)
        {
            fixedUpdateEvent();
        }
    }
    

    /// <summary>
    /// 添加帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    /// <summary>
    /// 移除帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
    
    /// <summary>
    /// 添加帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void AddFixedUpdateListener(UnityAction fun)
    {
        fixedUpdateEvent += fun;
    }

    /// <summary>
    /// 移除帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveFixedUpdateListener(UnityAction fun)
    {
        fixedUpdateEvent -= fun;
    }
}
