using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDisplay : MonoBehaviour
{
    public float hintTime = 1.5f;
    public ArrowPrefab[] arrows;
    public GameObject hint;

    public TipType Tip;

    private KeyDirectionType thisArrow;
    private ArrowPrefab showingArrow;
    private float timeToArriveMP;
    private MovePoint linkMP;
    private MovePointDisplay linkMPDisplay;
    [HideInInspector] public bool isNull = false;
    private bool hasEnter;

    private void Awake()
    {
        //ResetArrow();
    }

    private void Start()
    {
        linkMP = transform.parent.GetComponent<MovePoint>();
        linkMPDisplay = transform.parent.GetComponent<MovePointDisplay>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !isNull && !hasEnter && linkMP.active && PlayerController.Instance.startPlaying)
        {
            ChangeStartToArrow();

            timeToArriveMP = linkMP.timeInTrack - LevelController.Instance.mainMusicPlayingTimer;
            if(timeToArriveMP > hintTime)
            {
                Debug.Log("������Ȧ hintTime" + hintTime + " timeToArriveMP " + timeToArriveMP);
                //StartCoroutine(ShowInputHint(timeToArriveMP - hintTime, 1.5f));
                StartCoroutine(ShowInputHint(0, hintTime / timeToArriveMP * 1.5f));
            }
            else
            {
                if(timeToArriveMP > 0 )
                {
                    Debug.Log("���ٲ�����Ȧ " + hintTime / timeToArriveMP);
                    StartCoroutine(ShowInputHint(0, hintTime / timeToArriveMP));
                }
                else
                {
                    Debug.Log("С��0");
                    StartCoroutine(ShowInputHint(0, 3));
                }
                
            }

            hasEnter = true;
        }
    }

    public void ChangeStartToArrow()
    {
        showingArrow.star.SetTrigger("StarChange");
    }

    public void SuccessAnimation()
    {
        showingArrow.showArrow.SetTrigger("Success");
        showingArrow.hint_Special.SetActive(false);
        showingArrow.hint_Normal.SetActive(false);
        showingArrow.hint_Special.GetComponent<Animator>().speed = 1;
        showingArrow.hint_Normal.GetComponent<Animator>().speed = 1;
    }

    public void FailureAnimation()
    {
        showingArrow.showArrow.SetTrigger("Fail");
        showingArrow.hint_Special.SetActive(false);
        showingArrow.hint_Normal.SetActive(false);
        showingArrow.hint_Special.GetComponent<Animator>().speed = 1;
        showingArrow.hint_Normal.GetComponent<Animator>().speed = 1;
    }

    private IEnumerator ShowInputHint(float waitTime, float speed = 1f)
    {
        yield return new WaitForSeconds(waitTime);

        if(linkMPDisplay.specialHint)
        {
            showingArrow.hint_Special.SetActive(true);
            showingArrow.hint_Special.GetComponent<Animator>().speed = speed;
        }
        else
        {
            showingArrow.hint_Normal.SetActive(true);
            showingArrow.hint_Normal.GetComponent<Animator>().speed = speed;
        }

        //c播放combo timinig同时播放tip的声音
        if(linkMPDisplay.ComboTiming)
        {
            GameObject.Find("comboTime").GetComponent<Animator>().SetTrigger("Show");

            switch (Tip)
            {
                case TipType.Tip_Triple:
                    SoundController.Instance.Tip_Triple.HandleEvent(gameObject);
                    break;
                case TipType.Tip_Double:
                    SoundController.Instance.Tip_Double.HandleEvent(gameObject);
                    break;
                case TipType.Tip_TripleChange:
                    SoundController.Instance.Tip_TripleChange.HandleEvent(gameObject);
                    break;
            }
        }

    }


    public void ResetArrow()
    {
        // �ָ��ɳ�ʼ����ʾ

        if(showingArrow!=null)
        {
            showingArrow.hint_Special.GetComponent<Animator>().speed = 1;
            showingArrow.hint_Normal.GetComponent<Animator>().speed = 1;

            showingArrow.showArrow.SetTrigger("Idle");

            showingArrow.star.gameObject.SetActive(true);
            showingArrow.showArrow.gameObject.SetActive(false);
        }

        hasEnter = false;
    }

    public void InitiateArrow(KeyDirectionType type)
    {
        thisArrow = type;

        foreach (ArrowPrefab prefab in arrows)
        {
            if(type == prefab.type)
            {
                // ��ʼ����Ӧ��ͷ��ʾ
                showingArrow = prefab;
                showingArrow.baseArrow.SetActive(true);
                //ResetArrow()��
            }
        }

        hint.SetActive(false);

        if (type == KeyDirectionType.Null) isNull = true;
    }
}

[System.Serializable]
public class ArrowPrefab
{
    public KeyDirectionType type;
    public GameObject baseArrow;
    public Animator showArrow;
    public Animator star;
    public GameObject hint_Normal;
    public GameObject hint_Special;

}
public enum TipType
{
    Tip_Triple,
    Tip_Double,
    Tip_TripleChange,
}