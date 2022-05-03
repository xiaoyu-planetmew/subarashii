using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ArtLetter_Percent : MonoBehaviour
{
    public GameObject letter;
    public GameObject letter1;
    public GameObject p100;
    void Start()
    {
        //SetShowNumber(0f);
    }
    public void UpdateShowLetters()
    {
        letter.GetComponent<ArtLetter>().UpdateShowLetters();
        letter1.GetComponent<ArtLetter>().UpdateShowLetters();
    }

    public void SetShowNumberWithEffect(float num)
    {
        letter.GetComponent<ArtLetter>().SetShowNumberWithEffect(Mathf.FloorToInt(((num * 10000) / Mathf.Pow(10, 3))) % 10 * 10 + Mathf.FloorToInt(((num * 10000) / Mathf.Pow(10, 2))) % 10);
        letter1.GetComponent<ArtLetter>().SetShowNumberWithEffect(Mathf.FloorToInt(((num * 10000) / Mathf.Pow(10, 1))) % 10 * 10 + Mathf.FloorToInt(((num * 10000) / Mathf.Pow(10, 0))) % 10);
    }

    public void SetShowNumber(float num)
    {
        if(num == 1)
        {
            p100.SetActive(true);
            letter.SetActive(false);
            letter1.SetActive(false);
        }else{
            Debug.Log(num);
            p100.SetActive(false);
            letter.SetActive(true);
            letter1.SetActive(true);
            letter.GetComponent<ArtLetter>().SetShowNumber(Mathf.FloorToInt(((num * 10000) / Mathf.Pow(10, 3))) % 10 * 10 + Mathf.FloorToInt(((num * 10000) / Mathf.Pow(10, 2))) % 10);
            letter1.GetComponent<ArtLetter>().SetShowNumber(Mathf.FloorToInt(((num * 10000) / Mathf.Pow(10, 1))) % 10 * 10 + Mathf.FloorToInt(((num * 10000) / Mathf.Pow(10, 0))) % 10);
        }
    }

    /// <summary>
    /// 设置字间距和大小
    /// </summary>
    /// <param name="size"></param>
    /// <param name="space"></param>
    public void SetLetters(float size, float space)
    {
        letter.GetComponent<ArtLetter>().SetLetters(size, space);
        letter1.GetComponent<ArtLetter>().SetLetters(size, space);
    }    

    public void Blink()
    {
        letter.GetComponent<ArtLetter>().Blink();
        letter1.GetComponent<ArtLetter>().Blink();
    }

    public void StartKeepBlink()
    {
        letter.GetComponent<ArtLetter>().StartKeepBlink();
        letter1.GetComponent<ArtLetter>().StartKeepBlink();
    }

    public void StopKeepBlink()
    {
        letter.GetComponent<ArtLetter>().StopKeepBlink();
        letter1.GetComponent<ArtLetter>().StopKeepBlink();
    }
}
