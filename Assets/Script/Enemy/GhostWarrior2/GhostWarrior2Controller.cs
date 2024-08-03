using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Enemy.GhostWarrior2
{
    public class GhostWarrior2Controller : EnemyController
    {
        public float waitTime;
        public float LastWaitTime;
        public float durationAttack2Tiem;
        public bool IsAttack2;
        public float faceDir;
        protected override void Start()
        {
            base.Start();
            waitTime = 1f;
        }
        protected override void Update()
        {
            base.Update();
            LastWaitTime -= Time.deltaTime;
            durationAttack2Tiem -= Time.deltaTime;

            if (FSM.CurrentState == Attack2State)
            {
                RB.velocity = new Vector2(faceDir * 8f, RB.velocity.y);
                if (durationAttack2Tiem < 0)
                {
                    AnimationTrigger();
                }
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
            else if(CanAttack1())
            {
                FSM.ChangeState(Attack1State);
                return;
            }
            else if (CanAttack2())
            {
                durationAttack2Tiem = 2f;
                faceDir = 1;
                if (PlayerDistanceX() < 1) { faceDir = -1; } 
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
