using Assets.Script.Manager;
using UnityEngine;
namespace Assets.Script.Enemy
{
    public abstract class EnemyState
    {
        protected float stateTime { get; set; }
        public EnemyFSM FSM;
        public EnemyController Enemy;
        protected string animName;
        protected bool isAnimFinish;

        public EnemyState(EnemyController _enemy, EnemyFSM _FSM, string _animName)
        {
            Enemy = _enemy;
            FSM = _FSM;
            animName = _animName;
        }

        public virtual void OnEnter()
        {
            Enemy.CloseCanStunned();
            AnimatorPlay();
            isAnimFinish = false;
        }

        public virtual void OnUpdate()
        {
            if (GameManager.Instance.IsPaused) return;
            stateTime -= Time.deltaTime;
            if (GameManager.Instance.IsDie)
            {
                FSM.ChangeState(Enemy.IdleState);
                return;
            }
            if (Enemy.IsHurting && !Enemy.Data.IsSuperArmor)
            {
                FSM.ChangeState(Enemy.HurtState);
                return;
            }
        }

        public virtual void OnFixedUpdate()
        {
            if (GameManager.Instance.IsPaused) return;
        }
        public virtual void OnLateUpdate()
        {
            if (GameManager.Instance.IsPaused) return;
        }

        public virtual void OnExit()
        {
            if (GameManager.Instance.IsPaused) return;
        }
        public virtual void AnimatorPlay()
        {
            Enemy.animator.Play(animName);
        }
        public virtual void AnimationFinishTrigger()
        {
            isAnimFinish = true;
        }
    }
}