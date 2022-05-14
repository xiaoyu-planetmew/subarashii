using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    public TipType Tip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            switch(Tip)
            {
                case TipType.Tip_Triple:
                SoundController.Instance.Tip_Triple.HandleEvent(gameObject);
                break;
                case TipType.Tip_Double:
                SoundController.Instance.Tip_Double.HandleEvent(gameObject);
                break;
                case TipType.Tip_TripleChange:
                SoundController.Instance.Tip_TripleChange.HandleEvent(gameObject);
                break;
            }
        }
    }
    public enum TipType
    {
        Tip_Triple,
        Tip_Double,
        Tip_TripleChange,
    }
}
