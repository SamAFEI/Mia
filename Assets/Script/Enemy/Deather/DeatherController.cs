using Assets.Script.Bullet;
using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Enemy
{
    public class DeatherController : EnemyController
    {
        protected override void Awake()
        {
            base.Awake();
            IsStunAttack = true;
        }
        #region ATTACK METHODS
        public override void RangedAttack()
        {
            //base.RangedAttack();
            Vector3 vector = GameManager.Instance.Player.transform.position + new Vector3(0, 3, 0);
            GameObject gObj = Instantiate(Bullet, vector, Quaternion.identity);
            gObj.GetComponent<Spell>().Damage = Data.Attack3Damage;
        }
        #endregion
    }
}
