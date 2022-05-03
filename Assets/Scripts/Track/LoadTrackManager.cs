using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class LoadTrackManager : BaseManager<LoadTrackManager>
{

    /// <summary>
    /// 各个关卡的音乐时间文件
    /// </summary>
    public Dictionary<string, List<float>> trackTimeDic = new Dictionary<string, List<float>>();
    public Dictionary<string, List<float>> trackTotalTimeDic = new Dictionary<string, List<float>>();

    /// <summary>
    /// 各个关卡的各个打点的名字
    /// </summary>
    public Dictionary<string, List<KeyDirectionType>> trackDirDic = new Dictionary<string, List<KeyDirectionType>>();

    public void LoadAllTrackAssetsConfig()
    {
        KoreographyTrackBase[] aTrackManagers = Resources.LoadAll<KoreographyTrackBase>("Tracks/");
        for (int i = 0; i < aTrackManagers.Length; ++i)
        {

            Debug.Log("Loaded Track " + aTrackManagers[i].name);

            List<KoreographyEvent> eventList = aTrackManagers[i].GetAllEvents();
            List<float> eventTimeTrack = new List<float>();
            List<float> eventTotalTimeTrack = new List<float>();
            List<KeyDirectionType> eventDir = new List<KeyDirectionType>();
            for (int j = 0; j < eventList.Count; j++)
            {
                if(j==0)
                {
                    eventTimeTrack.Add((float)eventList[j].StartSample / TrackManager.Instance.sampleRate);
                    eventTotalTimeTrack.Add((float)eventList[j].StartSample / TrackManager.Instance.sampleRate);

                }
                else
                {
                    eventTimeTrack.Add((float)eventList[j].StartSample / TrackManager.Instance.sampleRate - (float)eventList[j - 1].StartSample / TrackManager.Instance.sampleRate);
                    eventTotalTimeTrack.Add((float)eventList[j].StartSample / TrackManager.Instance.sampleRate);
                }

                eventDir.Add(GetKeyDir(eventList[j].GetTextValue(), i, j));
            }

            trackTimeDic.Add(aTrackManagers[i].name, eventTimeTrack);
            trackTotalTimeDic.Add(aTrackManagers[i].name, eventTotalTimeTrack);
            trackDirDic.Add(aTrackManagers[i].name, eventDir);
        }
    }

    private KeyDirectionType GetKeyDir(string keyDirName, int fileIndex, int index)
    {
        foreach (KeyDirectionType key in Enum.GetValues(typeof(KeyDirectionType)))
        {
            if (key.ToString() == keyDirName)
                return key;
        }

        Debug.LogError("Track谱面Text文字错误！第" + fileIndex +"个文件  第" + index +"处");
        return KeyDirectionType.Null;
    }
}
