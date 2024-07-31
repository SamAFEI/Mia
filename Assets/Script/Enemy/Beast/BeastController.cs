using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Enemy.Beast
{
    public class BeastController : EnemyController
    {
        #region STATE METHODS
        public override void ChaseStateAction()
        {
            //base.ChaseStateAction();
            if (CanAttack2())
            {
                FSM.ChangeState(Attack2State);
                return;
            }
            else if (CanAttack1())
            {
                FSM.ChangeState(Attack1State);
                return;
            }
            else if (LastAttack1Time > 0)
            {
                AwayToPlayer();
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
        public void AwayToPlayer()
        {
            float posX = transform.position.x - GameManager.Instance.Player.transform.position.x;
            posX *= Random.Range(3f, 5f);
            Vector2 target = new Vector2(transform.position.x + posX, transform.position.y);
            CheckIsFacingRight(target.x > transform.position.x);
            RB.position = Vector2.MoveTowards(RB.position, target
                                            , Data.runSpeed * Time.deltaTime);
        }
    }
}
