using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WavyRectLineRenderer : MonoBehaviour
{
    private LineRenderer line;
    private bool hasInit;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3[] points)
    {
        if(!hasInit)
        {
            InitLineRenderer(points.Length);
        }

        for (int i = 0; i < points.Length; i++)
        {
            line.SetPosition(i, points[i]);
        }
    }

    private void InitLineRenderer(int pointNum)
    {
        line.positionCount = pointNum;
        line.useWorldSpace = false;
        hasInit = true;
    }
}
