using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDisplay : MonoBehaviour
{
    public float hintTime = 1.5f;
    public ArrowPrefab[] arrows;
    public GameObject hint;

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
        if(collision.tag == "Player" && !isNull && !hasEnter)
        {
            ChangeStartToArrow();

            timeToArriveMP = linkMP.timeInTrack - LevelController.Instance.mainMusicPlayingTimer;
            if(timeToArriveMP > hintTime)
            {
                Debug.Log("播放缩圈 hintTime" + hintTime + " timeToArriveMP " + timeToArriveMP);
                //StartCoroutine(ShowInputHint(timeToArriveMP - hintTime));
                StartCoroutine(ShowInputHint(0, hintTime / timeToArriveMP * 1.5f));
            }
            else
            {
                if(timeToArriveMP > 0 )
                {
                    Debug.Log("加速播放缩圈 " + hintTime / timeToArriveMP);
                    StartCoroutine(ShowInputHint(0, hintTime / timeToArriveMP));
                }
                else
                {
                    Debug.Log("小于0");
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


    }


    public void ResetArrow()
    {
        // 恢复成初始的显示
        showingArrow.hint_Special.GetComponent<Animator>().speed = 1;
        showingArrow.hint_Normal.GetComponent<Animator>().speed = 1;

        showingArrow.star.gameObject.SetActive(true);
        showingArrow.showArrow.gameObject.SetActive(false);

        hasEnter = false;
    }

    public void InitiateArrow(KeyDirectionType type)
    {
        thisArrow = type;

        foreach (ArrowPrefab prefab in arrows)
        {
            if(type == prefab.type)
            {
                // 初始化对应箭头显示
                showingArrow = prefab;
                showingArrow.baseArrow.SetActive(true);
                //ResetArrow()；
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
