using UnityEngine;
namespace Assets.Script.Mia
{
    public abstract class MiaState
    {
        protected float stateTime { get; set; }
        public MiaFSM FSM;
        public MiaController Player;
        private string animName;
        protected bool isAnimFinish;

        public MiaState(MiaController _player, MiaFSM _FSM, string _animName)
        {
            Player = _player;
            FSM = _FSM;
            animName = _animName;
        }

        public virtual void OnEnter()
        {
            isAnimFinish = false;
            AnimatorPlay();
        }

        public virtual void OnUpdate()
        {
            if (GameManager.Instance.IsPaused) return;
            stateTime -= Time.deltaTime;

            if (Player.CurrentHP <= 0)
            {
                FSM.ChangeState(Player.DieState);
                return;
            }
            if (Player.IsHurting)
            {
                FSM.ChangeState(Player.HurtState);
                return;
            }
            if (Player.IsDashing)
            {
                FSM.ChangeState(Player.DashState);
                return;
            }
            if (Player.IsDodging)
            {
                FSM.ChangeState(Player.DodgeState);
                return;
            }
            if (Player.IsCircleHiting && FSM.CurrentState != Player.CircleHitState)
            {
                FSM.ChangeState(Player.CircleHitState);
                return;
            }
            if (Player.IsShockWaving && FSM.CurrentState != Player.ShockWaveState)
            {
                FSM.ChangeState(Player.ShockWaveState);
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

        }
        public virtual void AnimatorPlay()
        {
            if (animName != "")
            {
                Player.animator.Play(animName);
            }
        }

        public virtual void AnimationFinishTrigger()
        {
            isAnimFinish = true;
        }
    }
}
