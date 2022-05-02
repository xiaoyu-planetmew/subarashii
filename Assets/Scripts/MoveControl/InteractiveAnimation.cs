using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveAnimation : MonoBehaviour
{
    [Header("是否自动播放")]
    public bool autoPlay = false;

    [Header("按键触发")]
    public string successAnimation;
    public string failureAnimation;

    [HideInInspector] public bool success;
    private bool startPlayAnimation;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if(anim == null)
            anim = transform.parent.GetComponent<Animator>();

        startPlayAnimation = false;
    }

    private void Update()
    {
        if(startPlayAnimation)
        {
            anim.SetTrigger(success ? successAnimation : failureAnimation);

            startPlayAnimation = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(autoPlay)
            {
                anim.SetTrigger(successAnimation);
            }
            else
            {
                startPlayAnimation = true;
            }
        }
    }

 
}
