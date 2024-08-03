using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Enemy
{
    public class EnemyStateChase : EnemyState
    {
        public EnemyStateChase(EnemyController _enemy, EnemyFSM _FSM, string _animName) : base(_enemy, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Enemy.RunSpeed = Enemy.Data.runSpeed;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Enemy.ChaseStateAction();
            /*if (Enemy.CanAttack3())
            {
                FSM.ChangeState(Enemy.Attack3State);
                return;
            }
            else if (Enemy.CanAttack1())
            {
                FSM.ChangeState(Enemy.Attack1State);
                return;
            }
            else if (Enemy.PlayerDistance() > Enemy.Data.ChaseDistance)
            {
                Enemy.MoveToPlayer();
                return;
            }
            else
            { 
                FSM.ChangeState(Enemy.IdleState);
                return;
            }*/
        }
    }
}
