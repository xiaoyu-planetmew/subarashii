using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovePoint))]
public class MovePointInputController : MonoBehaviour
{
    [Header("输入命令")]
    [Tooltip("此节点输入命令")]
    public MovePointInput keyInput;

    [Header("大力拖拽")]
    public bool powerfulEffect = false;

    [Header("交互角色")]
    public InteractiveAnimation interactiveAnimation;
    public  PlayerSpecialAnimationType  PlayerSpecialAnimation = PlayerSpecialAnimationType.Null;

    private bool waitingForInputs;
    private float inputTimer;

    private void Start()
    {
        ResetMovePointInput();
        if (interactiveAnimation != null)
            interactiveAnimation.linkMovePoint = this;
    }

    private void Update()
    {
        //等待玩家输入
        if (waitingForInputs)
        {
            CheckingInput();
        }
    }

    public void StartCheckingInput()
    {
        if (PlayerController.Instance.startPlaying)
        {
            //开始等待玩家输入
            waitingForInputs = true;
            inputTimer = 0;
            keyInput.nowInputStatus = MovePointInput.NowStatus.WaitingForInput;

            //节点输入开启特效

        }
        else
        {
            // 角色死亡后的节点特效
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {

            StartCheckingInput();
            Debug.Log("等待输入：" + gameObject.name);
        }
    }

    /// <summary>
    /// 检查玩家的键盘输入
    /// </summary>
    private void CheckingInput()
    {
        inputTimer += Time.deltaTime;

        if (keyInput.keyInput == KeyDirectionType.Null) return;

        if(keyInput.nowInputStatus == MovePointInput.NowStatus.WaitingForInput)
        {

            if (keyInput.CheckInput(inputTimer) == MovePointInput.NowStatus.InputSuccess)
            {
                //输入成功,开启下一个检查
                inputTimer = 0;

                // 完全成功
                PointInputSuccess();

                Debug.Log("输入成功 " + gameObject.name);
                
            }
            else if (keyInput.CheckInput(inputTimer) == MovePointInput.NowStatus.Fail)
            {
                //此次输入失败
                PointInputFail();
                Debug.Log("输入失败 " + gameObject.name + " 应输入 " + keyInput.keyInput);
            }
            else
            {
                return;
            }
        }
        
    }

    /// <summary>
    /// 失败
    /// </summary>
    public void PointInputFail()
    {
        KeyboardInputChecker.Instance.ResetChecker();
        ResetMovePointInput();

        //Combo
        PlayerController.Instance.comboNum = 0;

        // 失败扣分
        PlayerController.Instance.AddBlood();

        // 失败特效

        // 主角动画
        CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Miss);

        // 箭头
        GetComponent<MovePointDisplay>().arrowPrefab.FailureAnimation();

        // 声音
        if (keyInput.keyInput == KeyDirectionType.Space)
        {
            SoundController.Instance.Input_Space_Miss.HandleEvent(gameObject);

        }
        else if (keyInput.keyInput != KeyDirectionType.Null)
        {
            SoundController.Instance.Input_Arrow_Miss.HandleEvent(gameObject);
        }

        PlayerController.Instance.miss();
    }

    /// <summary>
    /// 成功
    /// </summary>
    public void PointInputSuccess()
    {
        KeyboardInputChecker.Instance.ResetChecker();
        ResetMovePointInput();

        //Combo
        PlayerController.Instance.comboNum++;

        // 成功特效
        // 按最后一个方向出拖拽特效
        PlayerEffectController.Instance.DragCircleEffect(keyInput.keyInput, powerfulEffect);

        // 主角动画
        CharacterAnimationController.Instance.ChangeAnimationEvent(AnimationEventType.Good);

        // 其它交互角色动画
        if (keyInput.keyInput == KeyDirectionType.Space)
        {
            if (interactiveAnimation != null)
                interactiveAnimation.success = true;
        }

        // 箭头
        GetComponent<MovePointDisplay>().arrowPrefab.SuccessAnimation();


        //加速
        PlayerMoveController.Instance.AccerateMove();

        //声音
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

        PlayerController.Instance.success();
        
    }

    public void ResetMovePointInput()
    {
        waitingForInputs = false;
        inputTimer = 0;
    }

}

#region 节点输入类
[System.Serializable]
public class MovePointInput
{
    public float timeForOneInput; // 单个输入时长
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
    FoodAbsorb_Left,
    FoodAbsorb_Right,
}
