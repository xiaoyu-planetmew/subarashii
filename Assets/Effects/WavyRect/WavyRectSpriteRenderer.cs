using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeController))]
public class WavyRectSpriteRenderer : MonoBehaviour
{
    private SpriteShapeController shapeController;
    private bool hasInit;

    private void Start()
    {
        shapeController = GetComponent<SpriteShapeController>();
    }


    public void RenderShape(Vector3[] points)
    {
        if(!hasInit)
        {
            //InitialateSpriteShape(points.Length);
        }

        for(int i = 0; i<points.Length; i++)
        {
            shapeController.spline.SetPosition(i, points[i]);
        }

    }

    private void InitialateSpriteShape(int num)
    {
        int round = num / 4;
        for(int i = 0; i<round; i++)
        {
            int splinePointNum = shapeController.spline.GetPointCount();
            Debug.Log("point num " + splinePointNum);
            for(int p = 0; p < splinePointNum; p++)
            {
                try
                {
                    shapeController.spline.InsertPointAt(p, GetAvePoint(shapeController.spline.GetPosition(p), shapeController.spline.GetPosition((p + 1) % splinePointNum)));
                }
                catch
                {

                }
                Debug.Log("Int " + p);
            }
        }
        hasInit = true;
    }

    private Vector3 GetAvePoint(Vector3 a, Vector3 b)
    {
        Vector3 newPoint = new Vector3((a.x + b.x) / 2, (a.y + b.y) / 2, (a.z + b.z) / 2);
        Debug.Log("a " + a);
        Debug.Log("b " + b);
        Debug.Log(newPoint);
        return newPoint;
    }
}
