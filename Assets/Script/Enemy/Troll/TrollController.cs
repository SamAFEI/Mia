using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Enemy.Troll
{
    public class TrollController : EnemyController
    {
        #region STATE METHODS
        public override void ChaseStateAction()
        {
            //base.ChaseStateAction();
            if (CanAttack3())
            {
                IsStunAttack = false;
                FSM.ChangeState(Attack3State);
                return;
            }
            else if (CanAttack2())
            {
                IsStunAttack = true;
                FSM.ChangeState(Attack2State);
                return;
            }
            else if (CanAttack1())
            {
                IsStunAttack = false;
                FSM.ChangeState(Attack1State);
                return;
            }
            else if (PlayerDistance() > Data.ChaseDistance)
            {
                MoveToPlayer();
                return;
            }
            else
            {
                FSM.ChangeState(IdleState);
                return;
            }
        }
        #endregion
    }
}
