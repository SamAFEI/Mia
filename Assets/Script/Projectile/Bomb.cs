using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script.Bullet
{
    public class Bomb : ProjectileBase
    {
        #region COMPONENTS
        public Transform AttackPoint { get; private set; }
        #endregion
        public float lifeTime = 1f;

        protected override void Awake()
        {
            base.Awake();
            AttackPoint = this.transform.Find("Attack").GameObject().transform;
            HitCollider = AttackPoint.GetComponent<Collider2D>();
        }
        protected override void FixedUpdate()
        {
            lifeTime -= Time.fixedDeltaTime;
            if (lifeTime <= 0 )
            {
                RB.velocity = Vector3.zero;
                animator.Play("Attack1");
                Destroy(this.gameObject, 0.9f);
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                MiaController enemy = collision.GetComponent<MiaController>();
                //enemy.StartCoroutine(enemy.Hurt(Damage, this.transform));
            }
        }
    }
}
