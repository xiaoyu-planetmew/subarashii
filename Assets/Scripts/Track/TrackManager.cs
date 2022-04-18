using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [Header("������")]
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
                {
                    // ʱ��
                    _mp.timeToNextMovePoint = trackTimeList[0];

                    //��ʾ
                    _mp.GetComponent<MovePointDisplay>().InitiateDisplay();
                }
                else
                {
                    if (_mp.nextPoint == null)
                    {
                        Debug.LogError("Track�������ڳ����еĵ�����");
                        return;
                    }

                    _mp = _mp.nextPoint;

                    // ʱ��
                    _mp.timeToNextMovePoint = trackTimeList[i];

                    // ����
                    _mp.GetComponent<MovePointInputController>().keyInput.keyInput = LoadTrackManager.GetInstance().trackDirDic[track.trackFile][i];

                    //��ʾ
                    _mp.GetComponent<MovePointDisplay>().InitiateDisplay();
                }

            }

            if(_mp!=track.endPoint) Debug.LogError("Track�������ڳ����еĵ�����");
        }

        
    }
}
