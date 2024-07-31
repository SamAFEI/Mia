namespace Assets.Script.Enemy
{
    public class EnemyFSM
    {
        public EnemyState CurrentState { get; private set; }

        public void InitState(EnemyState _startState)
        {
            CurrentState = _startState;
            CurrentState.OnEnter();
        }

        public void ChangeState(EnemyState _newState)
        {
            CurrentState.OnExit();
            CurrentState = _newState;
            CurrentState.OnEnter();
        }
    }
}
