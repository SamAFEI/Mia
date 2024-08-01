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
            Enemy.IsHeavyAttack = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            Enemy.LastAttack1Time = Enemy.Data.Attack1RefillTime;
            Enemy.CloseDamageTrigger();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isAnimFinish)
            {
                FSM.ChangeState(Enemy.IdleState);
                return;
            }
        }
    }
}
