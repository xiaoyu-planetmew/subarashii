using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovePoint))]
public class MovePointInputController : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("�˽ڵ���������")]
    public MovePointInput[] keyInputs;

    [Header("������ק")]
    public bool powerfulEffect = false;

    private bool waitingForInputs;
    private float inputTimer;
    private int nowCheckingInputNum;

    private void Start()
    {
        ResetMovePointInput();
    }

    private void Update()
    {
        //�ȴ��������
        if (waitingForInputs)
        {
            if (keyInputs.Length == 0) return;

            CheckingInput();
        }
    }

    public void StartCheckingInput()
    {
        if (PlayerController.Instance.startPlaying)
        {
            //��ʼ�ȴ��������
            waitingForInputs = true;
            inputTimer = 0;
            if (keyInputs.Length > 0)
                keyInputs[nowCheckingInputNum].nowInputStatus = MovePointInput.NowStatus.WaitingForInput;

            //�ڵ����뿪����Ч

        }
        else
        {
            // ��ɫ������Ľڵ���Ч
        }
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

                        // С�ɹ���Ч

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
        ResetMovePointInput();

        // ʧ�ܿ۷�
        PlayerController.Instance.AddBlood();

        // ʧ����Ч

    }

    public void PointInputSuccess()
    {
        KeyboardInputChecker.Instance.ResetChecker();
        ResetMovePointInput();


        // ��ȫ�ɹ���Ч
        // �����һ���������ק��Ч
        PlayerEffectController.Instance.DragCircleEffect(keyInputs[keyInputs.Length-1].keyInput, powerfulEffect);

    }

    public void ResetMovePointInput()
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
    Stop,
}