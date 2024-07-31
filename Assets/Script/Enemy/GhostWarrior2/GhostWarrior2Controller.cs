using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Enemy.GhostWarrior2
{
    public class GhostWarrior2Controller : EnemyController
    {
        #region STATE METHODS
        public override void ChaseStateAction()
        {
            //base.ChaseStateAction();
            if (CanAttack1())
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
