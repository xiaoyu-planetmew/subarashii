using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sceneControl : MonoBehaviour
{
    public List<string> SceneList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void onClick(int s)
    {
        SceneManager.LoadScene(SceneList[s]);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
