using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    public AkEvent HoverSound;
    public AkEvent ClickSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HoverSound != null)
            HoverSound.HandleEvent(WwiseManager.Instance.gameObject);
    }

    public void PlayClickSound()
    {
        ClickSound.HandleEvent(WwiseManager.Instance.gameObject);
    }
    
    
}
