
namespace Assets.Script.Enemy.Skeleton
{
    public class SkeletonStateShield : EnemyState
    {
        public SkeletonStateShield(EnemyController _enemy, EnemyFSM _FSM, string _animName) : base(_enemy, _FSM, _animName)
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
            Enemy.CheckIsFacingRight(Enemy.PlayerDistanceX() > 0);
            if (Enemy.LastAttack1Time < 0)
            {
                FSM.ChangeState(Enemy.ChaseState);
            }
        }
    }
}
