using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDisplay : MonoBehaviour
{
    public ArrowPrefab[] arrows;

    private ArrowDisplayType thisArrow;

    private void Awake()
    {
        ResetArrow();
    }

    public void ResetArrow()
    {
        foreach(ArrowPrefab prefab in arrows)
        {
            prefab.arrow.SetActive(false);
            
            // �ָ��ɳ�ʼ����ʾ

        }
    }

    public void InitiateArrow(ArrowDisplayType type)
    {
        thisArrow = type;

        foreach (ArrowPrefab prefab in arrows)
        {
            if(type == prefab.type)
            {
                // ��ʼ����Ӧ��ͷ��ʾ
                prefab.arrow.SetActive(true);
                //ResetArrow()��
            }
        }
    }
}

[System.Serializable]
public class ArrowPrefab
{
    public ArrowDisplayType type;
    public GameObject arrow;

}


public enum ArrowDisplayType
{
    Up,
    UpLeft,
    Left,
    DownLeft,
    Down,
    DownRight,
    Right,
    UpRight,
    Space,
    Null,
}