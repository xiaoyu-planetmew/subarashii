using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    public CinemachineVirtualCamera MainCamera;
    public CinemachineVirtualCamera VCcam_LookUp;
    public CinemachineVirtualCamera VCcam_LookDown;
    public CinemachineVirtualCamera VCcam_LookLeft;
    public CinemachineVirtualCamera VCcam_LookRight;
    public CinemachineVirtualCamera VCcam_Nearby;
    public CinemachineVirtualCamera VCcam_Farfrom;
    public CinemachineVirtualCamera VCcam_LookUpLeft;
    public CinemachineVirtualCamera VCcam_LookUpRight;
    public CinemachineVirtualCamera VCcam_LookDownLeft;
    public CinemachineVirtualCamera VCcam_LookDownRight;
    public CinemachineVirtualCamera VCcam_Final;

    [HideInInspector] public Dictionary<CameraStatus, CinemachineVirtualCamera> camDic = new Dictionary<CameraStatus, CinemachineVirtualCamera>();

    public static CameraController Instance;
    private CinemachineVirtualCamera nowCam;

    private void Awake()
    {
        Instance = this;

        InitiateCameraToPlayer();
    }

    private void Start()
    {
        ResetCam();
    }

    private void InitiateCameraToPlayer()
    {
        camDic.Add(CameraStatus.Normal, MainCamera);
        camDic.Add(CameraStatus.LookUp, VCcam_LookUp);
        camDic.Add(CameraStatus.LookDown, VCcam_LookDown);
        camDic.Add(CameraStatus.LookLeft, VCcam_LookLeft);
        camDic.Add(CameraStatus.LookRight, VCcam_LookRight);
        camDic.Add(CameraStatus.Nearby, VCcam_Nearby);
        camDic.Add(CameraStatus.Farfrom, VCcam_Farfrom);
        camDic.Add(CameraStatus.LookUpLeft, VCcam_LookUpLeft);
        camDic.Add(CameraStatus.LookUpRight, VCcam_LookUpRight);
        camDic.Add(CameraStatus.LookDownLeft, VCcam_LookDownLeft);
        camDic.Add(CameraStatus.LookDownRight, VCcam_LookDownRight);
        camDic.Add(CameraStatus.Final, VCcam_Final);

        foreach (KeyValuePair<CameraStatus, CinemachineVirtualCamera> cam in camDic)
        {
            cam.Value.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    public void ResetCam()
    {
        foreach (KeyValuePair<CameraStatus, CinemachineVirtualCamera> cam in camDic)
        {
            cam.Value.gameObject.SetActive(false);
        }

        MainCamera.gameObject.SetActive(true);

        nowCam = MainCamera;
    }

    public void ChangeCameraStatus(CameraStatus toCam)
    {
        if (camDic[toCam] == nowCam) return;

        nowCam.gameObject.SetActive(false);
        nowCam = camDic[toCam];
        nowCam.gameObject.SetActive(true);
    }

}

public enum CameraStatus
{
    Normal,
    LookUp,
    LookDown,
    LookLeft,
    LookRight,
    LookUpLeft,
    LookUpRight,
    LookDownLeft,
    LookDownRight,
    Nearby,
    Farfrom,
    Final,
}
