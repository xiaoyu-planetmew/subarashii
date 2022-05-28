using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;
    public LanguageType Language = LanguageType.Chinese;
    public int LanguageNum = 0;
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
    public void SetLanguage(LanguageType l)
    {
        Language = l;
        if(l == LanguageType.Chinese) LanguageNum = 0;
        if(l == LanguageType.English) LanguageNum = 1;
        if(l == LanguageType.Japanese) LanguageNum = 2;
    }
    public enum LanguageType
    {
        Chinese,
        English,
        Japanese,
    }
}
