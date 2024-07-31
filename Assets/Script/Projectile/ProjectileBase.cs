using UnityEngine;

namespace Assets.Script.Bullet
{
    public class ProjectileBase : MonoBehaviour
    {
        #region COMPONENTS
        public Animator animator { get; protected set; }
        public Rigidbody2D RB { get; protected set; }
        public Collider2D HitCollider { get; protected set; }
        #endregion

        public int Damage;
        float destroyTiem = 1000f;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            RB = GetComponent<Rigidbody2D>();
            HitCollider = GetComponent<Collider2D>();
        }
        protected virtual void FixedUpdate()
        {
            destroyTiem -= Time.deltaTime;
            if (destroyTiem < 0)
            {
                RB.velocity = Vector3.zero;
                animator.Play("Attack1");
                Destroy(this.gameObject, 0.4f);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                RB.velocity = Vector3.zero;
                animator.Play("Attack1");
                MiaController enemy = collision.GetComponent<MiaController>();
                //enemy.StartCoroutine(enemy.Hurt(Damage, this.transform));
                Destroy(this.gameObject, 0.4f);
            }else if (collision.tag == "Ground" || collision.tag == "Weapon")
            {
                RB.velocity = Vector3.zero;
                animator.Play("Attack1");
                Destroy(this.gameObject, 0.4f);
            }
        }
    }
}
