using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStatesAttackBase : MiaState
    {
        protected float damage;
        protected bool isStrong;
        public MiaStatesAttackBase(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            damage = 10;
            Player.SetAttacking(true);
            Player.SetAttackEffect(true);
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayAttack());
            Player.AttackDamage = damage;
            Player.IsHeavyAttack = false;
            isStrong = false;
            if (Player.InputX != 0)
            { Player.CheckDirectionToFace(Player.InputX > 0); } 
            //if (Player.IsOnGround)
            if (!Player.IsCircleHiting)
            {
                Player.RB.velocity = Vector2.zero;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Player.SetAttacking(false);
            Player.SetHeavyAttacking(false);
            Player.SetAttackEffect(false);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isAnimFinish && !Player.IsSkilling)
            {
                if (Player.IsOnGround)
                {
                    FSM.ChangeState(Player.IdleState);
                    return;
                }
                else
                {
                    FSM.ChangeState(Player.FallState);
                    return;
                }
            }
        }
    }
}
