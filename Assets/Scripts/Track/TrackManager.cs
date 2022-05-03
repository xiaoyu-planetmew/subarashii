using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [Header("采样率")]
    public long sampleRate = 48000;

    public static TrackManager Instance;

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

        LoadTrackManager.GetInstance().LoadAllTrackAssetsConfig();
    }

    public void InitiateLevelTrack()
    {
        TrackInLevel[] trackcontrollers = LevelController.Instance.trackController;
        if (trackcontrollers.Length == 0) return;

        foreach(TrackInLevel track in trackcontrollers)
        {
            MovePoint _mp = track.startPoint;
            if (!LoadTrackManager.GetInstance().trackTimeDic.ContainsKey(track.trackFile))
            {
                Debug.LogError("ERROR IN TRACK FILE NAME");
                return;
            }
            List<float> trackTimeList = LoadTrackManager.GetInstance().trackTimeDic[track.trackFile];
            List<float> trackTimeTotal = LoadTrackManager.GetInstance().trackTotalTimeDic[track.trackFile];
            for(int i = 0; i<trackTimeList.Count;i++)
            {
                if (i == 0)
                {
                    // 时间
                    _mp.timeToNextMovePoint = trackTimeList[0];

                    //显示
                    _mp.GetComponent<MovePointDisplay>().InitiateDisplay();
                }
                else
                {
                    if (_mp.nextPoint == null)
                    {
                        Debug.LogError("Track点数多于场景中的点数！");
                        return;
                    }

                    _mp = _mp.nextPoint;

                    // 时间
                    _mp.timeToNextMovePoint = trackTimeList[i];
                    _mp.timeInTrack = trackTimeTotal[i-1];

                    // 输入
                    _mp.GetComponent<MovePointInputController>().keyInput.keyInput = LoadTrackManager.GetInstance().trackDirDic[track.trackFile][i-1];

                    //显示
                    _mp.GetComponent<MovePointDisplay>().InitiateDisplay();

                    //Debug.Log("Track Initiated No." + i);
                }

            }

            if(_mp!=track.endPoint) Debug.LogError("Track点数少于场景中的点数！");
        }

        
    }
}
