using UnityEngine;

namespace Assets.Script.Enemy
{
    public class EnemyStateAttack2 : EnemyState
    {
        public EnemyStateAttack2(EnemyController _enemy, EnemyFSM _FSM, string _animName) : base(_enemy, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Enemy.RB.velocity = Vector3.zero;
            Enemy.CheckIsFacingRight(Enemy.PlayerDistanceX() > 0);
            Enemy.AttackDamage = Enemy.Data.Attack2Damage;
            Enemy.IsHeavyAttack = Enemy.Data.IsHeaveAttack2;
            Enemy.IsAttacking = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            Enemy.LastAttack2Time = Enemy.Data.Attack2RefillTime;
            Enemy.CloseDamageTrigger();
            Enemy.CloseMoveTrigger();
            Enemy.IsAttacking = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isAnimFinish)
            {
                if (Enemy.IsMode3)
                {
                    FSM.ChangeState(Enemy.Attack1State);
                    return;
                }
                FSM.ChangeState(Enemy.IdleState);
                return;
            }
        }
    }
}
