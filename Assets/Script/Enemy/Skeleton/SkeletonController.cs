using Unity.IO.LowLevel.Unsafe;

namespace Assets.Script.Enemy.Skeleton
{
    public class SkeletonController : EnemyController
    {
        public SkeletonStateShield ShieldState { get; private set; }
        protected override void SetFSMState()
        {
            base.SetFSMState();
            //ShieldState = new SkeletonStateShield(this, FSM, "Shield");
        }
        #region STATE METHODS
        public override void ChaseStateAction()
        {
            //base.ChaseStateAction();
            if (CanAttack3())
            {
                FSM.ChangeState(Attack3State);
                return;
            }
            else if (CanAttack1())
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
