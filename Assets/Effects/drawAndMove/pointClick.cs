using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointClick : MonoBehaviour
{
    public bool pointActive = false;
    public List<KeyCode> keys = new List<KeyCode>();
    [SerializeField] List<bool> keyClicked = new List<bool>();
    [SerializeField] int nowKey = 0;
    public bool good = false;
    public bool miss = false;
    [SerializeField] float currentTime = 0f;
    float invokeTime;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<keys.Count; i++)
        {
            keyClicked.Add(false);
        }
        invokeTime = currentTime;
    }
    private void OnGUI() 
    {
        if(pointActive)
        {
            currentTime += Time.deltaTime;
            if(Input.anyKey)
            {
                if(currentTime < 0.5f) return;
                if(currentTime >= 0.5f && Event.current.keyCode != KeyCode.Mouse0 && Event.current.keyCode != KeyCode.Mouse1 && Event.current.keyCode != KeyCode.None)
                {
                    Debug.Log(Event.current.keyCode);
                    if(Event.current.keyCode != keys[nowKey])
                    {                        
                        pointActive = false;
                        miss = true;
                        GameObject.Find("MainLine").GetComponent<mainLine>().pointMove();
                        
                        currentTime = 0;
                        
                            Debug.Log("miss");
                    }
                    if(Event.current.keyCode == keys[nowKey])
                    {
                        if(nowKey == keys.Count - 1)
                        {                    
                            pointActive = false;
                            good = true;
                            GameObject.Find("MainLine").GetComponent<mainLine>().pointMove();
                            
                            currentTime = 0;
                            
                            Debug.Log("good");
                        }else{
                            
                            keyClicked[nowKey] = true;
                            nowKey++;
                            
                            currentTime = 0;
                            Debug.Log("next");
                        }
                    }
                }
            }
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
