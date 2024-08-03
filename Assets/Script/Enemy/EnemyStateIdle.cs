using Assets.Script.Manager;
using Unity.VisualScripting;
using UnityEngine;
namespace Assets.Script.Enemy
{
    public class EnemyStateIdle : EnemyState
    {
        public EnemyStateIdle(EnemyController _enemy, EnemyFSM _FSM, string _animName) : base(_enemy, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateTime = Enemy.Data.IdleTime;
            Enemy.RB.velocity = Vector2.zero; 
            Enemy.RunSpeed = Enemy.Data.runSpeed;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            if (GameManager.Instance.IsDie) return;
            base.OnUpdate();
            if (stateTime < 0f && !Enemy.IsStunning)
            {
                if (Enemy.IsMode1 || Enemy.IsMode2)
                {
                    FSM.ChangeState(Enemy.ChaseState);
                    return;
                }
                if (Enemy.IsBattling)
                {
                    if (Enemy.PlayerDistance() > Enemy.Data.ChaseDistance ||
                        Enemy.CanAttack3() || Enemy.CanAttack1())
                    {
                        FSM.ChangeState(Enemy.ChaseState);
                        return;
                    }
                }
                else
                {
                    FSM.ChangeState(Enemy.PatrolState);
                }
            }
        }
    }
}
