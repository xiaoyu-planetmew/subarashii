using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveAnimation : MonoBehaviour
{
    [Header("�Ƿ��Զ�����")]
    public bool autoPlay = false;

    [Header("��������")]
    public string successAnimation;
    public string failureAnimation;

    [HideInInspector] public bool success;
    [HideInInspector] public bool active;
    [HideInInspector] public MovePointInputController linkMovePoint;
    private bool startPlayAnimation;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if(anim == null)
            anim = transform.parent.GetComponent<Animator>();

        startPlayAnimation = false;
        active = false;
    }

    private void Update()
    {
        if(startPlayAnimation && active)
        {
            anim.SetTrigger(success ? successAnimation : failureAnimation);
            PlayerAnimation();
            Debug.Log("success = "+success);
            startPlayAnimation = false;
            active = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(autoPlay)
            {
                anim.SetTrigger(successAnimation);
                PlayerAnimation();
            }
            else
            {
                startPlayAnimation = true;
            }
        }
    }

 
    public void PlayerAnimation()
    {
        if (linkMovePoint == null) return;

        linkMovePoint.PlayerSpecialAnimation();
    }
}
