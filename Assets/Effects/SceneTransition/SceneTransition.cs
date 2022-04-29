using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    private Animator anim;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void EffectStart()
    {
        anim.SetBool("EffectStart", true);
    }

    public void EffectClose()
    {
        anim.SetBool("EffectStart", false);
    }

}
