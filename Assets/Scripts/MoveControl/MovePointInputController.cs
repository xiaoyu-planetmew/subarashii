using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovePoint))]
public class MovePointInputController : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("�˽ڵ���������")]
    public MovePointInput keyInput;

    [Header("������ק")]
    public bool powerfulEffect = false;

    [Header("������ɫ")]
    public InteractiveAnimation interactiveAnimation;
    public bool animationTowardRight = false;

    private bool waitingForInputs;
    private float inputTimer;

    private void Start()
    {
        ResetMovePointInput();
    }

    private void Update()
    {
        //�ȴ��������
        if (waitingForInputs)
        {
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
            keyInput.nowInputStatus = MovePointInput.NowStatus.WaitingForInput;

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

        if (keyInput.keyInput == KeyDirectionType.Null) return;

        if(keyInput.nowInputStatus == MovePointInput.NowStatus.WaitingForInput)
        {

            if (keyInput.CheckInput(inputTimer) == MovePointInput.NowStatus.InputSuccess)
            {
                //����ɹ�,������һ�����
                inputTimer = 0;

                // ��ȫ�ɹ�
                PointInputSuccess();

                Debug.Log("����ɹ� " + gameObject.name);
                
            }
            else if (keyInput.CheckInput(inputTimer) == MovePointInput.NowStatus.Fail)
            {
                //�˴�����ʧ��
                PointInputFail();
                Debug.Log("����ʧ�� " + gameObject.name + " Ӧ���� " + keyInput.keyInput);
            }
            else
            {
                return;
            }
        }
        
    }

    /// <summary>
    /// ʧ��
    /// </summary>
    public void PointInputFail()
    {
        KeyboardInputChecker.Instance.ResetChecker();
        ResetMovePointInput();

        //Combo
        PlayerController.Instance.comboNum = 0;

        // ʧ�ܿ۷�
        PlayerController.Instance.AddBlood();

        // ʧ����Ч

        // ����
        if (keyInput.keyInput == KeyDirectionType.Space)
        {
            SoundController.Instance.Input_Space_Miss.HandleEvent(gameObject);

        }
        else if (keyInput.keyInput != KeyDirectionType.Null)
        {
            SoundController.Instance.Input_Arrow_Miss.HandleEvent(gameObject);
        }

    }

    /// <summary>
    /// �ɹ�
    /// </summary>
    public void PointInputSuccess()
    {
        KeyboardInputChecker.Instance.ResetChecker();
        ResetMovePointInput();

        //Combo
        PlayerController.Instance.comboNum++;

        // ��ȫ�ɹ���Ч
        // �����һ���������ק��Ч
        PlayerEffectController.Instance.DragCircleEffect(keyInput.keyInput, powerfulEffect);

        // ����
        if(keyInput.keyInput == KeyDirectionType.Space)
        {
            CharacterAnimationController.Instance.ChangeAnimationEvent(animationTowardRight ? AnimationEventType.Absorb_Right : AnimationEventType.Absorb_Left);

            // ����������ɫ����
            if (interactiveAnimation != null)
                interactiveAnimation.success = true;
        }
        else if(keyInput.keyInput != KeyDirectionType.Null)
        {
            CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Good);
        }

        //����
        PlayerMoveController.Instance.AccerateMove();

        //����
        if (keyInput.keyInput == KeyDirectionType.Space)
        {
            SoundController.Instance.Input_Space_Success.HandleEvent(gameObject);

        }
        else if (keyInput.keyInput != KeyDirectionType.Null)
        {
            if(PlayerController.Instance.comboNum>=3)
                SoundController.Instance.Input_Arrow_Combo.HandleEvent(gameObject);
            else
                SoundController.Instance.Input_Arrow_Success.HandleEvent(gameObject);
        }

        
    }

    public void ResetMovePointInput()
    {
        waitingForInputs = false;
        inputTimer = 0;
    }

}

#region �ڵ�������
[System.Serializable]
public class MovePointInput
{
    public float timeForOneInput; // ��������ʱ��
    public KeyDirectionType keyInput ;
    public NowStatus nowInputStatus = NowStatus.Freeze; 

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
    Null,
}
