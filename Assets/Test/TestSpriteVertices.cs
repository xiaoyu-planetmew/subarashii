using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpriteVertices : MonoBehaviour
{
    Vector3[] linePoints;

    private void Start()
    {
        linePoints = new Vector3[3];
        CreatePoints();
        CreateFilledGraphShape(linePoints);
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;

        float ang = 0f;  // start from 0 degree

        for (int i = 0; i < 3; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * ang) * 4;
            y = Mathf.Cos(Mathf.Deg2Rad * ang) * 4;

            linePoints[i] = new Vector3(x, y, z);
            ang += (360f / 3);
        }
    }

    public void CreateFilledGraphShape(Vector3[] linePoints)
    {
        Vector3[] filledGraphPoints = new Vector3[linePoints.Length + 1];

        filledGraphPoints[0] = Vector3.zero;
        for (int index = 1; index < linePoints.Length + 1; ++index)
        {
            filledGraphPoints[index] = linePoints[index-1];
        }

        int[] triangles = new int[linePoints.Length * 3];

        for (int t = 1; t < linePoints.Length + 1; t ++)
        {
            triangles[(t-1) * 3    ] = 0;
            triangles[(t-1) * 3 + 1] = t;
            triangles[(t-1) * 3 + 2] = t % linePoints.Length + 1;
        }

        for(int i = 0; i<triangles.Length;i++)
        {
            Debug.Log(triangles[i]);
        }

        // create mesh
        Mesh filledGraphMesh = new Mesh();
        filledGraphMesh.vertices = filledGraphPoints;
        filledGraphMesh.triangles = triangles;
        // you might need to assign texture coordinates as well

        // create game object and add renderer and mesh to it
        //GameObject filledGraph = new GameObject("Filled graph");
        //MeshRenderer renderer = filledGraph.AddComponent<MeshRenderer>();
        //MeshFilter filter = filledGraph.AddComponent<MeshFilter>();
        //filter.mesh = filledGraphMesh;
        GetComponent<MeshFilter>().mesh = filledGraphMesh;
    }
}
