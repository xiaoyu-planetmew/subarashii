using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSys : MonoBehaviour
{
    [Header("eventNum")]
    public int eventNum;
    public GameObject player;
    public GameObject npc;
    public GameObject cam;
    public GameObject textLabelleft;
    public GameObject textLabelright;
    public GameObject textBackgroundLeft;
    public GameObject textBackgroundRight;
    public GameObject startButton;
    public GameObject nextPageButton;
    public GameObject sceneTransButton;
    public List<TextAsset> textfiles = new List<TextAsset>();
    public List<TextAsset> textfilesE = new List<TextAsset>();
    public List<TextAsset> textfilesJ = new List<TextAsset>();
    public bool firstMeet;
    public bool isTalking;
    //private bool holdTarget;
    public int index;
    public List<string> textList = new List<string>();
    public List<string> textTalker = new List<string>(); 
    public float textSpeed;
    public List<AudioClip> leftAudio = new List<AudioClip>();
    public List<AudioClip> rightAudio = new List<AudioClip>();
    bool textFinished;
    public string output;
    int textNum;
    // Start is called before the first frame update
    void Start()
    {
        firstMeet = true;        
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if((Mathf.Abs(npc.transform.position.x - player.transform.position.x) <= 5) && !isTalking)
        {
            startButton.SetActive(true);
        }
        if((Mathf.Abs(npc.transform.position.x - player.transform.position.x) > 5))
        {
            startButton.SetActive(false);
        }
        */
    }
    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        textTalker.Clear();
        var lineData = file.text.Split('#');        
        foreach(var line in lineData)
        {
            //Debug.Log(line);
            textList.Add(line);
        }
        //Debug.Log(textList.Count);
        textList.RemoveAt(textList.Count - 1);
        //textList.RemoveAt(textList.Count);
        for(int j = 0; j < textList.Count; j++)
        {
            if(textList[j][0] == '1')
            {
                textList[j] = textList[j].Substring(1);
                textTalker.Add("left");
            }
            if(textList[j][0] == '2')
            {
                textList[j] = textList[j].Substring(1);
                textTalker.Add("right");
            }
        }
    }
    /*
    public void languageChange()
    {
        textfiles.Clear();
        if(GameManager.instance.languageNum == 0)
        {
            for(int i=0; i<textfilesJ.Count; i++)
            {
                textfiles.Add(textfilesJ[i]);
            }
        }
        if(GameManager.instance.languageNum == 1)
        {
            for(int i=0; i<textfilesE.Count; i++)
            {
                textfiles.Add(textfilesE[i]);
            }
        }
    }
    */
    public void fileChoose()
    {
        
            /*
            for (int i = 0; i < GameManager.instance.items.Count; i++)
            {
                if(GameManager.instance.items[i] == target)
                {
                    GetTextFromFile(textfiles[1]);
                    holdTarget = true;
                }
                else
                {
                    GetTextFromFile(textfiles[2]);
                    holdTarget = false;
                }
            }
            */
        
        //startButton.gameObject.SetActive(false);
        //nextPageButton.gameObject.SetActive(true);
        //textLabelcn.gameObject.SetActive(true);
        //textLabelen.gameObject.SetActive(true);
        index = 0;
        if(textTalker[0] == "left")
        {
            textBackgroundLeft.gameObject.SetActive(true);
            //textLabelleft.GetComponent<Text>().text = textList[index];
            StartCoroutine(SetTextLeft());
            leftAudioRandom();
        }
        if(textTalker[0] == "right")
        {
            textBackgroundRight.gameObject.SetActive(true);
            //textLabelright.GetComponent<Text>().text = textList[index];
            
            StartCoroutine(SetTextRight());
            rightAudioRandom();
        }
        isTalking = true;
        //Time.timeScale = 0.0f;
        //GameManager.instance.isPaused = true;
        
        //textLabelcn.GetComponent<TMP_Text>().text = textList[index];
        //textLabelen.GetComponent<TMP_Text>().text = textList[index + 1];
        
        startButton.SetActive(false);
    }    
    void leftAudioRandom()
    {
        this.GetComponent<AudioSource>().enabled = false;
        this.GetComponent<AudioSource>().enabled = true;
        
        StopCoroutine("audioChangeLeft");
        StopCoroutine("audioChangeRight");
        StartCoroutine(audioStop());
        
        //Debug.Log(i);
        if(!this.GetComponent<AudioSource>().isPlaying && this.GetComponent<AudioSource>().enabled == true)
        {
            StartCoroutine(audioChangeLeft());
        }
        
    }
    void rightAudioRandom()
    {
        
        this.GetComponent<AudioSource>().enabled = false;
        this.GetComponent<AudioSource>().enabled = true;
        
        StopCoroutine("audioChangeLeft");
        StopCoroutine("audioChangeRight");
        StartCoroutine(audioStop());
        
        //Debug.Log(i);
        if(!this.GetComponent<AudioSource>().isPlaying && this.GetComponent<AudioSource>().enabled == true)
        {
            StartCoroutine(audioChangeRight());
        }
    }
    
    IEnumerator SetTextLeft()
    {
        //Debug.Log(textList[index].Length);
        textFinished = false;
        textLabelleft.GetComponent<Text>().text = "";
        for(int i = 0; i < textList[index].Length; i++)
        {
            //Debug.Log(i);
            textLabelleft.GetComponent<Text>().text += textList[index][i];
            yield return new WaitForSeconds(textSpeed);
        }
        index = index + 1;
        textFinished = true;
    }
    IEnumerator SetTextRight()
    {
        textFinished = false;
        textLabelright.GetComponent<Text>().text = "";
        //Debug.Log(textList[index].Length);
        for(int i = 0; i < textList[index].Length; i++)
        {
            //Debug.Log(i);
            textLabelright.GetComponent<Text>().text += textList[index][i];
            yield return new WaitForSeconds(textSpeed);
        }
        index = index + 1;
        textFinished = true;
    }
    IEnumerator audioStop()
    {
        yield return new WaitForSeconds(((textList[index].Length) * textSpeed));
        this.GetComponent<AudioSource>().Stop();
        this.GetComponent<AudioSource>().enabled = false;
        StopCoroutine("audioChangeLeft");
        StopCoroutine("audioChangeRight");
    }
    IEnumerator audioChangeLeft()
    {
        int i = Random.Range(0, leftAudio.Count);
        while(this.GetComponent<AudioSource>().enabled == true && textTalker[index] == "left")
        {        
            i = Random.Range(0, leftAudio.Count);
            this.GetComponent<AudioSource>().clip = leftAudio[i];
            this.GetComponent<AudioSource>().Play();
            //Debug.Log("left");
            yield return new WaitForSeconds(leftAudio[i].length);
        }
        
        //StartCoroutine(audioChangeLeft());
    }
    IEnumerator audioChangeRight()
    {
        int i = Random.Range(0, rightAudio.Count);
        while(this.GetComponent<AudioSource>().enabled == true && textTalker[index] == "right")
        {        
            i = Random.Range(0, rightAudio.Count);
            this.GetComponent<AudioSource>().clip = rightAudio[i];
            this.GetComponent<AudioSource>().Play();
            //Debug.Log("right");
            yield return new WaitForSeconds(rightAudio[i].length);
        }
        
        //StartCoroutine(audioChangeRight());
    }
}
