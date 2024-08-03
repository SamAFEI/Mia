using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Enemy.GhostBoss
{
    public class GhostBossController : EnemyController
    {
        public float waitTime;
        public float LastWaitTime;

        protected override void Start()
        {
            base.Start();
            IsMode3 = true;
            waitTime = 1f;
        }
        protected override void Update()
        {
            base.Update();
            LastWaitTime -= Time.deltaTime;


            if (FSM.CurrentState == Attack1State)
            {
                RunSpeed = 20f;
            }
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
            else if (CanAttack2())
            {
                IsStunAttack = false;
                RunSpeed = 20f;
                FSM.ChangeState(Attack2State);
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
