using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientLineControl : MonoBehaviour
{
    private LineRenderer line;
    public List<Transform> dots = new List<Transform>();

    private void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        foreach (Transform child in this.transform)
        {
            dots.Add(child);
            child.GetComponent<SpriteRenderer>().enabled = false;
        }


        line.positionCount = dots.Count;

        for (int i = 0; i < dots.Count; i++)
        {
            line.SetPosition(i, dots[i].position);
        }
    }
}
