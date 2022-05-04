using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MovePoint))]
public class MovePointDisplay : MonoBehaviour
{
    public bool specialHint = false;

    [HideInInspector] public ArrowDisplay arrowPrefab;
    [HideInInspector] public bool active;
    private MovePoint mp;

    private void Start()
    {
        mp = GetComponent<MovePoint>();

        //InitiateDisplay();
    }

    public void InitiateDisplay()
    {

        // 关闭贝塞尔控制点位置提示
        foreach(Transform besizer in mp.besizerControlPoints)
        {
            besizer.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (GetComponent<MovePointInputController>() == null) return;

        arrowPrefab = GetComponentInChildren<ArrowDisplay>();

        arrowPrefab.transform.localPosition = Vector3.zero;

        arrowPrefab.InitiateArrow(GetComponent<MovePointInputController>().keyInput.keyInput);
    }

    public void ResetMovePointDisplay()
    {
        arrowPrefab.ResetArrow();
    }
}




