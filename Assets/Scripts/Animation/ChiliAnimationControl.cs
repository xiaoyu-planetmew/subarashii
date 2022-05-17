using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiliAnimationControl : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void ChiliAnim_Debut()
    {
        anim.SetTrigger("debut");
    }

    void ChiliAnim_Attack()
    {
        anim.SetTrigger("attack");
    }

    void ChiliAnim_Defeat()
    {
        anim.SetTrigger("defeat");
    }

    void ChiliAnim_Hit()
    {
        anim.SetTrigger("hit");
    }

    void ChiliAnim_Proud()
    {
        anim.SetTrigger("proud");
    }
    void ChiliAnim_Idle()
    {
        anim.SetTrigger("idle");
    }

    //boss…˘“Ùøÿ÷∆≤ø∑÷
    void ChiliExploreSFX()
    {
        SoundController.Instance.Boss_Destroy.HandleEvent(gameObject);
    }

    void ChiliAttackSFX()
    {
        SoundController.Instance.Boss_Attack.HandleEvent(gameObject);
    }

    void ChiliSiren_Play()
    {
        SoundController.Instance.Boss_Siren_Play.HandleEvent(gameObject);
    }
    void ChiliSiren_Stop()
    {
        SoundController.Instance.Boss_Siren_Stop.HandleEvent(gameObject);
    }
}
