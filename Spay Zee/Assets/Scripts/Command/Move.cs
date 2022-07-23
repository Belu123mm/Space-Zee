using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //Up _up;
    //Down _down;
    //Left _left;
    //Right _right;

    Stack<ICommand> lastCommands = new Stack<ICommand>();

    private void Awake()
    {
        //_up = new Up(transform);
        //_down = new Down(transform);
        //_left = new Left(transform);
        //_right = new Right(transform);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            //_up.DoAction();
            //lastCommands.Push(_up);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            //_down.DoAction();
            //lastCommands.Push(_down);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //_left.DoAction();
            //lastCommands.Push(_left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //_right.DoAction();
            //lastCommands.Push(_right);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //lastCommands.Pop().UndoAction();
        }
    }
}
