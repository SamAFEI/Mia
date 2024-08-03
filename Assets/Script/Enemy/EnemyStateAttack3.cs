using UnityEngine;

namespace Assets.Script.Enemy
{
    public class EnemyStateAttack3 : EnemyState
    {
        public EnemyStateAttack3(EnemyController _enemy, EnemyFSM _FSM, string _animName) : base(_enemy, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Enemy.RB.velocity = Vector3.zero;
            Enemy.CheckIsFacingRight(Enemy.PlayerDistanceX() > 0);
            Enemy.AttackDamage = Enemy.Data.Attack3Damage;
            Enemy.IsHeavyAttack = Enemy.Data.IsHeaveAttack3;
            Enemy.IsAttacking = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            Enemy.LastAttack3Time = Enemy.Data.Attack3RefillTime;
            Enemy.CloseDamageTrigger();
            Enemy.CloseMoveTrigger();
            Enemy.IsAttacking = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isAnimFinish)
            {
                if (Enemy.IsMode2)
                {
                    Enemy.RunSpeed = 20f;
                    FSM.ChangeState(Enemy.Attack2State);
                    return;
                }
                if (Enemy.IsMode3)
                {
                    FSM.ChangeState(Enemy.Attack1State);
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
