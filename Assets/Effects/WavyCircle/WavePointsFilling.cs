using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WavePointsFilling : MonoBehaviour
{
    private MeshFilter filter;

    private void Start()
    {
        filter = GetComponent<MeshFilter>();
    }
    
    public void FillingPoints(Vector3[] linePoints)
    {
        Vector3[] filledGraphPoints = new Vector3[linePoints.Length + 1];

        filledGraphPoints[0] = Vector3.zero;
        for (int index = 1; index < linePoints.Length + 1; ++index)
        {
            filledGraphPoints[index] = linePoints[index - 1];
        }

        int[] triangles = new int[linePoints.Length * 3];

        for (int t = 1; t < linePoints.Length + 1; t++)
        {
            triangles[(t - 1) * 3] = 0;
            triangles[(t - 1) * 3 + 1] = t;
            triangles[(t - 1) * 3 + 2] = t % linePoints.Length + 1;
        }

        // create mesh
        Mesh filledGraphMesh = new Mesh();
        filledGraphMesh.vertices = filledGraphPoints;
        filledGraphMesh.triangles = triangles;

        // set filter
        filter.mesh.Clear();
        filter.mesh = filledGraphMesh;
    }
}
