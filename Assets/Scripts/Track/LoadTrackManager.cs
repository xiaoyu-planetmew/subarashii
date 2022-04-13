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

    public void LoadAllTrackAssetsConfig()
    {
        KoreographyTrackBase[] aTrackManagers = Resources.LoadAll<KoreographyTrackBase>("Tracks/");

        for (int i = 0; i < aTrackManagers.Length; ++i)
        {

            Debug.Log("Loaded Track " + aTrackManagers[i].name);

            List<KoreographyEvent> eventList = aTrackManagers[i].GetAllEvents();
            List<float> eventTimeTrack = new List<float>();
            for (int j = 0; j < eventList.Count; j++)
            {
                if(j==0) 
                    eventTimeTrack.Add((float)eventList[j].StartSample / TrackManager.Instance.sampleRate);
                else
                    eventTimeTrack.Add((float)eventList[j].StartSample/TrackManager.Instance.sampleRate - (float)eventList[j-1].StartSample / TrackManager.Instance.sampleRate);

            }

            trackTimeDic.Add(aTrackManagers[i].name, eventTimeTrack);
        }
    }
}
