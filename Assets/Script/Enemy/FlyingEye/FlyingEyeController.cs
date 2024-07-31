using Assets.Script.Bullet;
using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Enemy
{
    public class FlyingEyeController : EnemyController
    {
        protected override void Update()
        {
            base.Update();
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
            if (CanAttack3())
            {
                FSM.ChangeState(Attack3State);
                return;
            }
            //else if (CanAttack1())
            //{
            //    FSM.ChangeState(Attack1State);
            //    return;
            //}
            else if (PlayerDistance() > Data.ChaseDistance)
            {
                MoveToPlayer();
                return;
            }
            else
            {
                CheckIsFacingRight(PlayerDistanceX() > 0);
                if (notChaseDuringTime < Time.time -3f)
                {
                    notChaseDuringTime = Time.time;
                    RB.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                }
                return;
            }
        }
        #endregion
    }
}
