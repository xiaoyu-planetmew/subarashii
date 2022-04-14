using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MovePoint))]
public class MovePointDisplay : MonoBehaviour
{
    public ArrowDisplayType display;

    private GameObject arrowPrefab;

    private void Start()
    {
        InitiateDisplay();
    }

    private void InitiateDisplay()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        if (GetComponent<MovePointInputController>() == null) return;

        arrowPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"), transform);

        arrowPrefab.transform.localPosition = Vector3.zero;

        arrowPrefab.GetComponent<ArrowDisplay>().InitiateArrow(display);
    }

    
}




