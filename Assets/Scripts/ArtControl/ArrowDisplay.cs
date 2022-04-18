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
            
            // �ָ��ɳ�ʼ����ʾ

        }
    }

    public void InitiateArrow(KeyDirectionType type)
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
    public KeyDirectionType type;
    public GameObject arrow;

}
