using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputChecker : MonoBehaviour
{
    static public KeyboardInputChecker Instance;

    private bool[] inputSuccess;
    private bool hasInit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetChecker();
    }



    public bool CheckInputSuccess(KeyDirectionType dir)
    {
        if (!hasInit)
            InitiateChecker(dir);

        return CheckingInput(dir);
    }

    private void InitiateChecker(KeyDirectionType dir)
    {
        hasInit = true;

        if(dir == KeyDirectionType.Up || dir == KeyDirectionType.Left ||
            dir == KeyDirectionType.Down || dir == KeyDirectionType.Right)
        {
            inputSuccess = new bool[] { false };
        }
        else
        {
            inputSuccess = new bool[] { false, false };
        }

    }

    private bool CheckingInput(KeyDirectionType dir)
    {
        bool allIputSucess = true;

        switch (dir)
        {
            case KeyDirectionType.Up:
                {
                    if(Input.GetKey(KeyCode.UpArrow))
                        inputSuccess[0] = true;
                    break;
                }
            case KeyDirectionType.Right:
                {
                    if (Input.GetKey(KeyCode.RightArrow))
                        inputSuccess[0] = true;
                    break;
                }
            case KeyDirectionType.Down:
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                        inputSuccess[0] = true;
                    break;
                }
            case KeyDirectionType.Left:
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                        inputSuccess[0] = true;
                    break;
                }
            case KeyDirectionType.UpLeft:
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                        inputSuccess[0] = true;
                    if(Input.GetKey(KeyCode.LeftArrow))
                        inputSuccess[1] = true;
                    break;
                }
            case KeyDirectionType.DownLeft:
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                        inputSuccess[0] = true;
                    if (Input.GetKey(KeyCode.LeftArrow))
                        inputSuccess[1] = true;
                    break;
                }
            case KeyDirectionType.UpRight:
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                        inputSuccess[0] = true;
                    if (Input.GetKey(KeyCode.RightArrow))
                        inputSuccess[1] = true;
                    break;
                }
            case KeyDirectionType.DownRight:
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                        inputSuccess[0] = true;
                    if (Input.GetKey(KeyCode.RightArrow))
                        inputSuccess[1] = true;
                    break;
                }
        }

        foreach(bool flag in inputSuccess)
        {
            allIputSucess = flag && allIputSucess;
        }

        return allIputSucess;
    }

    public void ResetChecker()
    {
        hasInit = false;
    }

}
