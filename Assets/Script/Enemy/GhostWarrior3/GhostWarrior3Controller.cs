using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Enemy.GhostWarrior3
{
    public class GhostWarrior3Controller : EnemyController
    {
        #region STATE METHODS
        public override void ChaseStateAction()
        {
            //base.ChaseStateAction();
            if (CanAttack3())
            {
                FSM.ChangeState(Attack3State);
                return;
            }
            else if(CanAttack2())
            {
                FSM.ChangeState(Attack2State);
                return;
            }
            else if(CanAttack1())
            {
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
