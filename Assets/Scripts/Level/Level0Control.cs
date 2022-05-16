using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Level0Control : MonoBehaviour
{
    public GameObject Capsule;
    bool openFinished = false;

    public int pressNum;
    // Start is called before the first frame update
    void Start()
    {
        pressNum = 0;
    }

    // Update is called once per frame
    void Update()
    {

/*        if(Input.anyKeyDown && !openFinished)
        {
            if(pressNum < 3)
            {
                DialogSys.Instance.dialogNext();
                CapsuleAnim1();
                pressNum++;
            }
            else
            {

            }
        }*/
    }
    public void level0Start()
    {
        DialogSys.Instance.dialogStart(0);

    }
    public void CapsuleAnim1()
    {
        Capsule.GetComponent<Animator>().SetTrigger("shake");
    }
    public void CapsuleAnim2()
    {
        openFinished = true;
        Capsule.GetComponent<Animator>().SetTrigger("open");
    }
    public void dialog2Start()
    {
        DialogSys.Instance.dialogStart(1);
    }
}
