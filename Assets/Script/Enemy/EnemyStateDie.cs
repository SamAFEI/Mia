using UnityEngine;

namespace Assets.Script.Enemy
{
    public class EnemyStateDie : EnemyState
    {
        private bool IsAnimatorPlay;
        public EnemyStateDie(EnemyController _enemy, EnemyFSM _FSM, string _animName) : base(_enemy, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            IsAnimatorPlay = true; //不要直接播放Die動畫
            base.OnEnter();
            IsAnimatorPlay = false;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            AnimatorPlay();
        }
        public override void AnimatorPlay()
        {
            if (Enemy.IsOnGround && !IsAnimatorPlay)
            {
                base.AnimatorPlay();
                IsAnimatorPlay = true;
                Enemy.RB.velocity = Vector2.zero;
                Object.Destroy(Enemy.gameObject, 1f);
            }
        }
    }
}
