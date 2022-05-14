using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(MovePoint))]
public class MovePointInputController : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("�˽ڵ���������")]
    public MovePointInput keyInput;

    [Header("������ק")]
    public bool powerfulEffect = false;

    [Header("������ɫ")]
    public InteractiveAnimation interactiveAnimation1;
    public InteractiveAnimation interactiveAnimation2;
    public PlayerSpecialAnimationType  SpecialAnimation = PlayerSpecialAnimationType.Null;
    private MovePoint _mp;

    [Header("��ֹ����")]
    public bool dontAccerate = false;

    [Header("特殊激活")]
    public bool specialActiveNextMovePoints = false;
    public int specialActiveNum = 0;

    private bool waitingForInputs;
    private float inputTimer;

    private void Start()
    {
        ResetMovePointInput();
        if (interactiveAnimation1 != null)
            interactiveAnimation1.linkMovePoint = this;
        if (interactiveAnimation2 != null)
            interactiveAnimation2.linkMovePoint = this;
        _mp = GetComponent<MovePoint>();
    }

    private void FixedUpdate()
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
        if(collision.tag == "Player" && _mp.active)
        {
            if(_mp.nextPoint!=null)
                _mp.nextPoint.active = true;

            // 特殊激活
            if(specialActiveNextMovePoints)
            {
                MovePoint nextMP = _mp.nextPoint;

                for(int i= 0;i<specialActiveNum;i++)
                {
                    if(nextMP.nextPoint != null)
                    {
                        nextMP.nextPoint.active = true;

                        nextMP = nextMP.nextPoint;
                    }
                }

            }



            StartCheckingInput();
            Debug.Log("�ȴ����룺" + gameObject.name);
        }
    }

    /// <summary>
    /// �����ҵļ�������
    /// </summary>
    private void CheckingInput()
    {
        inputTimer += Time.fixedDeltaTime;

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
        //PlayerController.Instance.AddBlood();

        // ʧ����Ч

        // ���Ƕ���
        CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Miss);

        // ��ͷ
        GetComponent<MovePointDisplay>().arrowPrefab.FailureAnimation();

        // ����
        if (keyInput.keyInput == KeyDirectionType.Space)
        {
            SoundController.Instance.Input_Space_Miss.HandleEvent(gameObject);

        }
        else if (keyInput.keyInput != KeyDirectionType.Null)
        {
            SoundController.Instance.Input_Arrow_Miss.HandleEvent(gameObject);
        }
        
        PlayerController.Instance.Miss();
    }

    /// <summary>
    /// �ɹ�
    /// </summary>
    public void PointInputSuccess()
    {
        KeyboardInputChecker.Instance.ResetChecker();
        ResetMovePointInput();

        //Combo
        //PlayerController.Instance.comboNum++;

        // �ɹ���Ч
        // �����һ���������ק��Ч
        // Effect
        PlayerEffectController.Instance.DragCircleEffect(keyInput.keyInput, powerfulEffect);

        // ���Ƕ���
        // Character Animation
        if(!interactiveAnimation1 && !interactiveAnimation2 && SpecialAnimation == PlayerSpecialAnimationType.Null)//三个同时为空?
            CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Good);

        // ����������ɫ����
        if (keyInput.keyInput == KeyDirectionType.Space)
        {
            if (interactiveAnimation1 != null)
            {
                interactiveAnimation1.success = true;
                interactiveAnimation1.active = true;
                //Debug.Log("interactiveAnimation1 is success");
            }
            if (interactiveAnimation2 != null)
            {
                interactiveAnimation2.success = true;
                interactiveAnimation2.active = true;
            }
            if(SpecialAnimation != PlayerSpecialAnimationType.Null)
            {
                PlayerSpecialAnimation();
            }
        }
        

        // ��ͷ
        // Arrow
        GetComponent<MovePointDisplay>().arrowPrefab.SuccessAnimation();


        //����
        if(!dontAccerate)
            PlayerMoveController.Instance.AccerateMove();

        //����
        if (keyInput.keyInput == KeyDirectionType.Space)
        {
            if (GetComponent<MovePointDisplay>().specialHint)
            {
                SoundController.Instance.Input_Arrow_Success_Color.HandleEvent(gameObject);//彩色空格音效
            }
            else
            {
                SoundController.Instance.Input_Space_Success_White.HandleEvent(gameObject);//普通空格音效
            }
            //SoundController.Instance.Input_Space_Success.HandleEvent(gameObject);
        }
        else if (keyInput.keyInput != KeyDirectionType.Null)
        {
            if(PlayerController.Instance.comboNum>=3)//combo加值已注释，这段不会触发
            {
                SoundController.Instance.Input_Arrow_Combo.HandleEvent(gameObject);//不触发
            }
            else
            {
                if (GetComponent<MovePointDisplay>().specialHint)
                {
                    SoundController.Instance.Input_Arrow_Success_Color.HandleEvent(gameObject);//彩色空格音效
                }
                else
                {
                    SoundController.Instance.Input_Arrow_Success_White.HandleEvent(gameObject);//普通空格音效
                }
                //SoundController.Instance.Input_Arrow_Success.HandleEvent(gameObject);
            }
        }

        PlayerController.Instance.Success(keyInput.keyInput == KeyDirectionType.Space);
        
    }

    public void PlayerSpecialAnimation()
    {

        switch (SpecialAnimation)
        {
            case PlayerSpecialAnimationType.SpaceAbsorb_Left:
                CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Absorb_Left);
                break;
            case PlayerSpecialAnimationType.SpaceAbsorb_Right:
                CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Absorb_Right);
                break;
            case PlayerSpecialAnimationType.SpaceCharge1:
                CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Charge1);
                break;
            case PlayerSpecialAnimationType.SpaceSmash_Left:
                CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Smash_Left);
                break;
            case PlayerSpecialAnimationType.SpaceSmash_Right:
                CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Smash_Right);
                break;
            case PlayerSpecialAnimationType.SpaceGuard:
                CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Guard);
                break;
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

public enum PlayerSpecialAnimationType
{ 
    Null,
    SpaceAbsorb_Left,
    SpaceAbsorb_Right,
    SpaceCharge1,
    SpaceGuard,
    SpaceSmash_Left,
    SpaceSmash_Right,
}
