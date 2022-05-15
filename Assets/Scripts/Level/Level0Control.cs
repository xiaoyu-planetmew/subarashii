using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Level0Control : MonoBehaviour
{
    public GameObject Capsule;
    bool openFinished = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && !openFinished)
        {
            DialogSys.Instance.dialogNext();
            CapsuleAnim1();
        }
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
