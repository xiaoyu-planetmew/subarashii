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
            for(int i = 0; i<trackTimeList.Count;i++)
            {
                if (i == 0)
                    _mp.timeToNextMovePoint = trackTimeList[0];
                else
                {
                    if (_mp.nextPoint == null)
                    {
                        Debug.LogError("Track点数多于场景中的点数！");
                        return;
                    }

                    _mp = _mp.nextPoint;
                    _mp.timeToNextMovePoint = trackTimeList[i];
                }
            }

            if(_mp!=track.endPoint) Debug.LogError("Track点数少于场景中的点数！");
        }

        
    }
}
