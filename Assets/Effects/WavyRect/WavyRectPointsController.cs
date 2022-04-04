using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavyRectPointsController : MonoBehaviour
{
    public Vector2 size = new Vector2(100f,100f);
    public int segmentNum = 40;


    private void Start()
    {
        
    }

    private void InitialateRect()
    {
        int seg = Mathf.Abs(segmentNum / 4) * 4;


    }
}
