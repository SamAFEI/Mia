using UnityEngine;

namespace Assets.Script.Enemy
{
    public class FlyingEyeController : EnemyController
    {
        public float waitTime;
        public float LastWaitTime;

        protected override void Start()
        {
            base.Start();
            IsMode1 = true;
            waitTime = Random.Range(2.0f,3.0f);
        }
        protected override void Update()
        {
            base.Update();
            LastWaitTime -= Time.deltaTime;
            if (IsDie)
            {
                RB.gravityScale = 1;
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        #region STATE METHODS
        public override void ChaseStateAction()
        {
            //base.ChaseStateAction();
            if (LastWaitTime < 0)
            {
                if (CanAttack3())
                {
                    LastWaitTime = waitTime;
                    FSM.ChangeState(Attack3State);
                    return;
                }
                if (PlayerDistance() > Data.ChaseDistance)
                {
                    float offsetY = Random.Range(0.0f,4.0f);
                    MoveToPlayer(offsetY);
                    return;
                }
            }
            else
            {
                CheckIsFacingRight(PlayerDistanceX() > 0);
                if (notChaseDuringTime < Time.time - 3f)
                {
                    notChaseDuringTime = Time.time;
                    RB.velocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                }
                return;
            }
        }
        #endregion
    }
}
