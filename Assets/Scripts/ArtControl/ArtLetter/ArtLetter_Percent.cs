using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ArtLetter_Percent : MonoBehaviour
{
    public bool ShowZero = true;
    public Texture[] letters;
    public Texture dot;
    public Texture percent;
    public int num = 6;
    [Range(0,1)] public float lettersSize = 1;
    public float wordSpace;
    [SerializeField] private int showNumber;
    private int storedNumber;
    
    private RawImage[] images;
    private float localScaleX = 0.61f;
    private Animator[] anims;
    private bool startAddEffect = false;

    private void Start()
    {
        InitialNumbers();
        anims = GetComponentsInChildren<Animator>();
        showNumber = 0;
        storedNumber = 0;
        startAddEffect = false;
    }

    private void Update()
    {
        //SetLetters(lettersSize, wordSpace);
        //UpdateShowLetters();

        if(startAddEffect)
        {
            if(showNumber>=storedNumber)
            {
                showNumber = storedNumber;
                startAddEffect = false;
                //Debug.Log("到了");
                UpdateShowLetters();
                return;
            }

            showNumber += Mathf.Max(((int)(storedNumber-showNumber)/20),1);
            UpdateShowLetters();
        }
    }

    private void InitialNumbers()
    {
        
        images = new RawImage[num];
        for(int i = 0;i<num;i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ArtLetter"));
            images[i] = obj.GetComponent<RawImage>();
            obj.transform.SetParent(transform);
            obj.GetComponent<RectTransform>().localPosition = Vector3.left * (wordSpace) * i;
            obj.GetComponent<RectTransform>().localScale = new Vector3(localScaleX * lettersSize, lettersSize, 1);
        }
    }

    /// <summary>
    /// 更新显示的数字
    /// </summary>
    public void UpdateShowLetters()
    {
        int howLong = Mathf.FloorToInt(Mathf.Log10(showNumber))+1;
        //Debug.Log(howLong);

        for (int i = 0; i < num; i++)
        {
            images[i].texture = letters[Mathf.FloorToInt((showNumber / Mathf.Pow(10, i))) % 10];

            if(!ShowZero)
            {
                images[i].enabled = !(i > howLong - 1) ;
            }
        }
    }

    public void SetShowNumberWithEffect(int num)
    {
        storedNumber = num;
        startAddEffect = true;
    }

    public void SetShowNumber(int num)
    {
        
        showNumber = num;
        UpdateShowLetters();

    }

    /// <summary>
    /// 设置字间距和大小
    /// </summary>
    /// <param name="size"></param>
    /// <param name="space"></param>
    public void SetLetters(float size, float space)
    {
        for (int i = 0; i < num; i++)
        {
            images[i].gameObject.GetComponent<RectTransform>().localPosition = Vector3.left * (wordSpace) * i;
            images[i].gameObject.GetComponent<RectTransform>().localScale = new Vector3(localScaleX * lettersSize, lettersSize, 1);
        }
    }    

    public void Blink()
    {
        foreach(Animator anim in anims)
        {
            anim.SetTrigger("Blink");
        }
    }

    public void StartKeepBlink()
    {
        foreach (Animator anim in anims)
        {
            anim.SetBool("KeepBlink", true);
        }
    }

    public void StopKeepBlink()
    {
        foreach (Animator anim in anims)
        {
            anim.SetBool("KeepBlink", false);
        }
    }
}
