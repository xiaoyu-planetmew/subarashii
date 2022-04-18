using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ѫ��")]
    public float blood = 3;

    [Header("���¿�ʼλ��")]
    public Transform restartPos;


    public static PlayerController Instance;
    [HideInInspector] public bool startPlaying;
    private float originBlood;
    private Vector3 originPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        originBlood = blood;
        startPlaying = false;

        if (restartPos != null)
            originPos = restartPos.position;
        else
            originPos = transform.position;
    }

    /// <summary>
    /// ��Ѫ
    /// </summary>
    /// <param name="addBlood"></param>
    public void AddBlood(float addBlood = 1)
    {
        blood = Mathf.Min(blood+addBlood, originBlood);

        // ��Ѫ��Ч

    }

    /// <summary>
    /// ��Ѫ
    /// </summary>
    public void MinusBlood(float minusBlood = 1)
    {
        blood = Mathf.Max(blood - minusBlood, 0);

        // ��Ѫ��Ч

        // ����
        if(blood<=0)
        {
            LevelController.Instance.GameOver();
        }
    }

    public void ResetPlayer()
    {
        blood = originBlood;
        transform.position = originPos;
        startPlaying = false;
    }
}