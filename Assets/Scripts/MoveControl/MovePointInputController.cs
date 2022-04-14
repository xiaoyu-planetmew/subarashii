using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovePoint))]
public class MovePointInputController : MonoBehaviour
{

    [Header("Good����")]
    [Tooltip("Good")]
    public float goodCheckDistance = 0.5f;

    [Header("��������")]
    [Tooltip("�˽ڵ���������")]
    public MovePointInput[] keyInputs;

    [SerializeField] private bool waitingForInputs;
    private float inputTimer;
    private int nowCheckingInputNum;

    private void Start()
    {
        ResetThisPoint();
    }

    private void Update()
    {
        //�ȴ��������
        if (waitingForInputs)
        {
            if (keyInputs.Length == 0) return;

            CheckingInput();
        }

        if(Input.GetKeyDown(KeyCode.G) && gameObject.name == "Point (1)")
        {
            StartCheckingInput();
            PlayerMoveController.Instance.MoveToPoint(GetComponent<MovePoint>());
        }
    }

    public void StartCheckingInput()
    {
        //��ʼ�ȴ��������
        waitingForInputs = true;
        inputTimer = 0;
        if(keyInputs.Length>0)
            keyInputs[nowCheckingInputNum].nowInputStatus = MovePointInput.NowStatus.WaitingForInput;

        //�ڵ����뿪����Ч

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCheckingInput();
            Debug.Log("�ȴ����룺" + gameObject.name);
        }
    }

    /// <summary>
    /// �����ҵļ�������
    /// </summary>
    private void CheckingInput()
    {
        inputTimer += Time.deltaTime;

        for (int i = 0; i < keyInputs.Length; i++)
        {
            if(keyInputs[i].nowInputStatus == MovePointInput.NowStatus.WaitingForInput)
            {

                if (keyInputs[i].CheckInput(inputTimer) == MovePointInput.NowStatus.InputSuccess)
                {
                    //����ɹ�,������һ�����
                    inputTimer = 0;
                    if(nowCheckingInputNum<keyInputs.Length-1)
                    {
                        nowCheckingInputNum++;
                        keyInputs[nowCheckingInputNum].nowInputStatus = MovePointInput.NowStatus.WaitingForInput;
                    }
                    else
                    {
                        // ��ȫ�ɹ�
                        PointInputSuccess();

                        Debug.Log("����ɹ� " + gameObject.name);
                        break;
                    }
                }
                else if (keyInputs[i].CheckInput(inputTimer) == MovePointInput.NowStatus.Fail)
                {
                    //�˴�����ʧ��
                    PointInputFail();
                    Debug.Log("����ʧ�� " + gameObject.name);
                    break;
                }
                else
                {
                    return;
                }
            }
        }
    }

    public void PointInputFail()
    {
        KeyboardInputChecker.Instance.ResetChecker();
        ResetThisPoint();

        // ʧ�ܿ۷�

        // ʧ����Ч
    }

    public void PointInputSuccess()
    {
        KeyboardInputChecker.Instance.ResetChecker();
        ResetThisPoint();


        // ����ɹ���Ч
        if(Vector3.Distance( PlayerMoveController.Instance.transform.position, transform.position) < goodCheckDistance)
        {
            //Good
            Debug.Log("Good!");
        }
        else
        {
            //Normal
            Debug.Log("Normal");
        }
    }

    public void ResetThisPoint()
    {
        waitingForInputs = false;
        inputTimer = 0;
        nowCheckingInputNum = 0;
    }

}

#region �ڵ�������
[System.Serializable]
public class MovePointInput
{
    public KeyDirectionType keyInput ;
    public float timeForOneInput; // ��������ʱ��
    public NowStatus nowInputStatus = NowStatus.Freeze; 

    private bool[] inputKey;

    public MovePointInput()
    {
        keyInput = KeyDirectionType.Right;
        timeForOneInput = 0.5f;
        nowInputStatus = NowStatus.Freeze;
    }

    public NowStatus CheckInput(float nowTime)
    {
        NowStatus inputSuccess = NowStatus.WaitingForInput;

        if (KeyboardInputChecker.Instance.CheckInputSuccess(keyInput))
        {
            nowInputStatus = NowStatus.Freeze;
            return NowStatus.InputSuccess;
        }

        if (nowTime >= timeForOneInput)
        {
            nowInputStatus = NowStatus.Freeze;
            return NowStatus.Fail;
        }

        return inputSuccess;
    }

    public enum NowStatus
    {
        Freeze,
        WaitingForInput,
        InputSuccess,
        Fail,
    }

}
#endregion

public enum KeyDirectionType
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