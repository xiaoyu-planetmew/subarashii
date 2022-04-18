using System;

using System.Collections;

using System.Collections.Generic;

using System.Xml;

using UnityEngine;

public class WwiseManager : MonoBehaviour

{
    public static WwiseManager Instance;
    const float minRTPCValue = 0f;
    const float maxRTPCValue = 100f;
    GameObject globalGameObject;
    private void Awake()
    {
        Instance = this;
    }
    public void Init()
    {
        globalGameObject = new GameObject("GlobalAudio");
        GameObject.DontDestroyOnLoad(globalGameObject);
        AkSoundEngine.RegisterGameObj(globalGameObject, globalGameObject.name);
    }
    public void LoadBank(string name)
    {
        if(String.IsNullOrEmpty(name))
        {
            Debug.LogError("AudioServiceImpl LoadBank name is Null");
            return;
        }
        AkBankManager.LoadBank(name, false, false);
    }
    public void UnLoadBank(string name)
    {
        if(String.IsNullOrEmpty(name))
        {
            Debug.LogError("AudioServiceImpl UnLoadBank name is Null");
            return;
        }
        AkBankManager.UnloadBank(name);
    }
    public void Play(string name)
    {
        if(String.IsNullOrEmpty(name))
        {
            Debug.LogError("AudioServiceImpl Play name is Null");
            return;
        }
        Play(name, globalGameObject);
    }
    public void Play(string name, GameObject go)
    {
        if(String.IsNullOrEmpty(name))
        {
            Debug.LogError("AudioServiceImpl Play name is Null");
            return;
        }
        AkSoundEngine.PostEvent(name, go);
    }
    public void PlayAtGameObjectPosition(string name, GameObject go)
    {
        if(String.IsNullOrEmpty(name))
        {
            Debug.LogError("AudioServiceImpl PlayAtGameObjectPosition name is Null");
            return;
        }
        if(go == null)
        {
            Debug.LogError("AudioServiceImpl PlayAtGameObjectPosition go is Null");
            return;
        }
        if(AkSoundEngine.RegisterGameObj(go, go.name) != AKRESULT.AK_Success)
        {
            Debug.LogError("AudioServiceImpl PlayAtGameObjectPosition go is Register Failed");
            return;
        }
        AkSoundEngine.SetObjectPosition(
            go,
            go.transform.position.x,
            go.transform.position.y,
            go.transform.position.z,
            go.transform.forward.x,
            go.transform.forward.y,
            go.transform.forward.z,
            go.transform.up.x,
            go.transform.up.y,
            go.transform.up.z);
        Play(name, go);
        AkSoundEngine.UnregisterGameObj(go);
    }
    public void Stop(string name)
    {
        if(String.IsNullOrEmpty(name))
        {
            Debug.LogError("AudioServiceImpl Stop name is Null");
            return;
        }
        Stop(name, globalGameObject);
    }
    public void Stop(string name, GameObject go)
    {
        if(String.IsNullOrEmpty(name))
        {
            Debug.LogError("AudioServiceImpl Stop name is Null");
            return;
        }
        AkSoundEngine.ExecuteActionOnEvent(name, AkActionOnEventType.AkActionOnEventType_Stop);
    }
    public void SetRTPC(string rtpc, float value)
    {
        if(String.IsNullOrEmpty(rtpc))
        {
            Debug.LogError("AudioServiceImpl SetRTPC rtpc is Null");
            return;
        }
        AkSoundEngine.SetRTPCValue(rtpc, Mathf.Clamp(value, minRTPCValue, maxRTPCValue));
    }
    public void SetState(string group, string state)
    {
        if(String.IsNullOrEmpty(group))
        {
            Debug.LogError("AudioServiceImpl SetState group is Null");
            return;
        }
        if(String.IsNullOrEmpty(state))
        {
            Debug.LogError("AudioServiceImpl SetState state is Null");
            return;
        }
        AkSoundEngine.SetState(group, state);
    }
    public void SetSwitch(string group, string value, GameObject go)
    {
        if(String.IsNullOrEmpty(group))
        {
            Debug.LogError("AudioServiceImpl SetSwitch group is Null");
            return;
        }
        if(String.IsNullOrEmpty(value))
        {
            Debug.LogError("AudioServiceImpl SetSwitch value is Null");
            return;
        }
        if(go == null)
        {
            Debug.LogError("AudioServiceImpl SetSwitch go is Null");
            return;
        }
        AkSoundEngine.SetSwitch(group, value, go);
    }
    
}
