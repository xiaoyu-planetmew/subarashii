using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public GameObject Effect;
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
        Effect.SetActive(false);
    }

    public void EffectStart()
    {
        Effect.SetActive(true);
        anim.SetBool("EffectStart", true);
    }

    public void EffectClose()
    {
        anim.SetBool("EffectStart", false);
    }

}
