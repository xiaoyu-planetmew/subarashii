using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MovePoint))]
public class MovePointDisplay : MonoBehaviour
{
    public GameObject Up;
    public GameObject UpLeft;
    public GameObject Left;
    public GameObject DownLeft;
    public GameObject Down;
    public GameObject DownRight;
    public GameObject Right;
    public GameObject UpRight;
    public GameObject Space;

    private MovePointInputController inputController;

    private void Start()
    {
        inputController = GetComponentInParent<MovePointInputController>();
    }


}
