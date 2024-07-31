using Assets.Script.Bullet;
using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Enemy
{
    public class GoblinController : EnemyController
    {
        #region ATTACK METHODS
        public override void RangedAttack()
        {
            //base.RangedAttack();
            Vector3 vector = GameManager.Instance.Player.transform.position - AttackPoint.position;
            vector = new Vector3(vector.x, vector.y * Data.Attack3BulletSpeed, 0);
            vector = vector.normalized;
            GameObject gObj = GameObject.Instantiate(Bullet, AttackPoint.position, Quaternion.identity);
            gObj.GetComponent<Rigidbody2D>().AddForce(vector * Data.Attack3BulletSpeed, ForceMode2D.Impulse);
            gObj.GetComponent<ProjectileBase>().Damage = Data.Attack3Damage;
        }
        #endregion

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
