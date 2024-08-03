using UnityEngine;

namespace Assets.Script.Enemy.Troll
{
    public class TrollController : EnemyController
    {

        public float waitTime;
        public float LastWaitTime;
        protected override void Start()
        {
            base.Start();
            IsMode1 = true;
            waitTime = 1f;
        }
        protected override void Update()
        {
            base.Update();
            LastWaitTime -= Time.deltaTime;
        }
        #region STATE METHODS
        public override void ChaseStateAction()
        {
            if (LastWaitTime > 0)
            {
                FSM.ChangeState(IdleState);
                return;
            }
            else if (CanAttack1())
            {
                IsStunAttack = false;
                FSM.ChangeState(Attack1State);
                return;
            }
            else if (CanAttack3())
            {
                IsStunAttack = false;
                FSM.ChangeState(Attack3State);
                return;
            }
            else if (PlayerDistance() > Data.ChaseDistance)
            {
                MoveToPlayer();
                return;
            }
        }
        #endregion
    }
}
