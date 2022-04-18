using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDisplay : MonoBehaviour
{
    public ArrowPrefab[] arrows;

    private KeyDirectionType thisArrow;

    private void Awake()
    {
        ResetArrow();
    }

    public void ResetArrow()
    {
        foreach(ArrowPrefab prefab in arrows)
        {
            prefab.arrow.SetActive(false);
            
            // 恢复成初始的显示

        }
    }

    public void InitiateArrow(KeyDirectionType type)
    {
        thisArrow = type;

        foreach (ArrowPrefab prefab in arrows)
        {
            if(type == prefab.type)
            {
                // 初始化对应箭头显示
                prefab.arrow.SetActive(true);
                //ResetArrow()；
            }
        }
    }
}

[System.Serializable]
public class ArrowPrefab
{
    public KeyDirectionType type;
    public GameObject arrow;

}
