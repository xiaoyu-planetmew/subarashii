using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovePoint))]
public class MovePointInputController : MonoBehaviour
{

    [Header("Good距离")]
    [Tooltip("Good")]
    public float goodCheckDistance = 0.5f;

    [Header("输入命令")]
    [Tooltip("此节点输入命令")]
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
        //等待玩家输入
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
        //开始等待玩家输入
        waitingForInputs = true;
        inputTimer = 0;
        if(keyInputs.Length>0)
            keyInputs[nowCheckingInputNum].nowInputStatus = MovePointInput.NowStatus.WaitingForInput;

        //节点输入开启特效

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

        for (int i = 0; i < keyInputs.Length; i++)
        {
            if(keyInputs[i].nowInputStatus == MovePointInput.NowStatus.WaitingForInput)
            {

                if (keyInputs[i].CheckInput(inputTimer) == MovePointInput.NowStatus.InputSuccess)
                {
                    //输入成功,开启下一个检查
                    inputTimer = 0;
                    if(nowCheckingInputNum<keyInputs.Length-1)
                    {
                        nowCheckingInputNum++;
                        keyInputs[nowCheckingInputNum].nowInputStatus = MovePointInput.NowStatus.WaitingForInput;
                    }
                    else
                    {
                        // 完全成功
                        PointInputSuccess();

                        Debug.Log("输入成功 " + gameObject.name);
                        break;
                    }
                }
                else if (keyInputs[i].CheckInput(inputTimer) == MovePointInput.NowStatus.Fail)
                {
                    //此次输入失败
                    PointInputFail();
                    Debug.Log("输入失败 " + gameObject.name);
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

        // 失败扣分

        // 失败特效
    }

    public void PointInputSuccess()
    {
        KeyboardInputChecker.Instance.ResetChecker();
        ResetThisPoint();


        // 输入成功特效
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

#region 节点输入类
[System.Serializable]
public class MovePointInput
{
    public KeyDirectionType keyInput ;
    public float timeForOneInput; // 单个输入时长
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