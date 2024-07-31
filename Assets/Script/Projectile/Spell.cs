
using UnityEngine;

namespace Assets.Script.Bullet
{
    public class Spell : MonoBehaviour
    {
        #region COMPONENTS
        public Animator animator { get; protected set; }
        public Collider2D HitCollider { get; protected set; }
        #endregion

        public int Damage;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            HitCollider = GetComponent<Collider2D>(); 
            StartCoroutine(AudioManager.Instance.PlaySpell());
        }
        protected virtual void FixedUpdate()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                Destroy(this.gameObject);
            }
            
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                MiaController enemy = collision.GetComponent<MiaController>();
                //enemy.StartCoroutine(enemy.Hurt(Damage, this.transform));
            }
        }
    }
}
