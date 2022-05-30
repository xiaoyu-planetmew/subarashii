using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageImageChange : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public List<Font> fonts = new List<Font>();
    public List<int> size = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.GetComponent<Image>())
        {
            this.GetComponent<Image>().sprite = sprites[LanguageManager.Instance.LanguageNum];
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(sprites[LanguageManager.Instance.LanguageNum].bounds.size.x * 100, sprites[LanguageManager.Instance.LanguageNum].bounds.size.y * 100);
        }
        if(this.gameObject.GetComponent<Text>())
        {
            this.GetComponent<Text>().font = fonts[LanguageManager.Instance.LanguageNum];
            this.GetComponent<Text>().fontSize = size[LanguageManager.Instance.LanguageNum];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
