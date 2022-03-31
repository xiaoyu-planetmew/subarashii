using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WavyCirclePointsController))]
public class WaveJellyEffect : MonoBehaviour
{
    public float elasticity = 1f;
    public float mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;

    private Vector3[] originLinePos;
    private bool hasInitiated;
    private JellyVertex[] jvs;


    public void CalculateJelly(Vector3[] linePoints)
    {
        if(!hasInitiated)
        {
            hasInitiated = true;

            originLinePos = new Vector3[linePoints.Length];

            //初始化所有顶点
            jvs = new JellyVertex[linePoints.Length];
            for (int i = 0; i < linePoints.Length; i++)
            {
                //存储顶点世界坐标
                jvs[i] = new JellyVertex(i, transform.TransformPoint(linePoints[i]));

                //储存原坐标
                originLinePos[i] = linePoints[i];
            }
        }

        for (int i = 0; i < jvs.Length; i++)
        {
            Vector3 target = transform.TransformPoint(originLinePos[i]);
            Vector2 boundY = GetYPointsBound(linePoints);
            float intensity = (1 - (boundY.y - target.y) / (boundY.y - boundY.x)) * elasticity;
            jvs[i].Shake(target, mass, stiffness, damping);
            target = transform.InverseTransformPoint(jvs[i].position);
            linePoints[i] = Vector3.Lerp(linePoints[i], target, intensity);

        }

    }

    private Vector2 GetYPointsBound(Vector3[] points)
    {
        float maxY = 0f;
        float minY = 0f;

        foreach(Vector3 point in points)
        {
            maxY = Mathf.Max(maxY, point.y);
            minY = Mathf.Min(minY, point.y);
        }

        return new Vector2(minY, maxY);
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

            if ((velocity + force + force / m).magnitude < 0.001f)
            {
                position = target;
            }
        }

    }
}
