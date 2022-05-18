using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FinishManager : MonoBehaviour
{
    public static FinishManager Instance;
    public GameObject FinishCanvas;
    public Image level;
    public List<Sprite> levels = new List<Sprite>();
    public Image finishBackground;
    
    public List<Sprite> finishBackgrounds = new List<Sprite>();
    public Image finishImage;
    public List<Sprite> finishImages = new List<Sprite>();
    public Image finishClass;
    public List<Sprite> finishClasses = new List<Sprite>();
    public Transform HPHearts;
    public Text scoreNum;
    public Text clearNum;
    public Text missNum;
    public GameObject Stuff;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.L))
        {
            FinishPlaying(1, 1, 1, 1);

        }
#endif
    }
    public void FinishPlaying(int levelNum, int hearts, int score, int total)
    {
        //GameObject.Find("Player").GetComponent<PlayerController>().startPlaying = false;
        //LevelController.Instance.Pause();
        FinishCanvas.SetActive(true);
        level.sprite = levels[levelNum];
        if(hearts != 0)
        {
            finishBackground.sprite = finishBackgrounds[0];
            finishClass.gameObject.SetActive(true);
            finishImage.sprite = finishImages[0];
            if(total-score == 0)
            {
                finishClass.sprite = finishClasses[2];
            }
            if(total-score == 1 || total-score == 2 || total-score == 3)
            {
                finishClass.sprite = finishClasses[1];
            }
            if(total-score >= 4)
            {
                finishClass.sprite = finishClasses[0];
            }
            if(hearts == 1)
            {
                //finishClass.sprite = finishClasses[0];
                HPHearts.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                HPHearts.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
                HPHearts.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
            }
            if(hearts == 2)
            {
                //finishClass.sprite = finishClasses[1];
                HPHearts.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                HPHearts.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                HPHearts.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
            }
            if(hearts == 3)
            {
                //finishClass.sprite = finishClasses[2];
                HPHearts.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                HPHearts.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                HPHearts.GetChild(2).gameObject.GetComponent<Image>().enabled = true;
            }
        }
        if(hearts == 0)
        {
            finishBackground.sprite = finishBackgrounds[1];
            finishClass.gameObject.SetActive(false);
            finishImage.sprite = finishImages[1];
            HPHearts.GetChild(0).gameObject.SetActive(false);
            HPHearts.GetChild(1).gameObject.SetActive(false);
            HPHearts.GetChild(2).gameObject.SetActive(false);
        }
        scoreNum.text = score.ToString();
        clearNum.text = score.ToString();
        missNum.text = (total-score).ToString();
    }
    public void Level5Finish()
    {
        DialogSys.Instance.dialogStart(0);
        PlayerController.Instance.startPlaying = false;
    }
    public void ShowStuff()
    {
        //DialogSys.Instance.dialogStart(0);
        PlayerController.Instance.startPlaying = false;
        Stuff.SetActive(true);
        SoundController.Instance.Final_Shit.HandleEvent(WwiseManager.Instance.gameObject);
        Stuff.GetComponent<Image>().DOFade(1, 5);
        StartCoroutine(StuffButton());
    }
    IEnumerator StuffButton()
    {
        yield return new WaitForSeconds(5f);
        Stuff.transform.GetChild(0).gameObject.SetActive(true);
    }
}
