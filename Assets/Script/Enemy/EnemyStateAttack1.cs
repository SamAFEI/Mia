using UnityEngine;

namespace Assets.Script.Enemy
{
    public class EnemyStateAttack1 : EnemyState
    {
        public EnemyStateAttack1(EnemyController _enemy, EnemyFSM _FSM, string _animName) : base(_enemy, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Enemy.RB.velocity = Vector3.zero;
            Enemy.CheckIsFacingRight(Enemy.PlayerDistanceX() > 0);
            Enemy.AttackDamage = Enemy.Data.Attack1Damage;
            Enemy.IsHeavyAttack = Enemy.Data.IsHeaveAttack1;
            Enemy.IsAttacking = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            Enemy.LastAttack1Time = Enemy.Data.Attack1RefillTime;
            Enemy.CloseDamageTrigger();
            Enemy.CloseMoveTrigger();
            Enemy.IsAttacking = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isAnimFinish)
            {
                if (Enemy.IsMode1)
                {
                    FSM.ChangeState(Enemy.Attack2State);
                    return;
                }
                if (Enemy.IsMode4)
                {
                    FSM.ChangeState(Enemy.Attack2State);
                    return;
                }
                FSM.ChangeState(Enemy.IdleState);
                return;
            }
        }
    }
}
