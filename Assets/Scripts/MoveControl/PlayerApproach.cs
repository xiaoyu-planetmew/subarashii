using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerApproach : MonoBehaviour
{
    public List<Transform> children = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            children.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) > 30)
        {
            for(int i=0; i<children.Count; i++)
            {
                children[i].gameObject.SetActive(false);
            }
        }else{
            for(int i=0; i<children.Count; i++)
            {
                children[i].gameObject.SetActive(true);
            }
        }
    }
}
