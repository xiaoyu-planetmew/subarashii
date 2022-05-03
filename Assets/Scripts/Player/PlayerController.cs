using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("血量")]
    public float blood = 3;

    [Header("重新开始位置")]
    public Transform restartPos;


    public static PlayerController Instance;
    [HideInInspector] public bool startPlaying = false;
    [HideInInspector] public int comboNum = 0; //Combo 数
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
    /// 加血
    /// </summary>
    /// <param name="addBlood"></param>
    public void AddBlood(float addBlood = 1)
    {
        blood = Mathf.Min(blood+addBlood, originBlood);

        // 加血特效

    }

    /// <summary>
    /// 扣血
    /// </summary>
    public void MinusBlood(float minusBlood = 1)
    {
        blood = Mathf.Max(blood - minusBlood, 0);

        // 扣血特效

        // 死亡
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
        comboNum = 0;
        CharacterAnimationController.Instance.GetComponent<Animator>().SetBool("game over", false);
    }
}
