using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式基类
/// </summary>
public class BaseManager<T> where T:new()
{
    private static T instance;

    /// <summary>
    /// 获取实例
    /// </summary>
    public static T GetInstance()
    {
        if(instance == null)
        {
            instance = new T();
        }
        return instance;
    }
}
