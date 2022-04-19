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

    public static CameraController Instance;
    private CinemachineVirtualCamera nowCam;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetCam();
    }

    public void ResetCam()
    {
        MainCamera.gameObject.SetActive(true);
        VCcam_LookUp.gameObject.SetActive(false);
        VCcam_LookDown.gameObject.SetActive(false);
        VCcam_LookLeft.gameObject.SetActive(false);
        VCcam_LookRight.gameObject.SetActive(false);
        VCcam_Nearby.gameObject.SetActive(false);
        VCcam_Farfrom.gameObject.SetActive(false);
        VCcam_LookUpLeft.gameObject.SetActive(false);
        VCcam_LookUpRight.gameObject.SetActive(false);
        VCcam_LookDownLeft.gameObject.SetActive(false);
        VCcam_LookDownRight.gameObject.SetActive(false);

        nowCam = MainCamera;
    }

    public void ChangeCameraStatus(CameraStatus toCam)
    {
        switch (toCam)
        {
            case CameraStatus.Normal:
                if (nowCam == MainCamera) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = MainCamera;
                    nowCam.gameObject.SetActive(true);
                }
                break;
            case CameraStatus.LookUp:
                if (nowCam == VCcam_LookUp) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = VCcam_LookUp;
                    nowCam.gameObject.SetActive(true);
                }
                break;
            case CameraStatus.LookDown:
                if (nowCam == VCcam_LookDown) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = VCcam_LookDown;
                    nowCam.gameObject.SetActive(true);
                }
                break;
            case CameraStatus.LookLeft:
                if (nowCam == VCcam_LookLeft) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = VCcam_LookLeft;
                    nowCam.gameObject.SetActive(true);
                }
                break;
            case CameraStatus.LookRight:
                if (nowCam == VCcam_LookRight) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = VCcam_LookRight;
                    nowCam.gameObject.SetActive(true);
                }
                break;
            case CameraStatus.Nearby:
                if (nowCam == VCcam_Nearby) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = VCcam_Nearby;
                    nowCam.gameObject.SetActive(true);
                }
                break;
            case CameraStatus.Farfrom:
                if (nowCam == VCcam_Farfrom) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = VCcam_Farfrom;
                    nowCam.gameObject.SetActive(true);
                }
                break;
            case CameraStatus.LookUpLeft:
                if (nowCam == VCcam_LookUpLeft) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = VCcam_LookUpLeft;
                    nowCam.gameObject.SetActive(true);
                }
                break;
            case CameraStatus.LookUpRight:
                if (nowCam == VCcam_LookUpRight) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = VCcam_LookUpRight;
                    nowCam.gameObject.SetActive(true);
                }
                break;
            case CameraStatus.LookDownLeft:
                if (nowCam == VCcam_LookDownLeft) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = VCcam_LookDownLeft;
                    nowCam.gameObject.SetActive(true);
                }
                break;
            case CameraStatus.LookDownRight:
                if (nowCam == VCcam_LookDownRight) return;
                else
                {
                    nowCam.gameObject.SetActive(false);
                    nowCam = VCcam_LookDownRight;
                    nowCam.gameObject.SetActive(true);
                }
                break;
        }

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
}
