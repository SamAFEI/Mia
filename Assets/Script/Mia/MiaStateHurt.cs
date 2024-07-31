
using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStateHurt : MiaState
    {
        public MiaStateHurt(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            AudioManager.Instance.StopAttack();
            Player.StartCoroutine(Player.SuperArmor());
            Player.StartCoroutine(Player.HurtFlashFX());
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            if (Player.IsStunning)
            {
                FSM.ChangeState(Player.StunState);
                return;
            }
            if (!Player.IsHurting)
            {
                base.OnUpdate();
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
