namespace Assets.Script.Enemy
{
    public class EnemyStateHurt : EnemyState
    {
        public EnemyStateHurt(EnemyController _enemy, EnemyFSM _FSM, string _animName) : base(_enemy, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Enemy.SetIsBattling(true);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!Enemy.IsHurting)
            {
                FSM.ChangeState(Enemy.IdleState);
                return;
            }
        }
    }
}
