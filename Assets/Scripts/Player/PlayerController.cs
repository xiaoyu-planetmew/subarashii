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
    [HideInInspector] public bool startPlaying = false;
    [HideInInspector] public int comboNum = 0; //Combo ��
    private float originBlood;
    private Vector3 originPos;
    public int totalPoints;
    public int successPoints;

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
    void Update()
    {
        GameObject.Find("Score_Letter").GetComponent<ArtLetter>().SetShowNumber(successPoints);
        if(totalPoints != 0)
        {
            GameObject.Find("Level_Letters").GetComponent<ArtLetter_Percent>().SetShowNumber((float)successPoints/totalPoints);
        }
    }
    public void success()
    {
        totalPoints++;
        successPoints++;
    }
    public void miss()
    {
        totalPoints++;
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
        comboNum = 0;
        CharacterAnimationController.Instance.GetComponent<Animator>().SetBool("game over", false);
    }
}
