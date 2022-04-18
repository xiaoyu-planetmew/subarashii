using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MovePoint))]
public class MovePointDisplay : MonoBehaviour
{
    private GameObject arrowPrefab;
    private MovePoint mp;

    private void Start()
    {
        mp = GetComponent<MovePoint>();

        //InitiateDisplay();
    }

    public void InitiateDisplay()
    {
        // �ر�MovePointλ����ʾ
        GetComponent<SpriteRenderer>().enabled = false;

        // �رձ��������Ƶ�λ����ʾ
        foreach(Transform besizer in mp.besizerControlPoints)
        {
            besizer.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (GetComponent<MovePointInputController>() == null) return;

        arrowPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"), transform);

        arrowPrefab.transform.localPosition = Vector3.zero;

        arrowPrefab.GetComponent<ArrowDisplay>().InitiateArrow(GetComponent<MovePointInputController>().keyInput.keyInput);
    }

    public void ResetMovePointDisplay()
    {
        arrowPrefab.GetComponent<ArrowDisplay>().ResetArrow();
    }
}




