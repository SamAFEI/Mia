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
            Enemy.IsHeavyAttack = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            Enemy.LastAttack3Time = Enemy.Data.Attack3RefillTime;
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
