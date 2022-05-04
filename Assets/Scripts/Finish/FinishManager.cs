using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        
    }
    public void FinishPlaying(int levelNum, int hearts, int score, int total)
    {
        GameObject.Find("Player").GetComponent<PlayerController>().startPlaying = false;
        FinishCanvas.SetActive(true);
        level.sprite = levels[levelNum];
        if(hearts != 0)
        {
            finishBackground.sprite = finishBackgrounds[0];
            finishClass.gameObject.SetActive(true);
            finishImage.sprite = finishImages[0];
            
            if(hearts == 1)
            {
                finishClass.sprite = finishClasses[0];
                HPHearts.GetChild(0).gameObject.SetActive(true);
                HPHearts.GetChild(1).gameObject.SetActive(false);
                HPHearts.GetChild(2).gameObject.SetActive(false);
            }
            if(hearts == 2)
            {
                finishClass.sprite = finishClasses[1];
                HPHearts.GetChild(0).gameObject.SetActive(true);
                HPHearts.GetChild(1).gameObject.SetActive(true);
                HPHearts.GetChild(2).gameObject.SetActive(false);
            }
            if(hearts == 3)
            {
                finishClass.sprite = finishClasses[2];
                HPHearts.GetChild(0).gameObject.SetActive(true);
                HPHearts.GetChild(1).gameObject.SetActive(true);
                HPHearts.GetChild(2).gameObject.SetActive(true);
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
}
