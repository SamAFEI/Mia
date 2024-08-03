using UnityEngine;

namespace Assets.Script.Enemy.LizardBoss
{
    public class LizardBossController : EnemyController
    {
        public float waitTime;
        public float LastWaitTime;

        protected override void Start()
        {
            base.Start();
            IsMode2 = true;
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
            else if (CanAttack3() && PlayerDistance() > 3)
            {
                LastWaitTime = waitTime;
                IsStunAttack = false;
                FSM.ChangeState(Attack3State);
                return;
            }
            else if (CanAttack1())
            {
                IsStunAttack = false;
                RunSpeed = 20f;
                FSM.ChangeState(Attack1State);
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
