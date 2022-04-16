using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalPointPosition : MonoBehaviour
{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = obj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
