using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainLine : MonoBehaviour
{
    public List<Transform> lines = new List<Transform>();
    public int nowLine = 0;
    public float moveTime;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in this.transform)
        {
            lines.Add(child);
        }
    }
    public void pointMove()
    {
        
        GameObject.Find("player").GetComponent<DrawBesizerLine>().initializeLine();
        nowLine++;
        StopAllCoroutines();
        StartCoroutine(move());
    }

    IEnumerator move()
    {
        yield return new WaitForSeconds(moveTime);
        
        lines[nowLine].GetComponent<pointClick>().pointActive = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
