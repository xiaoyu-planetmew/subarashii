using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJelly : MonoBehaviour
{
    public float elasticity = 1f;
    public float mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;

    private Mesh originalMesh, meshClone;
    private MeshRenderer renderer;
    private JellyVertex[] jvs;
    private Vector3[] vertexArray;

    private bool hasInitiated;

    private void Start()
    {
        

    }

    public void InitiateMesh()
    {
        if (hasInitiated) return;

        hasInitiated = true;

        originalMesh = GetComponent<MeshFilter>().sharedMesh;
        meshClone = Instantiate(originalMesh);
        GetComponent<MeshFilter>().sharedMesh = meshClone;
        renderer = GetComponent<MeshRenderer>();

        //初始化所有顶点
        jvs = new JellyVertex[meshClone.vertices.Length];
        for (int i = 0; i < meshClone.vertices.Length; i++)
        {
            //存储顶点世界坐标
            jvs[i] = new JellyVertex(i, transform.TransformPoint(meshClone.vertices[i]));
        }
    }

    private void FixedUpdate()
    {
        if (!hasInitiated) return;

        vertexArray = originalMesh.vertices;
        for(int i = 0; i<jvs.Length; i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jvs[i].ID]);
            float intensity = (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * elasticity;
            jvs[i].Shake(target, mass, stiffness, damping);
            target = transform.InverseTransformPoint(jvs[i].position);
            vertexArray[jvs[i].ID] = Vector3.Lerp(vertexArray[jvs[i].ID], target, intensity);

        }
        meshClone.vertices = vertexArray;
    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 position;
        public Vector3 velocity, force;

        public JellyVertex(int _id, Vector3 _pos)
        {
            ID = _id;
            position = _pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)
        {
            force = (target - position) * s;
            velocity = (velocity + force / m) * d;
            position += velocity;
            if((velocity + force + force/m).magnitude < 0.001f)
            {
                position = target;
            }
        }

    }


}
