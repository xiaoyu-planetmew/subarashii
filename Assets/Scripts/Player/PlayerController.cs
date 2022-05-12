using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("Ѫ��")]
    public int blood = 3;

    [Header("���¿�ʼλ��")]
    public Transform restartPos;
    public Transform HP;

    public static PlayerController Instance;
    [HideInInspector] public bool startPlaying = false;
    [HideInInspector] public int comboNum = 0; //Combo ��
    private int originBlood;
    private Vector3 originPos;
    public int totalPoints;
    public int successPoints;
    public int totalMovePoints;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        originBlood = blood;
        startPlaying = false;
        totalMovePoints = LevelController.Instance.startMovePoint.transform.parent.childCount;
        if (restartPos != null)
            originPos = restartPos.position;
        else
            originPos = transform.position;
    }
    void Update()
    {
        switch(blood)
        {
            case 0:
            {
                HP.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                HP.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                HP.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                break;
            }
            case 1:
            {
                HP.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                HP.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                HP.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                break;
            }
            case 2:
            {
                HP.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                HP.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                HP.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                break;
            }
            case 3:
            {
                HP.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                HP.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                HP.GetChild(2).gameObject.GetComponent<Image>().enabled = true;
                break;
            }
        }
        try
        {
            GameObject.Find("Score_Letter").GetComponent<ArtLetter>().SetShowNumber(successPoints);
            if (totalPoints != 0)
            {
                GameObject.Find("Level_Letters").GetComponent<ArtLetter_Percent>().SetShowNumber((float)totalPoints / totalMovePoints);
            }
        }
        catch { }
    }
    public void Success(bool addBlood = false)
    {
        if (!startPlaying) return;

        totalPoints++;
        successPoints++;

        if (addBlood) AddBlood();
    }
    public void Miss()
    {
        if (!startPlaying) return;

        totalPoints++;
        MinusBlood();
    }
    /// <summary>
    /// ��Ѫ
    /// </summary>
    /// <param name="addBlood"></param>
    public void AddBlood(int addBlood = 1)
    {
        blood = Mathf.Min(blood+addBlood, originBlood);

        // ��Ѫ��Ч

    }

    /// <summary>
    /// ��Ѫ
    /// </summary>
    public void MinusBlood(int minusBlood = 1)
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
        totalPoints = 0;
        successPoints = 0;
    }
}
