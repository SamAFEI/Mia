using UnityEngine;

namespace Assets.Script.Mia
{
    public class MiaStatesGroundBase : MiaState
    {
        public MiaStatesGroundBase(MiaController _player, MiaFSM _FSM, string _animName) : base(_player, _FSM, _animName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Player.IsDodging)
            {
                FSM.ChangeState(Player.DodgeState);
                return;
            }
            if (Player.IsThreeCombosing)
            {
                FSM.ChangeState(Player.ThreeCombosState);
                return;
            }
            if (Player.IsAttacking && !Player.IsSkilling)
            {
                FSM.ChangeState(Player.AttackState);
                return;
            }
            if (Player.IsJumping)
            {
                FSM.ChangeState(Player.JumpState);
                return;
            }
            if (!Player.IsOnGround)
            {
                FSM.ChangeState(Player.FallState);
                return;
            }

        }
    }
}
