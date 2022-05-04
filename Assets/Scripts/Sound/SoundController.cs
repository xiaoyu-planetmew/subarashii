using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AkEvent Door_Impacting;
    public AkEvent Door_Open;
    public AkEvent Food_Aborb;
    public AkEvent Food_Happy;
    public AkEvent Space_Food;
    public AkEvent Food_Barriage;
    public AkEvent Space_Hinder;
    public AkEvent Boss_Attack;
    public AkEvent Boss_Destroy;
    public AkEvent Boss_Siren_Play;
    public AkEvent Boss_Siren_Stop;
    public AkEvent Final_Shit;
    public AkEvent Shit_Combine;
    public AkEvent Button_On;
    public AkEvent Button_Off;
    public AkEvent Button_Pause;
    public AkEvent Input_Arrow_Menu;
    public AkEvent Input_Arrow_Success;
    public AkEvent Input_Arrow_Miss;
    public AkEvent Input_Arrow_Combo;
    public AkEvent Input_Space_Success;
    public AkEvent Input_Space_Miss;
    public AkEvent Talk_Buzz;
    public AkEvent Talk_Radio_Play;
    public AkEvent Talk_Radio_Stop;
    public AkEvent Talking_1;
    public AkEvent Talking_2;
    public AkEvent Tip_Triple;
    public AkEvent Tip_Double;
    public AkEvent Tip_TripleChange;
    public AkEvent GameOver;

    public static SoundController Instance;
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
}

