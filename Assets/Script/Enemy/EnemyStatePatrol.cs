namespace Assets.Script.Enemy
{
    public class EnemyStatePatrol : EnemyState
    {
        public EnemyStatePatrol(EnemyController _enemy, EnemyFSM _FSM, string _animName) : base(_enemy, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Enemy.IsOnFrontWall)
            {
                Enemy.Turn();
                FSM.ChangeState(Enemy.IdleState);
                return;
            }
            Enemy.SetVelocity(Enemy.transform.localScale.x * Enemy.Data.runSpeed, Enemy.RB.velocity.y);
        }
    }
}
