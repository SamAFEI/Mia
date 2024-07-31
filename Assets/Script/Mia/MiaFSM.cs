using Assets.Script.Mia;
using UnityEngine;

public class MiaFSM
{
    public MiaState CurrentState { get; private set; }

    public void InitState(MiaState _startState)
    {
        CurrentState = _startState;
        CurrentState.OnEnter();
    }

    public void ChangeState(MiaState _newState)
    {
        CurrentState.OnExit();
        CurrentState = _newState;
        CurrentState.OnEnter();
    }

}
