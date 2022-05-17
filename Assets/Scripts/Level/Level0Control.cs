using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Level0Control : MonoBehaviour
{
    public GameObject Capsule;
    bool openFinished = false;

    public int pressNum;
    int hitCount = 0;
    [SerializeField] bool isShaking = true;
    // Start is called before the first frame update
    void Start()
    {
        pressNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.DownArrow))
        {
            if(hitCount == 3)
            {
                CapsuleAnim2();
                StopAllCoroutines();
                StartCoroutine(Open());
            }
            if(!isShaking && hitCount < 3) StartCoroutine(Shake());

        }
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
    public void OpenCapsule()
    {
        isShaking = false;
    }
    IEnumerator Shake()
    {
        isShaking = true;
        CapsuleAnim1();
        yield return new WaitForSeconds(0.5f);
        hitCount++;
        if(hitCount < 3)
        isShaking = false;
        
    }
    IEnumerator Open()
    {
        yield return new WaitForSeconds(2f);
        SceneController.Instance.ChangeScene("Level_1_ver4");
        Time.timeScale = 1;
    }
}
